using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuOpciones : MonoBehaviour
{


    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixer audioMixerEfectos;

    public void ChangeSceneCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }
    public void ChangeSceneMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Detener el juego en el editor
#else
        Application.Quit(); // Salir del juego en una build
#endif
    }

    public void CambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("volumen",volumen);
    }

    public void CambiarVolumenEfectos(float VolumenEfectos)
    {
        audioMixerEfectos.SetFloat("VolumenEfectos", VolumenEfectos);
    }
}
