using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public void Die()
    {
        // En vez de destruir, regresamos el enemigo al pool
        EnemySpawnPool.Instance.ReturnEnemy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) // Si una bala lo golpea
        {
            Die();
        }
    }
}
