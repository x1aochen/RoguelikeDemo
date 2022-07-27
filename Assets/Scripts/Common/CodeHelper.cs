using System;
using UnityEngine;
using Random = System.Random;

public static class CodeHelper
{
    /// <summary>
    /// 在此物体的孩子中找到指定物体
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindChild(Transform trans, string name)
    {
        Transform child = trans.Find(name);

        if (child != null)
        {
            return child;
        }

        Transform go;

        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, name);

            if (go != null)
            {
                return go;
            }
        }

        return null;
    }

    /// <summary>
    /// 在此物体上得到或添加某个组件
    /// </summary>
    /// <typeparam name="T">组件类型，必须继承Component，否则无法Add</typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T AddOrGetComponent<T>(GameObject obj)
        where T : Component
    {
        return obj.TryGetComponent(out T tmp) ? tmp : obj.AddComponent<T>();
    }

    public static Component AddOrGetComponent(Type type,GameObject obj)
    {

        return obj.TryGetComponent(type, out Component tmp) ? tmp : obj.AddComponent(type);
    }

    /// <summary>
    /// 随机数
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static int Random(int minValue,int maxValue)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        return random.Next(minValue, maxValue);
    }
    //TODO: 浮点型的取随机？
    public static float Random(float minValue,float maxValue)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        return random.Next(Mathf.RoundToInt(minValue), Mathf.RoundToInt(maxValue));
    }

}

