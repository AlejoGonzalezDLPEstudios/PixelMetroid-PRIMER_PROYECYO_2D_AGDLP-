using UnityEngine;

public class ControlPowerUp : MonoBehaviour

     
{

    [Header("Puntaje")]

    [SerializeField, Range(1, 10)] private int gananciaPuntuación = 2;
    [SerializeField, Range(2, 5)] private int multiplicadorVelocidad = 2;

    

    

    private int numColeccionables;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numColeccionables++;

        

        if (!collision.CompareTag("Player")) return;

        if (collision.gameObject.TryGetComponent(out MovimientoPlayer2dSaltoMetodo2 jugador))
        {
            jugador.IncrementoPutuacion(gananciaPuntuación);
            Destroy(gameObject);

           // numColeccionables--;
            //hud.setPowerUps(numColeccionables);
            
            
        }
        else 
        {
            Debug.LogError("El objeto 'Player' no tiene su componente 'MovimientoJugador', establezcalo e intente nuevamente");
        
        }
    }
}
