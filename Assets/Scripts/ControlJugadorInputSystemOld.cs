using UnityEngine;

public class ControlJugadorInputSystemOld : MonoBehaviour
{
    public float velocidad;

    private Rigidbody2D fisicas;

    private float entradaX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       fisicas = GetComponent<Rigidbody2D>(); //Se obtiene el componente RigidBody2D del player para modificar posteriormente su LinearVelocity. 
    }

    private void FixedUpdate()
    {
        entradaX = Input.GetAxis("Horizontal");
        if (fisicas != null)
        {
            fisicas.linearVelocity = new Vector2(entradaX * velocidad, fisicas.linearVelocity.y);
        }
        else
        {
            Debug.LogError("El GameObject no tiene asignado un componente RigidBody2D, el mismo es 100% necesario para el movimiento, corriga y vuelva a intentar");
        }
    }




    // Update is called once per frame
    //void Update()
    //{
    //    Vector2 position = transform.position; //se crea una variable temporal

    //    position.x = position.x + 0.1f; //La "F" viene de Float, Flotante, aqui le damos el valor a la variable para que mueva el sprite en la X (aumenta, asi que será a la derecha)

    //    transform.position = position; //le asignamos el valor de la variable temporal a la variable real
    //}
}
