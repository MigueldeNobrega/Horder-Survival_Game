using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private Rigidbody2D rb;
    private ProyectilPooling pool;
    private AudioSource audioSource; // Referencia al AudioSource
    private float speed = 10f;
    private float lifeTime = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Obtener el AudioSource
        GameObject spawnObject = GameObject.Find("ProyectilSpawn"); // Busca el objeto por su nombre
        if (spawnObject != null)
        {
            pool = spawnObject.GetComponent<ProyectilPooling>(); // Obtiene el script de la pool
        }
        else
        {
            Debug.LogError("No se encontró el GameObject 'ProyectilSpawn' con el script ProyectilPooling.");
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
        audioSource.Play(); // Reproducir el sonido al lanzar el proyectil
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
        if (other.isTrigger) return; //  Ignorar triggers (como el del ataque del zombie)

        // Verifica si impactó contra un enemigo
        Enemy enemigo = other.GetComponent<Enemy>();
        if (enemigo != null)
        {
            Debug.Log("Impacto en el enemigo, aplicando daño.");
            enemigo.TakeDamage(10); // Aplica daño (ajusta según sea necesario)
        }

        // Desactivar el proyectil al colisionar con algo válido
        Deactivate();
    }
}
