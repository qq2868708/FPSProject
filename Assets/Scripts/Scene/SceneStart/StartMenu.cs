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

        PlayerPrefs.SetString("NextScene", "Assets/Scenes/Scene01/Scene01.unity");
    }

    #region 开始游戏按钮
    //开始游戏
    public void GameStartClick()
    {
        bool b= SearchPlayerData();
        if(b)
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
        if (FindData(playerName))
        {
            SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
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
        bool b = SearchPlayerData();
        if (b)
        {
            SceneManager.LoadScene("Assets/Scenes/SceneLoading/SceneLoading.unity");
        }
        else
        {
            CallSecondPanel("Cou");
        }
        
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
    private bool SearchPlayerData()
    {
        bool b;
        CreateDataBase();
        SqliteDataReader reader= db.ExistTables("PlayerData");
        if (reader.Read())
        {
            SqliteDataReader tmp_reader = db.DataCount("PlayerData");
            if (tmp_reader.Read())
            {
                b= true;
            }
            else
            {
                b= false;
            }
        }
        else
        {
            db.CreateTable("PlayerData", new string[] { "PlayerID", "PlayerName" }, new string[] { "int", "text" });
            b= false;
        }
        db.CloseSqlConnection();
        return b;
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
    }

    #endregion


}
