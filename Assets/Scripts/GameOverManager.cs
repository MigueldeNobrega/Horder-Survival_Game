using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Ocultar la pantalla de Game Over al inicio
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // Mostrar la pantalla de Game Over
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal"); // Cargar la escena del menú principal
    }
}
