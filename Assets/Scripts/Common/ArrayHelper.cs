using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public static class ArrayHelper
{
    ///// <summary>
    ///// 方案3，接口作为参数，会造成代码的膨胀
    ///// </summary>
    //static public void OrderBy<T>(T[] array) where T:IComparable<T>
    //{
    //    for(int i = 0; i < array.Length; i++)
    //    {
    //        for(int j = i + 1; j < array.Length; j++)
    //        {
    //            if (array[i].CompareTo(array[j]) > 0)
    //            {
    //                var temp = array[i];
    //                array[i] = array[j];
    //                array[j] = temp;
    //            }
    //        }
    //    }
    //}

    ////有关于这里为什么是IComparer：IComparer是一个比较器接口，可以接受两个数据，可以在类的外部定义并通过实例化比较器调用，而IComparable接口就必须在类的内部定义，不适合作为参数传递
    //static public void OrderBy<T>(T[] array,IComparer<T> comparer)
    //{
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        for (int j = i + 1; j < array.Length; j++)
    //        {
    //            if (comparer.Compare(array[i],array[j]) > 0)
    //            {
    //                var temp = array[i];
    //                array[i] = array[j];
    //                array[j] = temp;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 方案4，委托作为参数,用匿名方法实现
    /// </summary>

    //升序
    static public void OrderBy<T,TKey>(T[] array,SelectHandler<T,TKey> handler) where TKey:IComparable<TKey>
    {
        for (int i = 0; i<array.Length; i++)
        {
            for (int j = i + 1; j<array.Length; j++)
            {
                if (handler(array[i]).CompareTo(handler(array[j]))>0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }

    //降序
    static public void OrderByDecending<T, TKey>(T[] array, SelectHandler<T, TKey> handler) where TKey : IComparable<TKey>
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (handler(array[i]).CompareTo(handler(array[j])) < 0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }

    //最大值
    static public T Max<T, TKey>(T[] array, SelectHandler<T, TKey> handler) where TKey : IComparable<TKey>
    {
        T temp = default(T);
        temp = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]).CompareTo(handler(temp)) > 0)
            {
                temp = array[i];
            }
        }
        return temp;
    }

    //最小值
    static public T Min<T, TKey>(T[] array, SelectHandler<T, TKey> handler) where TKey : IComparable<TKey>
    {
        T temp = default(T);
        temp = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]).CompareTo(handler(temp)) < 0)
            {
                temp = array[i];
            }
        }
        return temp;
    }

    //查找符合条件的一个
    static public T Find<T>(T[] array,FindHandler<T> handler)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]))
            {
                return array[i];
            }
        }
        return default(T);
    }

    //查找符合条件的所有
    static public T[] FindAll<T>(T[] array, FindHandler<T> handler)
    {
        List<T> temp = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]))
            {
                temp.Add(array[i]);
            }
        }

        //T[] retArry = new T[temp.Count];
        //for(int i = 0; i < array.Length; i++)
        //{
        //    retArry[i] = temp[i];
        //}
        //return retArry;

        return temp.ToArray();
    }

    //将需要的数据全部提取出来
    static public TKey[] Select<T,TKey>(T[] array, SelectHandler<T,TKey> handler)
    {
        TKey[] keys = new TKey[array.Length];
        for(int i = 0; i < array.Length; i++)
        {
            keys[i] = handler(array[i]);
        }

        return keys;
    }

}

//从T中取一个属性，返回属性的值
public delegate TKey SelectHandler<T,TKey>(T t);//从T对象返回某个属性的值

public delegate bool FindHandler<T>(T t);