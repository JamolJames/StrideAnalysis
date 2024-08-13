// using UnityEngine;
// using System.Collections.Generic;

// public class PoolManager : MonoBehaviour
// {
//     public GameObject squarePrefab;
//     public int poolSize = 1; // Set the pool size to 1 for this example
//     private List<GameObject> pool;
//     private int currentIndex = 0;

//     void Start()
//     {
//         pool = new List<GameObject>();

//         for (int i = 0; i < poolSize; i++)
//         {
//             GameObject obj = Instantiate(squarePrefab);
//             obj.SetActive(false);
//             pool.Add(obj);
//         }
//     }

//     public GameObject GetPooledObject()
//     {
//         GameObject obj = pool[currentIndex];
//         currentIndex = (currentIndex + 1) % poolSize;

//         // Ensure the object is returned regardless of its active state
//         return obj;
//     }
// }

using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public GameObject squarePrefab;
    public int poolSize = 1; // Set the pool size to 1 for this example
    private List<GameObject> pool;
    private int currentIndex = 0;

    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(squarePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        GameObject obj = pool[currentIndex];
        currentIndex = (currentIndex + 1) % poolSize;

        // Ensure the object is returned regardless of its active state
        return obj;
    }
}
