using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 对象池，用于管理对象和复用
/// </summary>
public class GameObjectPool : MonoBehaviour
{
    //单例模式
    public static GameObjectPool instance;
    private GameObject pool;

    public Dictionary<string, List<GameObject>> dict = new Dictionary<string, List<GameObject>>();


    private void Awake()
    {
        instance = GetComponent<GameObjectPool>();
        if (instance == null)
        {
            pool = new GameObject("GameObjectPool");
            instance = pool.AddComponent<GameObjectPool>();
        }
    }

    public GameObject CreatObject(string objName, GameObject obj)
    {
        //如果对象池已经管理了该对象
        if (dict.ContainsKey(objName))
        {
            //查找有没有没有使用的空闲对象
            var tmp_list = dict[objName];
            var tmp_obj = CheckUsage(tmp_list);
            if (tmp_obj)
            {
                //找到了空闲对象，则显示
                tmp_obj.SetActive(true);
                return tmp_obj;
            }
            else
            {
                //没有空闲对象，则新建一个对象并交给对象池统一管理
                tmp_obj = Instantiate(obj);
                tmp_list.Add(tmp_obj);
                tmp_obj.transform.parent = this.transform;
                tmp_obj.SetActive(true);
                return tmp_obj;
            }
        }
        else
        {
            //该类对象没有交给对象池管理，则在对象池字典中注册，并产生一个新的对象返回给调用者
            dict.Add(objName, new List<GameObject>());
            var tmp_obj = Instantiate(obj);
            dict[objName].Add(tmp_obj);
            tmp_obj.transform.parent = this.transform;
            tmp_obj.SetActive(true);
            return tmp_obj;
        }
    }

    //检测有无空闲对象
    public GameObject CheckUsage(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    //回收对象
    public void CollectGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    //延时回收对象
    public void CollectGameObject(GameObject obj,float time)
    {
        StartCoroutine(CollectDelay(obj, time));
    }

    //无法在其他对象中启用协程，做成私有并公开方法
    private IEnumerator CollectDelay(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        CollectGameObject(obj);
    }
}
