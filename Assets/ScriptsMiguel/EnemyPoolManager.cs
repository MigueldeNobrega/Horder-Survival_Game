using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetEnemy(string tag, Vector3 spawnPosition)
    {
        if (!poolDictionary.ContainsKey(tag) || poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("No hay un pool con el tag: " + tag);
            Debug.LogWarning($"No hay enemigos disponibles en el pool para el tag: {tag}");
            return null;
        }

        GameObject enemy = poolDictionary[tag].Dequeue();

        if (enemy == null)
        {
            Debug.LogWarning("Pool vacío para el tag: " + tag);
            return null;
        }

        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(string tag, GameObject enemy)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Intentando devolver un objeto a un pool inexistente: " + tag);
            return;
        }

        enemy.SetActive(false);
        poolDictionary[tag].Enqueue(enemy);
    }
}
