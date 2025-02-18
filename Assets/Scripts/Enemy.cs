using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configuraci�n General")]
    public Transform playerToFollow; // Referencia al jugador
    public float stopDistance = 0.5f; // Distancia m�nima antes de detenerse
    public float speed = 2f; // Velocidad del enemigo
    public float damageAmount = 10f; // Da�o que inflige el enemigo
    public float attackInterval = 1.5f; // Intervalo entre ataques (segundos)
    public int maxHealth = 50; // Vida m�xima del enemigo

    private int currentHealth; // Vida actual
    private float lastAttackTime = 0f; // Control del tiempo del �ltimo ataque

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
            Debug.LogError($"? No se encontr� el hitbox de ataque en {gameObject.name}");
        }
        else
        {
            attackCollider.enabled = false; // Se desactiva al inicio
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return;  // Si est� muerto, no hace nada
        if (playerToFollow == null || currentHealth <= 0)
            return; // Si el jugador no est� o el enemigo est� muerto, no hacer nada

        // ?? Bloquea el movimiento y ataque si est� en Hurt
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

            // Direcci�n del movimiento
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

            // Si est� cerca y es momento de atacar
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

    // Recibir da�o
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Si ya est� muerto, no hacer nada

        currentHealth -= damage;
        animator.SetBool("isHurt", true);

        // ?? Detener el movimiento mientras est� en Hurt
        rb.velocity = Vector2.zero;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo para la muerte del enemigo
    private void Die()
    {
        animator.SetBool("isDeath", true);
        rb.velocity = Vector2.zero; // Detener el movimiento
                                    // Desactivar el Collider para que no reciba m�s colisiones
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        // Desactivar el Rigidbody para que no lo mueva la f�sica
        if (rb != null)
        {
            rb.simulated = false;
        }

        // Desactivar la hitbox para que no pueda hacer da�o
        DesactivarHitbox();
        this.enabled = false; // Desactivar el script
    }

    // Eventos de animaci�n
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
