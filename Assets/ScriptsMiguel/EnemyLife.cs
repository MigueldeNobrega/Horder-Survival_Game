using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [SerializeField] private string enemyTag; // Asigna el tag del enemigo en el Inspector

    public void Die()
    {
        
        EnemyPoolManager.Instance.ReturnEnemy(enemyTag, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) // Si una bala lo golpea
        {
            Die();
        }
    }
}
