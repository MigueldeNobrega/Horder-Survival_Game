using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Rigidbody2D rb;
    private ProyectilPooling pool;
    private float speed = 10f;
    private float lifeTime = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject spawnObject = GameObject.Find("ProyectilSpawn"); // Busca el objeto por su nombre
        if (spawnObject != null)
        {
            pool = spawnObject.GetComponent<ProyectilPooling>(); // Obtiene el script de la pool
        }
        else
        {
            Debug.LogError("? No se encontró el GameObject 'ProyectilSpawn' con el script ProyectilPooling.");
        }
    }

    public void Launch(Vector2 direction)
    {
        Collider2D playerCollider = GameObject.FindWithTag("Player")?.GetComponent<Collider2D>();
        Collider2D myCollider = GetComponent<Collider2D>();

        if (playerCollider != null && myCollider != null)
        {
            Physics2D.IgnoreCollision(myCollider, playerCollider, true);
        }
        rb.velocity = direction * speed;
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void Deactivate()
    {
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
        pool.ReturnProjectile(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return; // ?? Ignorar triggers (como el del ataque del zombie)

        // ?? Si en el futuro el enemigo tiene un método de daño, lo puedes llamar aquí
        Debug.Log("?? Impacto en el enemigo");

        // Desactivar el proyectil al colisionar con algo válido
        Deactivate();
    }
}
