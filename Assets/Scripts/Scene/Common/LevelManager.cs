using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSProject.Character;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class LevelManager : MonoBehaviour
{
    //场景ID
    private int sceneID;

    //单例模式
    public static LevelManager instance;

    //场景组件
    private GameObject levelManager;
    public Transform playerStart;
    public GameObject player;
    public GameObject monsterGroup;
    public List<GameObject> monsterList = new List<GameObject>();
    private List<GameObject> monsterAlive = new List<GameObject>();

    //界面组件
    public Transform transformUI;
    public Image image;
    public List<Text> texts;
    //显影速度
    public float speed;
    //控制所有玩家的输入
    public bool gameStart;
    //控制游戏暂停
    public bool gamePause = false;
    //开始界面之前不允许暂停等操作
    private bool isStarting;

    private void Awake()
    {
        levelManager = GameObject.Find("LevelManager");
        if(levelManager!=null)
        {
            instance = levelManager.GetComponent<LevelManager>();
            if(instance==null)
            {
                instance = levelManager.AddComponent<LevelManager>();
            }
        }
       else
        {
            levelManager = new GameObject("LevelManager");
            instance = levelManager.AddComponent<LevelManager>();
        }

        playerStart = GameObject.Find("PlayerStart").GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        monsterGroup = GameObject.Find("MonsterGroup");
        for (int i = 0; i < monsterGroup.transform.childCount; i++)
        {
            monsterList.Add(monsterGroup.transform.GetChild(i).gameObject);
        }
        InitPlayer();
        //意味着玩家控制的功能都被终止
        gameStart = false;
    }

    public void InitPlayer()
    {
        player = GameObject.Find("Player");
        transformUI = GameObject.Find("LevelCanvas").transform;
        player.transform.position = playerStart.transform.position;
        player.GetComponentInChildren<FPSMouseLook>().playerQuaternion = playerStart.transform.rotation;
    }


    private IEnumerator Start()
    {
        isStarting = true;
        //测试时注释，否则不能对单个场景进行调试
        sceneID = int.Parse(SceneManager.GetActiveScene().name.Remove(0, 5));
        yield return StartCoroutine(GameStart());
        //启用玩家控制
        if(!gamePause)
        {
            gameStart = true;
        }
        image.gameObject.SetActive(false);
        isStarting = false;
    }

    public IEnumerator Victory()
    {
        //禁用玩家控制
        gameStart = false;
        var nextScene = PlayerPrefs.GetString("appSceneBase")+"0" + (sceneID+1) + "/Scene0" + (sceneID + 1)+".unity";
        PlayerPrefs.SetString("NextScene", nextScene);
        var playerName = PlayerPrefs.GetString("Player");
        DbManager.CreateDataBase();
        DbManager.db.UpdateInto("PlayerData", new string[] { "Stage" }, new string[] { "2" },  "PlayerName" ,  "'" + playerName + "'" );
        DbManager.db.CloseSqlConnection();
        yield return GameVictory();
        SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
    }

    public IEnumerator Defeat()
    {
        //禁用玩家控制
        gameStart = false;
        yield return GameDefeat();
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if(!isStarting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePause = !gamePause;
                Pause(gamePause);
            }
        }
    }

    //运用场景异步加载的方式进行暂停界面的复用
    public void Pause(bool gamePause)
    {
        if(gamePause)
        {
            SceneManager.LoadScene("Assets/Scenes/ScenePause/ScenePause.unity", LoadSceneMode.Additive);
            gameStart = false;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            SceneManager.UnloadSceneAsync("Assets/Scenes/ScenePause/ScenePause.unity", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            gameStart = true;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void CheckVictory()
    {
        if(gameStart)
        {
            monsterAlive = monsterList.FindAll(o => o.GetComponent<CharacterStatus>().currentHp > 0);

            if (monsterAlive.Count == 0 && player.GetComponent<CharacterStatus>().currentHp > 0)
            {
                StartCoroutine(Victory());
            }
            else if (player.GetComponent<CharacterStatus>().currentHp <= 0)
            {
                StartCoroutine(Defeat());
            }
        }
    }

    //游戏开始
    public IEnumerator GameStart()
    {
        transformUI = GameObject.Find("LevelCanvas").transform;
        image = TransformHelper.FindChild(transformUI, "StartPanel").GetComponent<Image>();
        texts.Clear();
        for (int i=0;i<image.transform.childCount;i++)
        {
            texts.Add(image.transform.GetChild(i).GetComponent<Text>());
        }
        texts[0].text = "Mission " + sceneID;
        yield return FadeIn(image);
        foreach(var text in texts)
        {
            yield return FadeIn(text);
        }

        while(true)
        {
            yield return null;
            if(!gamePause)
            {
                if (!Input.GetKeyDown(KeyCode.Escape) && Input.anyKeyDown)
                {
                    break;
                }
            }
        }

        foreach (var text in texts)
        {
            yield return FadeOut(text);
        }
        yield return FadeOut(image);   
    }

    #region 角色胜利
    public IEnumerator GameVictory()
    {
        transformUI = GameObject.Find("LevelCanvas").transform;
        image = TransformHelper.FindChild(transformUI, "VictoryPanel").GetComponent<Image>();
        texts.Clear();
        for (int i = 0; i < image.transform.childCount; i++)
        {
            texts.Add(image.transform.GetChild(i).GetComponent<Text>());
        }
        yield return FadeIn(image);
        foreach (var text in texts)
        {
            yield return FadeIn(text);
        }

        while (true)
        {
            yield return null;
            if (Input.anyKeyDown)
            {
                break;
            }
        }

        foreach (var text in texts)
        {
            yield return FadeOut(text);
        }
        yield return FadeOut(image);
    }
    #endregion

    #region 角色死亡
    public IEnumerator GameDefeat()
    {
        transformUI = GameObject.Find("LevelCanvas").transform;
        image = TransformHelper.FindChild(transformUI, "DefeatPanel").GetComponent<Image>();
        var tmp_obj = TransformHelper.FindChild(image.transform, "Texts");
        texts.Clear();
        for (int i = 0; i < tmp_obj.childCount; i++)
        {
            texts.Add(tmp_obj.transform.GetChild(i).GetComponent<Text>());
        }

        yield return FadeIn(image);
        foreach (var text in texts)
        {
            yield return FadeIn(text);
        }

        tmp_obj = TransformHelper.FindChild(image.transform, "Buttons");
        List<Button> buttons = new List<Button>();
        for (int i = 0; i < tmp_obj.childCount; i++)
        {
            buttons.Add(tmp_obj.transform.GetChild(i).GetComponent<Button>());
        }

        //按钮注册，按照排序注册
        buttons[0].onClick.AddListener(ReturnToMenu);
        buttons[1].onClick.AddListener(ReTry);

        foreach (var button in buttons)
        {
            yield return FadeIn(button);
        }
    }

    public void ReTry()
    {
        var nextScene = PlayerPrefs.GetString("appSceneBase") + "0" + (sceneID) + "/Scene0" + (sceneID) + ".unity";
        PlayerPrefs.SetString("NextScene", nextScene);
        SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
    }

    public void  ReturnToMenu()
    {
        var nextScene = PlayerPrefs.GetString("appSceneBase") + "Start/SceneStart.unity";
        PlayerPrefs.SetString("NextScene", nextScene);
        SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
    }
    #endregion

    public IEnumerator FadeIn(object obj)
    {
        if(obj is Image)
        {
            var tmp_obj = obj as Image;
            while(tmp_obj.color.a<0.7)
            {
                yield return null;
                var r = tmp_obj.color.r;
                var g = tmp_obj.color.g;
                var b = tmp_obj.color.b;
                var a = tmp_obj.color.a + Time.deltaTime * speed;
                tmp_obj.color = new Color(r, g, b, a);
            }
        }
        else if (obj is Text)
        {
            var tmp_obj = obj as Text;
            while (tmp_obj.color.a < 0.7)
            {
                yield return null;
                var r = tmp_obj.color.r;
                var g = tmp_obj.color.g;
                var b = tmp_obj.color.b;
                var a = tmp_obj.color.a + Time.deltaTime * speed;
                tmp_obj.color = new Color(r, g, b, a);
            }
        }
        else if(obj is Button)
        {
            var tmp_obj = obj as Button;
            while (tmp_obj.gameObject.GetComponent<Image>().color.a < 0.7)
            {
                yield return null;
                var r = tmp_obj.gameObject.GetComponent<Image>().color.r;
                var g = tmp_obj.gameObject.GetComponent<Image>().color.g;
                var b = tmp_obj.gameObject.GetComponent<Image>().color.b;
                var a = tmp_obj.gameObject.GetComponent<Image>().color.a + Time.deltaTime * speed;
                tmp_obj.gameObject.GetComponent<Image>().color = new Color(r, g, b, a);
                r = tmp_obj.gameObject.GetComponentInChildren<Text>().color.r;
                g = tmp_obj.gameObject.GetComponentInChildren<Text>().color.g;
                b = tmp_obj.gameObject.GetComponentInChildren<Text>().color.b;
                a = tmp_obj.gameObject.GetComponentInChildren<Text>().color.a + Time.deltaTime * speed;
                tmp_obj.gameObject.GetComponentInChildren<Text>().color = new Color(r, g, b, a);
            }
        }

    }

    public IEnumerator FadeOut(object obj)
    {
        if (obj is Image)
        {
            var tmp_obj = obj as Image;
            while (tmp_obj.color.a > 0.1)
            {
                yield return null;
                var r = tmp_obj.color.r;
                var g = tmp_obj.color.g;
                var b = tmp_obj.color.b;
                var a = tmp_obj.color.a - Time.deltaTime * speed;
                tmp_obj.color = new Color(r, g, b, a);
            }
        }
        else if (obj is Text)
        {
            var tmp_obj = obj as Text;
            while (tmp_obj.color.a > 0.1)
            {
                yield return null;
                var r = tmp_obj.color.r;
                var g = tmp_obj.color.g;
                var b = tmp_obj.color.b;
                var a = tmp_obj.color.a - Time.deltaTime * speed;
                tmp_obj.color = new Color(r, g, b, a);
            }
        }
        else if (obj is Button)
        {
            var tmp_obj = obj as Button;
            while (tmp_obj.gameObject.GetComponent<Image>().color.a > 0.1)
            {
                yield return null;
                var r = tmp_obj.gameObject.GetComponent<Image>().color.r;
                var g = tmp_obj.gameObject.GetComponent<Image>().color.g;
                var b = tmp_obj.gameObject.GetComponent<Image>().color.b;
                var a = tmp_obj.gameObject.GetComponent<Image>().color.a + Time.deltaTime * speed;
                tmp_obj.gameObject.GetComponent<Image>().color = new Color(r, g, b, a);
                r = tmp_obj.gameObject.GetComponentInChildren<Text>().color.r;
                g = tmp_obj.gameObject.GetComponentInChildren<Text>().color.g;
                b = tmp_obj.gameObject.GetComponentInChildren<Text>().color.b;
                a = tmp_obj.gameObject.GetComponentInChildren<Text>().color.a + Time.deltaTime * speed;
                tmp_obj.gameObject.GetComponentInChildren<Text>().color = new Color(r, g, b, a);
            }
        }

    }
}
