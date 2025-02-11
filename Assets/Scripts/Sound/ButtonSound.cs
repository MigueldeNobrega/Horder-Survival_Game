using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        // Obt�n el componente Button del GameObject
        Button button = GetComponent<Button>();

        if (button != null)
        {
            // A�ade un listener al evento onClick del bot�n
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        // Reproduce el sonido
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}