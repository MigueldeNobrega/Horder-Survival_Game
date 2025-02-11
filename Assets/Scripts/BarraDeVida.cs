using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private Slider barra; // Asigna manualmente el Slider en el Inspector

    void Start()
    {
        // Verifica que el slider esté asignado para evitar errores
        if (barra == null)
        {
            barra = GetComponent<Slider>();

            if (barra == null)
            {

            }
        }
    }

    public void CambiarVidaMaxima(float vidaMaxima)
    {
        if (barra != null)
        {
            barra.maxValue = vidaMaxima;
        }
    }

    public void CambiarVidaActual(float cantidadVida)
    {
        if (barra != null)
        {
            barra.value = cantidadVida;
        }
    }

    public void InicializarBarraDeVida(float cantidadVida)
    {
        CambiarVidaMaxima(cantidadVida);
        CambiarVidaActual(cantidadVida);
    }
}
