using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    public float separationRadius = 0.5f;    // Radio para detectar otros enemigos
    public float separationForce = 0.1f;     // Magnitud del desplazamiento para separarlos

    private Rigidbody2D rb;
    private int enemyLayer;                  // La capa en la que se encuentran los enemigos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("enemy"); // Asegúrate de que tus enemigos estén en la capa "enemy"
    }

    void FixedUpdate()
    {
        // Obtener todos los enemigos dentro del radio de separación
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationRadius, 1 << enemyLayer);
        Vector2 separationVector = Vector2.zero;
        int count = 0;

        foreach (Collider2D col in colliders)
        {
            if (col.gameObject != gameObject)
            {
                // Sumar la dirección opuesta al vector que conecta ambos enemigos
                separationVector += (Vector2)(transform.position - col.transform.position);
                count++;
            }
        }

        if (count > 0)
        {
            // Promediar la separación
            separationVector /= count;
            // Aplicar un pequeño desplazamiento
            Vector2 newPosition = rb.position + separationVector.normalized * separationForce;
            rb.MovePosition(newPosition);
        }
    }
}
