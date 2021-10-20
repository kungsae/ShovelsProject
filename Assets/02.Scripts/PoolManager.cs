using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager
{
    public static Dictionary<string, object> pool = new Dictionary<string, object>();
    public static Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    public static void CreatPool<T>(GameObject prefab, Transform parent, int count = 5)
    {
        //어떤 플리팹 풀? 누구자식? 몇개?
        Queue<T> q = new Queue<T>();
        //풀은 큐로, T타입 큐로
        for (int i = 0; i < count; i++)
        {
            // 해당 큐에 프리팹 지정 갯수만큼 만들어 넣기 
            GameObject g = GameObject.Instantiate(prefab, parent);
            T t = g.GetComponent<T>();
            g.SetActive(false);
            q.Enqueue(t);
        }

        string key = typeof(T).ToString();
        pool[key] = q;
        prefabDictionary[key] = prefab;
    }

    public static T GetItem<T>() where T : MonoBehaviour
    {
        string key = typeof(T).ToString();
        T Item = null;
        if (pool.ContainsKey(key))
        {
            Queue<T> q = (Queue<T>)pool[key];

            T firstItemk = q.Peek();

            if (firstItemk.gameObject.activeSelf)
            {
                GameObject prefab = prefabDictionary[key];
                GameObject g = GameObject.Instantiate(prefab, firstItemk.transform.parent);
                Item = g.GetComponent<T>();
            }
            else
            {
                Item = q.Dequeue();
            }
            Item.gameObject.SetActive(true);
            q.Enqueue(Item);
        }
        return Item;
    }

}

