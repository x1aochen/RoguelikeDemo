using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectPool : MonoSingleton<GameObjectPool>
{
    private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
    
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="key">������</param>
    /// <param name="prefab">����Ԥ����</param>
    /// <param name="pos">λ��</param>
    /// <param name="rotate">��ת</param>
    /// <returns></returns>
    public GameObject CreateObject(string key,GameObject prefab,Vector3 pos,Quaternion rotate)
    {
        GameObject go = null;
        
        if (pool.ContainsKey(key))
        {
            //���ҿ��ö���
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
        //�����Ƿ�����Ҫ�ļ�
        if (!pool.ContainsKey(key))
        {
            pool.Add(key, new List<GameObject>());
        }
        pool[key].Add(go);
        DontDestroyOnLoad(go);
    }


    /// <summary>
    /// ���ն���
    /// </summary>
    /// <param name="go"></param>
    /// <param name="delay">��ʱʱ��</param>
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
    /// ɾ��ĳ�����
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
