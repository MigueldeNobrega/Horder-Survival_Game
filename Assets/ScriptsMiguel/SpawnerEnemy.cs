using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private string enemyTag; // El tag del enemigo que se spawnea

    // Método para spawnar un enemigo
    public void SpawnEnemy()
    {
        // Verificar que el EnemyPoolManager existe
        if (EnemyPoolManager.Instance == null)
        {
            Debug.LogError("? EnemyPoolManager.Instance es NULL.");
            return;
        }

        // Intentamos obtener un enemigo del pool
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyTag, transform.position);

        // Verificar que hemos recibido un enemigo del pool
        if (enemy == null)
        {
            Debug.LogError($"? No se ha obtenido un enemigo del EnemyPoolManager con el tag {enemyTag}.");
            return;
        }

        // Obtener el componente Enemy
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent == null)
        {
            Debug.LogError($"? El enemigo con tag {enemyTag} no tiene el componente 'Enemy'.");
            return;
        }

        // Reseteamos la vida del enemigo para asegurar que comienza con su salud completa
        enemyComponent.ResetHealth();

        // Asegurarnos de que el enemigo sea activado si es necesario
        enemy.SetActive(true);

        // Restaurar físicas y colisiones en caso de que estuvieran desactivadas tras la muerte
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        if (enemyCollider != null) enemyCollider.enabled = true;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;
    }
}
