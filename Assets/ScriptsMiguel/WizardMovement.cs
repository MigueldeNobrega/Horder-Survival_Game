using UnityEngine;

public class WizardMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private bool isShooting = false;
    private float attackDuration = 1f; // Duraci�n de la animaci�n de disparo
    private float attackTimer = 0f;
    private Vector2 shootDirection; // Direcci�n del disparo

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Actualizar animaci�n de movimiento
        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

        // Detectar clic izquierdo para disparar
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            isShooting = true;
            attackTimer = 0f;

            // Calcular la direcci�n del disparo
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shootDirection = (mousePosition - transform.position).normalized;

            // Configurar la animaci�n de disparo en la direcci�n adecuada
            playerAnimator.SetFloat("ShootHorizontal", shootDirection.x);
            playerAnimator.SetFloat("ShootVertical", shootDirection.y);
            playerAnimator.SetBool("isShooting", true);
        }

        // Control del temporizador para la animaci�n de ataque
        if (isShooting)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                playerAnimator.SetBool("isShooting", false);
                isShooting = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Movimiento del jugador
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}
