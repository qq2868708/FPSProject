using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;

[SerializeField]
public class StartMenu : MonoBehaviour
{
    //数据库
    private DbAccess db;
    [SerializeField]
    private string appDBPath;

    //当前用户名
    public string playerName;

    //一级界面
    [SerializeField]
    private GameObject UI_01;
    //二级界面
    [SerializeField]
    private GameObject UI_02;

    //提示信息
    public Text warningText;

    //储存了打开顺序，用于关闭界面
    public List<string> sceneList = new List<string>();

    public ToggleGroup toggleGroup;

    //用于实例化的Toggle预制件
    public GameObject dataButton;
    public Transform dataList;

    [SerializeField]
    private GameObject activeToggle;

    private void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        appDBPath = Application.streamingAssetsPath + "/Test.db";
#elif UNITY_ANDROID || UNITY_IPHONE
        appDBPath = Application.persistentDataPath + "/Test/db";
        if(!File.Exists(appDBPath))
        {
           yield return CopyDataBase();
        }
#endif
        HideSecondPanel();
        PlayerPrefs.SetString("appDBDathPath", appDBPath);
        PlayerPrefs.SetString("appSceneBase", "Assets/Scenes/Scene");
        PlayerPrefs.SetString("NextScene", "Assets/Scenes/Scene01/Scene01.unity");
        //在后续场景中使用dbmanager管理数据库
        DbManager.appDBPath = appDBPath;
        //统计游戏场景的数目
        var count = SceneManager.sceneCountInBuildSettings-3;
        //Debug.Log(count);
        PlayerPrefs.SetInt("SceneCount", count);
    }

    #region 开始游戏按钮
    //开始游戏
    public void GameStartClick()
    {
        int b= SearchPlayerData();
        if(b>=1)
        {
            CallSecondPanel("NewGameMenu");
        }
        else
        {
            SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
        }


    }

    //已有存档，仍然开始游戏
    public void NewName()
    {
        //显示二级标题，名字
        CallSecondPanel("NewName");
    }

    //如果输入的名字可用，则加载新场景
    public void StartNewGame()
    {
        if(playerName!="")
        {
            if (!FindData(playerName))
            {
                InsertData(playerName);
                PlayerPrefs.SetString("Player", playerName);
                SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
            }
        }
    }

    //设置提示信息
    public void CleanWarningText()
    {
        warningText.text = "";
    }


    //检查名字的使用情况，并显示提示信息
    public void CheckName(string a)
    {
        if (a.Contains(" "))
        {
            warningText.color = Color.red;
            warningText.text = "用户名不能包含空格";
            return;
        }
        bool b = FindData(a);
        if (b)
        {
            warningText.color = Color.red;
            warningText.text = "用户名已被占用";
        }
        else
        {
            warningText.color = Color.green;
            warningText.text = "用户名可用";
            playerName = a;
        }
    }

    #endregion


    #region 继续游戏按钮
    //继续游戏
    public void GameContinueClick()
    {
        int b = SearchPlayerData();
        if (b==1)
        {
            CreateDataBase();
            SqliteDataReader reader = db.ReadFullTable("PlayerData");
            reader.Read();
            playerName = reader.GetString(reader.GetOrdinal("PlayerName"));
            PlayerPrefs.SetString("Player", playerName);
            int sceneID = reader.GetInt32(reader.GetOrdinal("Stage"));
            var nextScene = PlayerPrefs.GetString("appSceneBase") + "0" + (sceneID) + "/Scene0" + (sceneID) + ".unity";
            PlayerPrefs.SetString("NextScene", nextScene);
            db.CloseSqlConnection();
            SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
        }
        else if(b==0)
        {
            CallSecondPanel("ContinueMenu");
        }
        else
        {
            //根据记录的数目显示出来
            CallSecondPanel("ChooseData");
            CreateDataBase();
            SqliteDataReader reader = db.ReadFullTable("PlayerData");
            float tmp_size = 0;
            while(reader.Read())
            {
                var tmp = Instantiate(dataButton);
                tmp.name = tmp.name.Remove(tmp.name.Length - 7);
                tmp.transform.SetParent(dataList);
                tmp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tmp.GetComponent<Toggle>().group = toggleGroup;
                tmp.GetComponent<Toggle>().onValueChanged.AddListener(ChooseData);
                string tmp_Name = reader.GetString(reader.GetOrdinal("PlayerName"));
                tmp.GetComponentInChildren<Text>().text = tmp_Name;
                if(reader.GetInt32(reader.GetOrdinal("Stage"))>PlayerPrefs.GetInt("SceneCount"))
                {
                    tmp.GetComponentInChildren<Text>().text += " （通关）";
                }
                tmp_size += tmp.GetComponent<RectTransform>().sizeDelta.y;
                if (toggleGroup.GetComponent<RectTransform>().sizeDelta.y < tmp_size)
                {
                    toggleGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(toggleGroup.GetComponent<RectTransform>().sizeDelta.x, tmp_size);
                }
            }
            db.CloseSqlConnection();
            TransformHelper.FindChild(UI_02.transform, "DataScrollbar").GetComponent<Scrollbar>().value = 1;
        }
    }

    //点击存档开始游戏
    public void ContinueGame()
    {
        playerName = activeToggle.GetComponentInChildren<Text>().text;
        playerName = playerName.Split(' ')[0];
        PlayerPrefs.SetString("Player", playerName);
        Debug.Log(playerName);
        CreateDataBase();
        SqliteDataReader reader = db.Select("PlayerData", "PlayerName", "'" + playerName + "'");
        reader.Read();
        int sceneID = reader.GetInt32(reader.GetOrdinal("Stage"));
        if (sceneID > PlayerPrefs.GetInt("SceneCount"))
        {
            var tmp = TransformHelper.FindChild(this.transform, "MessageBox");
            tmp.gameObject.SetActive(true);
            return;
        }
        db.CloseSqlConnection();
        var nextScene = PlayerPrefs.GetString("appSceneBase") + "0" + (sceneID) + "/Scene0" + (sceneID) + ".unity";
        PlayerPrefs.SetString("NextScene", nextScene);
        //加载场景
        SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
    }

    //删除存档
    public void Delete()
    {
        playerName = activeToggle.GetComponentInChildren<Text>().text;
        playerName = playerName.Split(' ')[0];
        DeleteData(playerName);
    }

    //当选择了存档
    public void ChooseData(bool value)
    {
        var toggle= UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if(toggle.GetComponentInChildren<Toggle>()==null)
        {
            return;
        }
        if(toggle.GetComponent<Toggle>().isOn)
        {
            activeToggle = toggle;
        }
    }

    //更新菜单
    public void CleanToggle()
    {
        for(int i=0;i<toggleGroup.transform.childCount;i++)
        {
            Destroy(toggleGroup.transform.GetChild(i).gameObject);
        }
    }

    public void HideMessageBox()
    {
        var tmp = TransformHelper.FindChild(this.transform, "MessageBox");
        tmp.gameObject.SetActive(false);
    }
    #endregion


    #region 数据库相关
    //创建数据库
    private void CreateDataBase()
    {
        db = new DbAccess("URI=file:" + appDBPath);
    }

    //如果是移动平台，拷贝数据库
    public IEnumerator CopyDataBase()
    {
        WWW www = new WWW(Application.streamingAssetsPath + "/Test.db");
        yield return www;
        BinaryWriter writer = new BinaryWriter(new FileStream(appDBPath, FileMode.Create));
        for(int i=0;i<www.bytes.Length;i++)
        {
            writer.Write(www.bytes[i]);
        }
    }

    //查找有没有用户数据
    private int SearchPlayerData()
    {
        int i = 0;
        CreateDataBase();
        SqliteDataReader reader= db.ExistTables("PlayerData");
        if (reader.Read())
        {
            SqliteDataReader tmp_reader = db.DataCount("PlayerData");
            while(tmp_reader.Read())
            {
                i++;
            }
        }
        else
        {
            db.CreateTable("PlayerData", new string[] { "PlayerID", "PlayerName" }, new string[] { "int", "text" });
            i = 0;
        }
        db.CloseSqlConnection();
        return i;
    }

    //查找记录
    public bool FindData(string Name)
    {
        bool b;
        CreateDataBase();
        SqliteDataReader reader = db.Select("PlayerData", "PlayerName", "=", "'" + Name + "'");
        b = reader.Read();
        db.CloseSqlConnection();
        return b;
    }

    //删除记录
    public void DeleteData(string Name)
    {
        CreateDataBase();
        db.Delete("PlayerData", new string[] { "PlayerName" }, new string[] { "'"+Name+"'" });
        db.CloseSqlConnection();
        RefreshData();
    }

    public void RefreshData()
    {
        CleanToggle();
        CreateDataBase();
        SqliteDataReader reader = db.ReadFullTable("PlayerData");
        while(reader.Read())
        {
            var tmp = Instantiate(dataButton);
            tmp.name = tmp.name.Remove(tmp.name.Length - 7);
            tmp.transform.SetParent(dataList);
            tmp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            tmp.GetComponent<Toggle>().group = toggleGroup;
            tmp.GetComponent<Toggle>().onValueChanged.AddListener(ChooseData);
            string tmp_Name = reader.GetString(reader.GetOrdinal("PlayerName"));
            tmp.GetComponentInChildren<Text>().text = tmp_Name;
        }
        db.CloseSqlConnection();
    }

    public void InsertData(string Name)
    {
        CreateDataBase();
        string tmp = "'" + Name + "'";
        Debug.Log(tmp);
        db.InsertInto("PlayerData", new string[] {"NULL",tmp,"1" });
        db.CloseSqlConnection();
    }
    #endregion

    #region 界面操作的通用方法
    //显示二级界面
    public void CallSecondPanel(string widgetName)
    {
        UI_02.SetActive(true);
        for(int i=0; i< UI_02.transform.childCount;i++)
        {
            UI_02.transform.GetChild(i).gameObject.SetActive(false);
        }
        var tmp_widget = UI_02.transform.Find(widgetName);
        tmp_widget.gameObject.SetActive(true);
        sceneList.Add(widgetName);
    }

    //隐藏二级界面
    public void HideSecondPanel()
    {
        UI_02.SetActive(false);
    }


    //返回的公共方法
    public void Return()
    {
        sceneList.Remove(sceneList[sceneList.Count - 1]);
        if (sceneList.Count != 0)
        {
            CallSecondPanel(sceneList[sceneList.Count - 1]);
            sceneList.Remove(sceneList[sceneList.Count - 1]);
        }
        else
        {
            UI_02.SetActive(false);
        }

        CleanToggle();
    }

    #endregion


}
