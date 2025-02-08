using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwEnemy : MonoBehaviour
{
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
            if (EnemySpawnPool.Instance != null)
            {
                EnemySpawnPool.Instance.GetEnemy(transform.position);
            }
            else
            {
                Debug.LogError("EnemyPool.Instance es NULL. Asegúrate de que EnemyPool está en la escena.");
            }

            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
