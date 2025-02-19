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
    public int maxHealth = 20; // Vida máxima del enemigo

    private int currentHealth; // Vida actual
    private float lastAttackTime = 0f; // Control del tiempo del último ataque
    
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D attackCollider; // Hitbox de ataque

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy"), LayerMask.NameToLayer("enemy"), false);
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
            Debug.LogError($"No se encontró el hitbox de ataque en {gameObject.name}");
        }
        else
        {
            attackCollider.enabled = false; // Se desactiva al inicio
        }
    }

    void FixedUpdate()
    {

        if (currentHealth <= 0) return;  // Si está muerto, no hace nada
        if (playerToFollow == null || currentHealth <= 0)
            return; // Si el jugador no está o el enemigo está muerto, no hacer nada

        //  Bloquea el movimiento y ataque si está en Hurt
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
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, playerToFollow.position, speed * Time.fixedDeltaTime);
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

        //  Detener el movimiento mientras está en Hurt
        rb.velocity = Vector2.zero;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para la muerte del enemigo
    private void Die()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.EnemyDied(); // Notificar que el enemigo murió
        }

        animator.SetBool("isDeath", true);
        rb.velocity = Vector2.zero;

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        if (rb != null)
        {
            rb.simulated = false;
        }

        DesactivarHitbox();

        //  Iniciar una corrutina para esperar la animación
        StartCoroutine(DeactivateAfterDeath());
    }

    private IEnumerator DeactivateAfterDeath()
    {
        //  Esperar hasta que termine la animación de muerte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        //  Devolver el enemigo al pool en lugar de destruirlo
        EnemyPoolManager.Instance.ReturnEnemy(gameObject.tag, gameObject);
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

    // Agregamos el ResetHealth
    public void ResetHealth()
    {
        currentHealth = maxHealth;

        if (animator != null)
        {
            ResetHurt();
        }
        else
        {
            Debug.LogError($" No se puede resetear 'isHurt' porque 'animator' es NULL en {gameObject.name}");
        }
    }

    //HOLA
    public void ResetHurt()
    {
        if (animator != null)
        {
            animator.SetBool("isHurt", false);
        }
        else
        {
            Debug.LogError($"? Animator es NULL en {gameObject.name}. Asegúrate de que el objeto tiene un componente Animator.");
        }
    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
    }
}
