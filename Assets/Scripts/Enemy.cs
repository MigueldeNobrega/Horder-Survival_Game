using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private GameObject panelDeSangre;
    public Transform playerToFollow;    // Referencia al jugador que seguir� el zombie
    public float stopDistance = 0.1f;   // Distancia m�nima a la que el zombie se detiene
    public float speed = 10f;           // Velocidad de movimiento del zombie
    public float damageAmount = 5f;     // Da�o que inflige el zombie al jugador
    public float attackInterval = 2f;   // Intervalo de tiempo entre ataques en segundos
    private float lastAttackTime = 0f;  // Momento del �ltimo ataque

    private Animator animator;
    private Rigidbody2D rb;             // Referencia al Rigidbody2D para el control f�sico
    private CircleCollider2D attackCollider; // Collider para el �rea de da�o del zombie

    private PlayerMovement playerMovement; // Referencia al script del jugador para controlar la regeneraci�n del escudo

    void Start()
    {
        animator = GetComponent<Animator>();  // Obtiene el Animator
        rb = GetComponent<Rigidbody2D>();     // Obtiene el Rigidbody2D

        if (playerToFollow == null)
        {
            // Si no se asign� el jugador en el Inspector, buscarlo con su tag
            playerToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // A�adir un Collider como trigger para detectar el da�o
        attackCollider = gameObject.AddComponent<CircleCollider2D>();
        attackCollider.isTrigger = true;  // Configuramos como trigger (no colisiona f�sicamente)
        attackCollider.radius = 1f;  // Tama�o del �rea de da�o
        attackCollider.offset = Vector2.zero;  // Ajustamos el centro del collider al zombie

        // Obtener la referencia al script PlayerMovement del jugador
        playerMovement = playerToFollow.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerToFollow != null)
        {
            // Obtiene la posici�n actual del zombie y la posici�n del jugador
            Vector2 currentPosition = transform.position;

            // Siempre sigue al jugador, sin verificar el rango de visi�n
            if (Vector2.Distance(currentPosition, playerToFollow.position) > stopDistance)
            {
                // Calcula la nueva posici�n a mover utilizando MoveTowards
                Vector2 newPosition = Vector2.MoveTowards(currentPosition, playerToFollow.position, speed * Time.deltaTime);

                // Mueve al zombie usando el Rigidbody2D para aplicar f�sica correctamente
                rb.MovePosition(newPosition);

                // Calcula la direcci�n del movimiento
                Vector2 moveDirection = newPosition - currentPosition;

                // Normaliza la direcci�n para tener un valor consistente
                moveDirection.Normalize();

                // Actualiza los par�metros del Animator para las animaciones del movimiento
                animator.SetFloat("Horizontal", moveDirection.x);
                animator.SetFloat("Vertical", moveDirection.y);
                animator.SetBool("IsMoving", moveDirection.magnitude > 0.01f);  // Activa la animaci�n si el zombie se mueve
            }
            else
            {
                animator.SetBool("IsMoving", false);  // Detiene la animaci�n de movimiento si est� cerca del jugador
            }

            // Verificar si ha pasado el tiempo suficiente para realizar otro ataque
            if (Vector2.Distance(transform.position, playerToFollow.position) <= attackCollider.radius)
            {
                // Si el jugador est� dentro del rango de ataque, hacer el ataque si ha pasado el intervalo de tiempo
                if (Time.time - lastAttackTime >= attackInterval)
                {
                    OnAttack();
                    lastAttackTime = Time.time; // Actualiza el tiempo del �ltimo ataque
                }
            }
        }
    }

    // M�todo para realizar el ataque al jugador
    private void OnAttack()
    {
        // Usamos el Collider para verificar si el jugador est� dentro del �rea de ataque del zombie
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackCollider.radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Si el zombie entra en contacto con el jugador, aplicar da�o
                PlayerMovement player = collider.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.RecibirDa�o(damageAmount);  // Aplica el da�o al jugador
                    Debug.Log("Da�o recibido por el jugador.");
                       panelDeSangre.SetActive(true);
                   
                }
            }
        }
    }

    // M�todo para dibujar el rango de ataque del zombie en la vista de escena (solo para depuraci�n)
    private void OnDrawGizmos()
    {
        if (attackCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackCollider.radius); // Dibuja el c�rculo del rango de ataque
        }
    }
}
