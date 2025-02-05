using UnityEngine;
using UnityEngine.UI;

public class BarraDeEscudo : MonoBehaviour
{
    [SerializeField] private Slider barra;

    void Start()
    {
        if (barra == null)
        {
            barra = GetComponent<Slider>();
            if (barra == null)
            {
                Debug.LogError("No se encontró un componente Slider. Asigna uno en el Inspector.");
            }
        }
    }

    public void CambiarEscudoMaximo(float escudoMaximo)
    {
        if (barra != null)
        {
            barra.maxValue = escudoMaximo;
        }
    }

    public void CambiarEscudoActual(float cantidadEscudo)
    {
        if (barra != null)
        {
            barra.value = cantidadEscudo;
        }
    }

    public void InicializarBarraDeEscudo(float cantidadEscudo)
    {
        CambiarEscudoMaximo(cantidadEscudo);
        CambiarEscudoActual(cantidadEscudo);
    }

}
