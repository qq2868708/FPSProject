using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;

/// <summary>
///
/// </summary>
public class Test : MonoBehaviour
{
    private string appDBPath;

    private DbAccess db;

    private string name;
    private int id;

    private void CreateDataBase()
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
        db = new DbAccess("URI=file:" + appDBPath);
    }

    public IEnumerator CopyDataBase()
    {
        WWW www = new WWW(Application.streamingAssetsPath + "/Test.db");
        yield return www;
        File.WriteAllBytes(appDBPath, www.bytes);
    }

    public void CreateTable()
    {
        CreateDataBase();
        db.CreateTable("Role", new string[] { "id", "name", "age", "lv", "exp" }, new string[] { "int", "text", "int", "int", "float" });
        db.CloseSqlConnection();
    }

    public void InsertData()
    {
        CreateDataBase();
        db.InsertInto("Role", new string[] { "1", "'张三'", "18", "10", "100.0" });
        db.CloseSqlConnection();
    }

    public void UpdateData()
    {
        CreateDataBase();
        //从哪个表格，更新哪个字段，更新哪条记录，他的值是多少
        db.UpdateInto("Role", new string[] { "name", "lv", "exp" }, new string[] { "'数据库'", "1", "1.1" }, "id", "1");
        db.CloseSqlConnection();
    }

    public void DeleteData()
    {
        CreateDataBase();
        //从那个表格，删除那个字段为哪个值的记录
        db.Delete("Role", new string[] { "id" }, new string[] { "1" });
        db.CloseSqlConnection();
    }

    public void DeleteAllData()
    {
        CreateDataBase();
        db.DeleteContents("Role");
        db.CloseSqlConnection();
    }

    public void FindData()
    {
        CreateDataBase();
        SqliteDataReader reader = db.Select("Role", "id","=", "1");
        Debug.Log( reader.Read());
        name = reader.GetString(reader.GetOrdinal("name"));
        id = reader.GetInt32(reader.GetOrdinal("id"));
        db.CloseSqlConnection();
    }
    

    private void OnGUI()
    {
        if(GUILayout.Button("CreateDataBase"))
        {
            CreateDataBase();
        }
        if (GUILayout.Button("CreateTable"))
        {
            CreateTable();
        }
        if (GUILayout.Button("Insert"))
        {
            InsertData();
        }
        if (GUILayout.Button("Update"))
        {
            UpdateData();
        }
        if (GUILayout.Button("Delete"))
        {
            DeleteData();
        }
        if (GUILayout.Button("DeleteAll"))
        {
            DeleteAllData();
        }
        if(GUILayout.Button("Select"))
        {
            FindData();
        }

        GUILayout.Label(name);
        GUILayout.Label(id.ToString());
    }
}
