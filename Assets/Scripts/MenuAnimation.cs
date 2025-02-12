using UnityEngine;
using System.Collections;

public class MenuAnimationController : MonoBehaviour
{
    public GameObject menuButtons;
    public GameObject optionsPanel;

    private Vector3 menuOffScreenPosition;
    private Vector3 menuOnScreenPosition;
    private Vector3 optionsOffScreenPosition;
    private Vector3 optionsOnScreenPosition;

    void Start()
    {
        // Establece las posiciones iniciales
        menuOnScreenPosition = menuButtons.transform.localPosition;
        optionsOffScreenPosition = optionsPanel.transform.localPosition;
        menuOffScreenPosition = new Vector3(-Screen.width, menuOnScreenPosition.y, menuOnScreenPosition.z);

        // Calcula la posición centrada en la pantalla para el panel de opciones
        optionsOnScreenPosition = new Vector3(0, 0, optionsPanel.transform.localPosition.z);

        // Asegúrate de que el panel de opciones esté fuera de la pantalla al inicio
        optionsPanel.transform.localPosition = optionsOffScreenPosition;
    }

    public void ShowOptions()
    {
        // Mueve el menú principal fuera de la pantalla y el panel de opciones al centro
        StartCoroutine(MovePanel(menuButtons, menuOffScreenPosition, 0.5f));
        StartCoroutine(MovePanel(optionsPanel, optionsOnScreenPosition, 0.5f));
    }

    public void ShowMenu()
    {
        // Mueve el panel de opciones fuera de la pantalla y el menú principal al centro
        StartCoroutine(MovePanel(optionsPanel, optionsOffScreenPosition, 0.5f));
        StartCoroutine(MovePanel(menuButtons, menuOnScreenPosition, 0.5f));
    }

    private IEnumerator MovePanel(GameObject panel, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = panel.transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            panel.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.transform.localPosition = targetPosition;
    }

    public void CargarEscenaOpciones()
    {
        ShowOptions();
    }
}
