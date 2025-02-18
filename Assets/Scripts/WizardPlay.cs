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
    [SerializeField] private GameObject panelDeSangre;
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes

    private float vidaMaxima = 100f;
    private float escudoMaximo = 50f;
    private float vidaActual;
    private float escudoActual;

    private float tiempoDeRegeneracion = 5f;  // Tiempo que debe pasar sin recibir daño para iniciar regeneración
    private float velocidadRegeneracionEscudo = 2f; // Velocidad de regeneración del escudo por segundo
    private bool regenerandoEscudo = false;  // Indica si el escudo está regenerándose
    private float tiempoSinDaño = 0f; // Acumula el tiempo sin recibir daño

    private bool detenerRegeneracionEscudo = false; // Indica si la regeneración está detenida temporalmente
    private float tiempoDetenerRegeneracion = 5f; // Tiempo en el que la regeneración estará detenida después de recibir daño
    private float tiempoDetenerRegeneracionActual = 0f; // Temporizador que se utiliza para contar los 30 segundos

    private Coroutine shieldRegenCoroutine; // Referencia a la coroutine de regeneración del escudo

    [Header("Disparo")]
    [SerializeField] private Transform puntoDisparo; // Empty donde aparece el proyectil
    private ProyectilPooling proyectilPool;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        // Buscar la pool de proyectiles en "ProyectilSpawn"
        GameObject spawnObject = GameObject.Find("ProyectilSpawn");
        if (spawnObject != null)
        {
            proyectilPool = spawnObject.GetComponent<ProyectilPooling>();
        }
        else
        {
            Debug.LogError("⚠ No se encontró el GameObject 'ProyectilSpawn' con el script ProyectilPooling.");
        }

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
            Vector2 direction = (mousePosition - transform.position).normalized;

            playerAnimator.SetFloat("Horizontal", direction.x);
            playerAnimator.SetFloat("Vertical", direction.y);

            // 🔥 Disparar proyectil
            Disparar(direction);
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
            shieldRegenCoroutine = StartCoroutine(RegenerarEscudo());
            panelDeSangre.SetActive(false);
        }

        // Acumula el tiempo sin daño
        if (!detenerRegeneracionEscudo)
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
                tiempoSinDaño = 0f; // Reiniciar el tiempo sin daño
            }
        }
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    // 🎯 MÉTODO PARA DISPARAR
    private void Disparar(Vector2 direction)
    {
        GameObject proyectil = proyectilPool.GetProjectile();
        if (proyectil != null)
        {
            proyectil.transform.position = puntoDisparo.position;
            proyectil.transform.rotation = Quaternion.identity;
            proyectil.SetActive(true);
            proyectil.GetComponent<Proyectil>().Launch(direction);
        }
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
        if (shieldRegenCoroutine != null)
        {
            StopCoroutine(shieldRegenCoroutine);
            shieldRegenCoroutine = null;
        }
        regenerandoEscudo = false;

        // Reiniciar el temporizador de regeneración
        tiempoSinDaño = 0f;  // Reiniciar el tiempo sin daño

        // Detener la regeneración del escudo por un tiempo de 30 segundos
        detenerRegeneracionEscudo = true;

        // Reiniciar el temporizador de 30 segundos
        tiempoDetenerRegeneracionActual = 0f;
    }

    private void RecibirDañoVida(float cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;
        barraDeVida.CambiarVidaActual(vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        // Aquí puedes agregar animaciones o efectos antes de destruir el GameObject
        Destroy(gameObject);

        // Mostrar la pantalla de Game Over
        GameOverManager gameOverManager = GameObject.Find("Canvas").GetComponent<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
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

            // Si se ha detenido la regeneración (por recibir daño), salir del bucle
            if (detenerRegeneracionEscudo)
            {
                regenerandoEscudo = false;
                yield break;
            }
        }

        regenerandoEscudo = false;
        shieldRegenCoroutine = null;
    }
}
