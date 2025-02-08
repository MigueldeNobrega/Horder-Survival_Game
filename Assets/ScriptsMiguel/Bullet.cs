using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb; // Cambié el nombre de la variable

    public float speed = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Usamos el nuevo nombre
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // En lugar de destruir, devolvemos la bala al pool
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
