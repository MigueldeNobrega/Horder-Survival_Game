using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public float damageAmount = 10f; // Daño que inflige el enemigo
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.RecibirDaño(damageAmount);
                Debug.Log($"?? {gameObject.name} golpeó al jugador.");
                
            }
        }
    }
}
