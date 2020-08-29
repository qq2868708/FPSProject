using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///层级未知，寻找子物体,提供两个方法：返回所有同名子物体；返回第一个找到的子物体
/// </summary>
public class TransformHelper:MonoBehaviour
{
    public static List<GameObject> gameObjects;
    public static GameObject gameobject;

    static TransformHelper()
    {
        gameObjects = new List<GameObject>();
        gameobject = null;
    }

    public static void FindChildsWithName(Transform TF,string name)
    {
        int count = TF.transform.childCount;
        if (count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Transform target = TF.transform.GetChild(i);
                if (target.name == name)
                {
                    gameObjects.Add(target.gameObject);
                    continue;
                }
                else
                {
                    FindChildsWithName(target, name);
                }
            }
        }
        
    }

    public static List<GameObject> GetChildsWithName(Transform TF,string name)
    {
        gameObjects.Clear();
        FindChildsWithName(TF, name);
        return gameObjects;
    }

    public static GameObject FindChildWithName(Transform TF, string name)
    { 
        int count = TF.transform.childCount;

        if (gameobject != null)
        {
            return gameobject;
        }

        for (int i = 0; i < count; i++)
        {
            gameobject = GetChildWithName(TF.GetChild(i), name);
            if (gameobject != null)
            {
                return gameobject;
            }
        }

        return null;
    }

    public static GameObject GetChildWithName(Transform TF,string name)
    {
        gameobject = null;
        FindChildWithName(TF, name);
        return gameobject;
    }

    //Unity2017
    public static Transform FindChild(Transform transform,string goName)
    {
        Transform child = transform.Find(goName);
        if (child != null)
        {
            return child;
        }

        Transform go;
        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            go = FindChild(child, goName);
            if(go!=null)
            {
                return go;
            }
        }
        return null;
    }

    public static void LookAtTarget(Vector3 target,Transform transform,float rotateSpeed)
    {
        if (target != Vector3.zero)
        {
            Quaternion dir = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotateSpeed);
        }
    }

}
