using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectPool : MonoSingleton<GameObjectPool>
{
    private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
    
    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="key">对象名</param>
    /// <param name="prefab">对象预制体</param>
    /// <param name="pos">位置</param>
    /// <param name="rotate">旋转</param>
    /// <returns></returns>
    public GameObject CreateObject(string key,GameObject prefab,Vector3 pos,Quaternion rotate)
    {
        GameObject go = null;
        
        if (pool.ContainsKey(key))
        {
            //查找可用对象
            go = pool[key].Find(go => !go.activeSelf);
        }
        if (go)
        {
            go.SetActive(true);
        }
        else
        {
            //GameObject prefab = Resources.Load<GameObject>(key);
            go = Instantiate(prefab);
            Add(key, go);
        }

        go.transform.position = pos;
        go.transform.rotation = rotate;
        return go;
    }

    private void Add(string key,GameObject go)
    {
        //查找是否有需要的键
        if (!pool.ContainsKey(key))
        {
            pool.Add(key, new List<GameObject>());
        }
        pool[key].Add(go);
        DontDestroyOnLoad(go);
    }


    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="go"></param>
    /// <param name="delay">延时时间</param>
    public void RecycleObject(GameObject go,float delay = 0)
    {
        if (delay == 0)
            go.SetActive(false);
        else
        {
            StartCoroutine(RecycleObjectDelay(go, delay));
        }    
    }


    private IEnumerator RecycleObjectDelay(GameObject go,float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    /// <summary>
    /// 删除某个类别
    /// </summary>
    /// <param name="key"></param>
    public void Clear(string key)
    {
        if (pool.ContainsKey(key))
        {
            foreach (var go in pool[key])
            {
                Destroy(go);
            }

            pool.Remove(key);
        }
    }

    //private void OnDestroy()
    //{
    //    foreach (var key in pool.Keys)
    //    {
    //        Clear(key);
    //    }
    //    StopAllCoroutines();
    //}
}
