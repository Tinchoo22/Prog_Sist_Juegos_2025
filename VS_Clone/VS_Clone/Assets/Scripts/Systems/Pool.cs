using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int preload = 10;

    private readonly Queue<GameObject> pool = new();
    private Transform runtimeRoot;

    private void Awake()
    {
      
        runtimeRoot = new GameObject($"{name}_Runtime").transform;
        runtimeRoot.SetParent(transform, false);

        for (int i = 0; i < preload; i++)
        {
            var go = Instantiate(prefab);
            go.transform.SetParent(runtimeRoot, false);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    public GameObject Get()
    {
        GameObject go = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        go.transform.SetParent(runtimeRoot, false);
        go.SetActive(true);
        return go;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(runtimeRoot, false);
        pool.Enqueue(go);
    }
}
