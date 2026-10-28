using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]

public class PlataformaMovil : MonoBehaviour
{

    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;

    [SerializeField, Min(0f)] private float velocity;

    [SerializeField] private float tolerancia = 0.1f;


    private Rigidbody2D fisica;

    private Boolean haciaB = true;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
    }

    public Vector2 Velocidad {  get; private set; } //Este parametro es para implementar los metodos SET y GET, como en Java, solo que con una sola linea de codigo más simplificada (Gracias a Dios)



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (puntoA == null || puntoB == null)
        {
            Debug.LogError("Tanto el punto A como el B necesitan un valor, sino la plataforma no puede tener un punto de salida o de final", this);
            return;
        }



        fisica.position = (Vector2)puntoA.position; //Con esto nos aseguramos al 100% de que la plataforma empiece en el mismo punto que el A, esto se hizo tambien en diseño pero asi nos aseguramos 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate() //llamaremos a las físicas, asi que es necesario implementar el FixedUpdate
    {
        Vector2 destino = haciaB ? (Vector2)puntoB.position : (Vector2)puntoA.position; //Si haciaB es TRUE, se dirigirá a la posicion B, sino, lo hara hacia la A

        if (Vector2.Distance(fisica.position, destino) <= tolerancia)
        {
            haciaB = !haciaB;
            destino = haciaB ? (Vector2)puntoB.position : (Vector2)puntoA.position;
        }

        Vector2 siguiente = Vector2.MoveTowards(fisica.position, destino, velocity * Time.fixedDeltaTime); //Aqui le estamos diciendo que queremos desplazarnos de forma NO instantanea, por eso se añade la velocidad (lo del Time.FixedDeltaTime es para que sea independiente de los FPS) 


        fisica.MovePosition(siguiente);
    }

    private void OnDrawGizmos()
    {
        if (puntoA == null || puntoB == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(puntoA.position, puntoB.position);
    }
}
