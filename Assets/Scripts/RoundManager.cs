using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Configuración de Rondas")]
    public TMP_Text roundText; // Referencia al texto en el Canvas
    public int currentRound = 1;
    public int baseEnemiesPerRound = 10; // En la Ronda 1 salen 10
    public int extraEnemiesPerRound = 2; // Cada ronda aumenta en 2 enemigos
    public float timeBetweenRounds = 5f; // Tiempo de espera antes de la siguiente ronda

    private int enemiesRemaining;
    private bool isSpawning = false;

    [SerializeField] private List<SpawnerEnemy> spawners; // Lista de spawners en la escena

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartNextRound());
    }

    private IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(timeBetweenRounds);

        int enemiesToSpawn = baseEnemiesPerRound + (extraEnemiesPerRound * (currentRound - 1));
        enemiesRemaining = enemiesToSpawn;
        isSpawning = true;

        // Actualizar el texto del número de ronda
        if (roundText != null)
            roundText.text = "Ronda: " + currentRound;

        StartCoroutine(SpawnEnemies(enemiesToSpawn));
    }

    private IEnumerator SpawnEnemies(int count)
    {
        int spawned = 0;

        while (spawned < count)
        {
            foreach (SpawnerEnemy spawner in spawners)
            {
                if (spawned >= count) break;
                spawner.SpawnEnemy(); // Asegúrate de que los spawners generen enemigos correctamente
                spawned++;
                yield return new WaitForSeconds(0.5f); // Controla la velocidad de spawn
            }
        }

        isSpawning = false;
    }

    public void EnemyDied()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0 && !isSpawning)
        {
            currentRound++;
            StartCoroutine(StartNextRound());
        }
    }
}
