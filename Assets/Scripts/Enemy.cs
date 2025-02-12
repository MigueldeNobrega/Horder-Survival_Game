using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private GameObject panelDeSangre;
    public Transform playerToFollow;    // Referencia al jugador que seguirá el zombie
    public float stopDistance = 0.1f;   // Distancia mínima a la que el zombie se detiene
    public float speed = 10f;           // Velocidad de movimiento del zombie
    public float damageAmount = 5f;     // Daño que inflige el zombie al jugador
    public float attackInterval = 2f;   // Intervalo de tiempo entre ataques en segundos
    private float lastAttackTime = 0f;  // Momento del último ataque

    private Animator animator;
    private Rigidbody2D rb;             // Referencia al Rigidbody2D para el control físico
    private CircleCollider2D attackCollider; // Collider para el área de daño del zombie

    private PlayerMovement playerMovement; // Referencia al script del jugador para controlar la regeneración del escudo

    void Start()
    {
        animator = GetComponent<Animator>();  // Obtiene el Animator
        rb = GetComponent<Rigidbody2D>();     // Obtiene el Rigidbody2D

        if (playerToFollow == null)
        {
            // Si no se asignó el jugador en el Inspector, buscarlo con su tag
            playerToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Añadir un Collider como trigger para detectar el daño
        attackCollider = gameObject.AddComponent<CircleCollider2D>();
        attackCollider.isTrigger = true;  // Configuramos como trigger (no colisiona físicamente)
        attackCollider.radius = 1f;  // Tamaño del área de daño
        attackCollider.offset = Vector2.zero;  // Ajustamos el centro del collider al zombie

        // Obtener la referencia al script PlayerMovement del jugador
        playerMovement = playerToFollow.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerToFollow != null)
        {
            // Obtiene la posición actual del zombie y la posición del jugador
            Vector2 currentPosition = transform.position;

            // Siempre sigue al jugador, sin verificar el rango de visión
            if (Vector2.Distance(currentPosition, playerToFollow.position) > stopDistance)
            {
                // Calcula la nueva posición a mover utilizando MoveTowards
                Vector2 newPosition = Vector2.MoveTowards(currentPosition, playerToFollow.position, speed * Time.deltaTime);

                // Mueve al zombie usando el Rigidbody2D para aplicar física correctamente
                rb.MovePosition(newPosition);

                // Calcula la dirección del movimiento
                Vector2 moveDirection = newPosition - currentPosition;

                // Normaliza la dirección para tener un valor consistente
                moveDirection.Normalize();

                // Actualiza los parámetros del Animator para las animaciones del movimiento
                animator.SetFloat("Horizontal", moveDirection.x);
                animator.SetFloat("Vertical", moveDirection.y);
                animator.SetBool("IsMoving", moveDirection.magnitude > 0.01f);  // Activa la animación si el zombie se mueve
            }
            else
            {
                animator.SetBool("IsMoving", false);  // Detiene la animación de movimiento si está cerca del jugador
            }

            // Verificar si ha pasado el tiempo suficiente para realizar otro ataque
            if (Vector2.Distance(transform.position, playerToFollow.position) <= attackCollider.radius)
            {
                // Si el jugador está dentro del rango de ataque, hacer el ataque si ha pasado el intervalo de tiempo
                if (Time.time - lastAttackTime >= attackInterval)
                {
                    OnAttack();
                    lastAttackTime = Time.time; // Actualiza el tiempo del último ataque
                }
            }
        }
    }

    // Método para realizar el ataque al jugador
    private void OnAttack()
    {
        // Usamos el Collider para verificar si el jugador está dentro del área de ataque del zombie
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackCollider.radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Si el zombie entra en contacto con el jugador, aplicar daño
                PlayerMovement player = collider.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.RecibirDaño(damageAmount);  // Aplica el daño al jugador
                    Debug.Log("Daño recibido por el jugador.");
                       panelDeSangre.SetActive(true);
                   
                }
            }
        }
    }

    // Método para dibujar el rango de ataque del zombie en la vista de escena (solo para depuración)
    private void OnDrawGizmos()
    {
        if (attackCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackCollider.radius); // Dibuja el círculo del rango de ataque
        }
    }
}
