using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    public Image logo;
    public float fadeDuration = 1.0f;
    public float displayDuration = 2.0f;
    public string nextSceneName = "MenuPrincipal";

    void Start()
    {
        StartCoroutine(PlaySplashScreen());
    }

    IEnumerator PlaySplashScreen()
    {
        logo.canvasRenderer.SetAlpha(1.0f); // Comienza con el logo completamente opaco
        yield return new WaitForSeconds(displayDuration);
        logo.CrossFadeAlpha(0.0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);

        // Cargar la escena del menú principal
        SceneManager.LoadScene(nextSceneName);
    }
}
