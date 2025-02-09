using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwEnemy : MonoBehaviour
{
    [SerializeField] private string enemyTag; // Asigna en el Inspector el tag del enemigo a spawnnear
    [SerializeField] private float minimumSpawnTime = 2f;
    [SerializeField] private float maximumSpawnTime = 5f;

    private float timeUntilSpawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            if (EnemyPoolManager.Instance != null)
            {
                EnemyPoolManager.Instance.GetEnemy(enemyTag, transform.position);
            }
            else
            {
                Debug.LogError("EnemyPoolManager.Instance es NULL. Asegúrate de que está en la escena.");
            }

            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
