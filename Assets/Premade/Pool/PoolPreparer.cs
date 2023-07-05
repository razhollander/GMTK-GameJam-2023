using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PoolPreparer : MonoBehaviour
{
    [SerializeField]
    PooledMonobehaviour[] prefabs;

    //[SerializeField]
    //private int initialPoolSize = 100;

    private void Start()
    {
        foreach (var prefab in prefabs)
        {
            if (prefab == null)
            {
                Debug.LogError("Null prefab in PoolPreparer");
            }
            else
            {
                PooledMonobehaviour poolablePrefab = prefab.GetComponent<PooledMonobehaviour>();
                if (poolablePrefab == null)
                {
                    Debug.LogError("Prefab does not contain an IPoolable and can't be pooled");
                }
                else
                {
                    Pool.GetPool(poolablePrefab).GrowPool();
                }
            }
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        List<GameObject> prefabsToRemove = new List<GameObject>();
        if (prefabs!=null)
        {
            foreach (var prefab in prefabs.Where(t => t != null))
            {
                    if (PrefabUtility.GetPrefabAssetType(prefab) != PrefabAssetType.Regular)//change
                    {
                        Debug.LogError(string.Format("{0} is not a prefab.  It has been removed.", prefab.gameObject.name));
                        prefabsToRemove.Add(prefab.gameObject);
                    }

                    PooledMonobehaviour poolablePrefab = prefab.GetComponent<PooledMonobehaviour>();
                    if (poolablePrefab == null)
                    {
                        Debug.LogError("Prefab does not contain an IPoolable and can't be pooled.  It has been removed.");
                        prefabsToRemove.Add(prefab.gameObject);
                    }
            }
            List<PooledMonobehaviour> newPrefabs = new List<PooledMonobehaviour>();

            for (int i = 0; i < prefabs.Length; i++)
            {
                if (prefabs[i] != null)
                {
                    if (!prefabsToRemove.Contains(prefabs[i].gameObject))
                        newPrefabs.Add(prefabs[i]);
                }
                else
                    newPrefabs.Add(prefabs[i]);
            }
            prefabs = newPrefabs.ToArray();
        }
#endif

    }
}