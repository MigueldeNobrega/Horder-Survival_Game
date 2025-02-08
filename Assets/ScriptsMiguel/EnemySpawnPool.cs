using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    public static EnemySpawnPool Instance;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 3; // Tamaño inicial del pool

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        // Crear enemigos y desactivarlos
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 spawnPosition)
    {
        GameObject enemy;

        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue(); // Sacamos un enemigo del pool
        }
        else
        {
            // Si el pool está vacío, creamos uno nuevo
            enemy = Instantiate(enemyPrefab);
        }

        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
