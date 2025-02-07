using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;

    private bool isShooting = false;
    private float attackDuration = 1f;
    private float attackTimer = 0f;

    [Header("Vida y Escudo")]
    [SerializeField] private BarraDeVida barraDeVida;
    [SerializeField] private BarraDeEscudo barraDeEscudo;

    private float vidaMaxima = 100f;
    private float escudoMaximo = 50f;
    private float vidaActual;
    private float escudoActual;

    private float tiempoUltimoAtaque = 0f;  // Temporizador para el último daño recibido
    private float tiempoDeRegeneracion = 3f;  // Tiempo que debe pasar sin recibir daño para iniciar regeneración
    private float velocidadRegeneracionEscudo = 2f; // Velocidad de regeneración del escudo por segundo
    private bool regenerandoEscudo = false;  // Indica si el escudo está regenerándose
    private float tiempoSinDaño = 0f; // Acumula el tiempo sin recibir daño

    private bool detenerRegeneracionEscudo = false; // Indica si la regeneración está detenida temporalmente
    private float tiempoDetenerRegeneracion = 30f; // Tiempo en el que la regeneración estará detenida después de recibir daño
    private float tiempoDetenerRegeneracionActual = 0f; // Temporizador que se utiliza para contar los 30 segundos

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        vidaActual = vidaMaxima;
        escudoActual = escudoMaximo;

        barraDeVida.InicializarBarraDeVida(vidaMaxima);
        barraDeEscudo.InicializarBarraDeEscudo(escudoMaximo); // Usamos el mismo script para el escudo
    }

    void Update()
    {
        // Movimiento del jugador
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Si NO está atacando, el personaje usa la dirección de movimiento para la animación
        if (!isShooting)
        {
            playerAnimator.SetFloat("Horizontal", moveX);
            playerAnimator.SetFloat("Vertical", moveY);
        }

        playerAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

        // Animación de disparo
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            isShooting = true;
            playerAnimator.SetBool("isShooting", true);
            attackTimer = 0f;

            // Mirar al ratón SOLO al atacar
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Asegurar que esté en 2D
            Vector2 lookDirection = (mousePosition - transform.position).normalized;

            playerAnimator.SetFloat("Horizontal", lookDirection.x);
            playerAnimator.SetFloat("Vertical", lookDirection.y);
        }

        if (isShooting)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                playerAnimator.SetBool("isShooting", false);
                isShooting = false;
            }
        }

        // Regeneración de escudo automática (solo después de que haya pasado un tiempo sin recibir daño)
        if (!detenerRegeneracionEscudo && escudoActual < escudoMaximo && tiempoSinDaño >= tiempoDeRegeneracion && !regenerandoEscudo)
        {
            StartCoroutine(RegenerarEscudo());
        }

        // Acumula el tiempo sin daño
        if (tiempoSinDaño < tiempoDeRegeneracion)
        {
            tiempoSinDaño += Time.deltaTime;
        }

        // Si la regeneración está detenida, aumenta el temporizador de espera
        if (detenerRegeneracionEscudo)
        {
            tiempoDetenerRegeneracionActual += Time.deltaTime;
            if (tiempoDetenerRegeneracionActual >= tiempoDetenerRegeneracion)
            {
                detenerRegeneracionEscudo = false; // Permitir que la regeneración se reanude
                tiempoDetenerRegeneracionActual = 0f; // Resetear el temporizador
            }
        }
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    // Método para recibir daño
    public void RecibirDaño(float cantidad)
    {
        if (escudoActual > 0)
        {
            escudoActual -= cantidad;
            if (escudoActual < 0)
            {
                float dañoRestante = Mathf.Abs(escudoActual); // Daño que pasa a la vida
                escudoActual = 0;
                RecibirDañoVida(dañoRestante);
            }
            barraDeEscudo.CambiarEscudoActual(escudoActual);
        }
        else
        {
            RecibirDañoVida(cantidad);
        }

        // Detener regeneración del escudo al recibir daño
        StopCoroutine(RegenerarEscudo());
        regenerandoEscudo = false;

        // Reiniciar el temporizador de regeneración
        tiempoSinDaño = 0f;  // Reiniciar el tiempo sin daño

        // Detener la regeneración del escudo por un tiempo de 30 segundos
        detenerRegeneracionEscudo = true;

        // Reiniciar el temporizador de 30 segundos
        tiempoDetenerRegeneracionActual = 0f;

        // Detener regeneración
        tiempoUltimoAtaque = Time.time;
    }

    private void RecibirDañoVida(float cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;
        barraDeVida.CambiarVidaActual(vidaActual);
    }

    // Método para curar vida
    public void CurarVida(float cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima) vidaActual = vidaMaxima;
        barraDeVida.CambiarVidaActual(vidaActual);
    }

    // Método para regenerar escudo manual
    public void RegenerarEscudoManual(float cantidad)
    {
        escudoActual += cantidad;
        if (escudoActual > escudoMaximo) escudoActual = escudoMaximo;
        barraDeEscudo.CambiarEscudoActual(escudoActual);
    }

    // Coroutine para la regeneración del escudo
    private IEnumerator RegenerarEscudo()
    {
        regenerandoEscudo = true;

        while (escudoActual < escudoMaximo)
        {
            escudoActual += velocidadRegeneracionEscudo * Time.deltaTime;
            if (escudoActual > escudoMaximo)
                escudoActual = escudoMaximo;

            barraDeEscudo.CambiarEscudoActual(escudoActual);
            yield return null;  // Pausa la ejecución de la coroutine hasta el siguiente frame
        }

        regenerandoEscudo = false;
    }
}