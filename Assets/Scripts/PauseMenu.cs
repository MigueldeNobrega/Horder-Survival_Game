using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseButton;
    public Button resumeButton;
    public Button menuButton;
    public Button restartButton;

   // private bool isPaused = false;

    void Start()
    {
        
        /*pauseMenuUI.SetActive(false);

        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(TogglePause);
        menuButton.onClick.AddListener(GoToMenu);
        restartButton.onClick.AddListener(RestartGame);*/
    }

   /* public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenuUI.SetActive(true);
        pauseButton.gameObject.SetActive(!isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null); // Evita que quede seleccionado un botón
            Debug.Log("Juego PAUSADO");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Juego REANUDADO");
        }
    }*/

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseMenuUI.SetActive(false);

    }





    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
