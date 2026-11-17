using TMPro;
using UnityEngine;

public class ControlHUD : MonoBehaviour
{
    public TMP_Text vidasTxt;
    public TMP_Text powerUpsTxt;

    public TMP_Text tiempoTxt;


    private void Awake()
    {
        if ( vidasTxt == null || powerUpsTxt == null || tiempoTxt == null )
        {
            Debug.LogError("Uno o mas textos TMP del HUB no estan asignados con el componente ControlHub, por favor asegurese e intentelo nuevamente");
        }
    }


    public void setVida (int cantidad)
    {
        if (vidasTxt != null)
        {
            vidasTxt.text = "Vidas: " + cantidad;
        }
    }

    public void setPowerUps(int cantidad)
    {
        if (powerUpsTxt != null)
        {
            vidasTxt.text = "Bolutas de energía restantes: " + cantidad;
        }
    }

    public void setTiempo(int segundosRestantes)
    {
        if (tiempoTxt == null) return;

        int minutos = segundosRestantes / 60;
        int segundos = segundosRestantes % 60;

        tiempoTxt.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

}
