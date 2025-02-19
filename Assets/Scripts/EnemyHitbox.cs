using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public float damageAmount = 10f; // Da�o que inflige el enemigo
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.RecibirDa�o(damageAmount);
                Debug.Log($"?? {gameObject.name} golpe� al jugador.");
                
            }
        }
    }
}
