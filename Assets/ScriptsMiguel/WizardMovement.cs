using System.Collections;
using UnityEngine;

public class WizardMovement : MonoBehaviour
{
        [SerializeField] private float speed = 3f;
        private Rigidbody2D playerRb;
        private Vector2 moveInput;
        private Animator playerAnimator;
        private bool isShooting = false;
        private float attackDuration = 1f;  // Duraci�n de la animaci�n de disparo (aj�stalo a la duraci�n real de tu animaci�n)
        private float attackTimer = 0f;
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
            playerAnimator.SetFloat("Horizontal", moveX);
            playerAnimator.SetFloat("Vertical", moveY);
            playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);
            // Detectar clic izquierdo
            if (Input.GetMouseButtonDown(0) && !isShooting)  // 0 es el bot�n izquierdo del rat�n
            {
                isShooting = true;
                playerAnimator.SetBool("isShooting", true);  // Activa la animaci�n de disparo
                attackTimer = 0f;  // Resetea el temporizador
            }
            // Control del temporizador para la animaci�n de ataque
            if (isShooting)
            {
                attackTimer += Time.deltaTime;  // Suma el tiempo transcurrido
                if (attackTimer >= attackDuration)  // Si ha pasado el tiempo de la animaci�n
                {
                    playerAnimator.SetBool("isShooting", false);  // Detiene la animaci�n de disparo
                    isShooting = false;  // Restablece el estado para permitir otro ataque
                }
            }
        }
        private void FixedUpdate()
        {
            // Movimiento del jugador
            playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
        }
    }

