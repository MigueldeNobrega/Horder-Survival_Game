using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    private PlayerMovement wizard;

    private void Start()
    {
        wizard = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        wizard.MuerteWizard += ShowGameOver;
    }
   private void ShowGameOver(object sender, EventArgs e)
    {
        gameOverPanel.SetActive(true); // Mostrar la pantalla de Game Over
    }



    public void GoToMenu()
    {
        
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }
}
