using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configuración General")]
    public Transform playerToFollow; // Referencia al jugador
    public float stopDistance = 0.5f; // Distancia mínima antes de detenerse
    public float speed = 2f; // Velocidad del enemigo
    public float damageAmount = 10f; // Daño que inflige el enemigo
    public float attackInterval = 1.5f; // Intervalo entre ataques (segundos)
    public int maxHealth = 50; // Vida máxima del enemigo

    private int currentHealth; // Vida actual
    private float lastAttackTime = 0f; // Control del tiempo del último ataque

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D attackCollider; // Hitbox de ataque

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Iniciar con vida completa

        if (playerToFollow == null)
        {
            playerToFollow = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        // Buscar el hitbox de ataque en un GameObject hijo
        attackCollider = transform.Find("Hitbox")?.GetComponent<Collider2D>();
        if (attackCollider == null)
        {
            Debug.LogError($"? No se encontró el hitbox de ataque en {gameObject.name}");
        }
        else
        {
            attackCollider.enabled = false; // Se desactiva al inicio
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return;  // Si está muerto, no hace nada
        if (playerToFollow == null || currentHealth <= 0)
            return; // Si el jugador no está o el enemigo está muerto, no hacer nada

        // ?? Bloquea el movimiento y ataque si está en Hurt
        if (animator.GetBool("isHurt"))
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 currentPosition = transform.position;
        float distanceToPlayer = Vector2.Distance(currentPosition, playerToFollow.position);

        if (distanceToPlayer > stopDistance)
        {
            // Movimiento del enemigo
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, playerToFollow.position, speed * Time.deltaTime);
            rb.MovePosition(newPosition);

            // Dirección del movimiento
            Vector2 moveDirection = newPosition - currentPosition;
            moveDirection.Normalize();

            // Actualizar animaciones
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetBool("IsMoving", moveDirection.magnitude > 0.01f);
        }
        else
        {
            animator.SetBool("IsMoving", false);

            // Si está cerca y es momento de atacar
            if (Time.time - lastAttackTime >= attackInterval)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
            }
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            DesactivarHitbox();
        }
    }

    // Recibir daño
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Si ya está muerto, no hacer nada

        currentHealth -= damage;
        animator.SetBool("isHurt", true);

        // ?? Detener el movimiento mientras está en Hurt
        rb.velocity = Vector2.zero;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para la muerte del enemigo
    private void Die()
    {
        animator.SetBool("isDeath", true);
        rb.velocity = Vector2.zero; // Detener el movimiento
                                    // Desactivar el Collider para que no reciba más colisiones
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        // Desactivar el Rigidbody para que no lo mueva la física
        if (rb != null)
        {
            rb.simulated = false;
        }

        // Desactivar la hitbox para que no pueda hacer daño
        DesactivarHitbox();
        this.enabled = false; // Desactivar el script
    }

    // Eventos de animación
    public void ActivarHitbox()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }

    public void DesactivarHitbox()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
    //HOLA
    public void ResetHurt()
    {
        animator.SetBool("isHurt", false);
    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
    }
}
