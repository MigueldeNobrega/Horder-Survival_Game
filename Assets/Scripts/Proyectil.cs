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
        pool = FindObjectOfType<ProyectilPooling>();  // Encuentra la pool en la escena
    }

    public void Launch(Vector2 direction)
    {
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
        // Aquí puedes poner lo que pasa cuando golpea algo
        Deactivate();
    }
}
