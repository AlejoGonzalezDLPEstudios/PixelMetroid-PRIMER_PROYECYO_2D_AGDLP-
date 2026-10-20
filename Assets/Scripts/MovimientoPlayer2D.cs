using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent (typeof(Rigidbody2D))] //Este elemento se asegurara de que el elemento en Unity posea un RigidBody, ya que obliga al programa a encontrar uno
public class MovimientoPlayer2D : MonoBehaviour
{
    [Header("Movimiento")] //Estas etiquetas "Header" funcionan como las etiquetas de HTML, aplicarán ese "titulo" a los atributos declarados debajo hasta el siguiente "Header"
    public float velocity;

    private Rigidbody2D fisica;

    [SerializeField ]private InputActionReference XMovement; //Al estar declarando las teclas en un mapa, no podemos ponerlo directamente como "InputAction", sino que tenemos que declararlo como InputActionReference

    //El apartado [SerializeField] es para poder poner elementos en privado, pero que podamos verlo en los elementos del editor, la diferencia con ponerlo en public es que nos permite que
    //solo pueda ser modificado desde este archivo, pero a su vez que nos permita modificarlo desde el editor de Unity

    [Header("Salto")]

    [SerializeField] private InputActionReference jump; //Con este InputReference, nos encargaremos de crear el código para que nuestro personaje pueda saltar
    public float jumpForce = 5f; //Este valor determinará la fuerza con la que nuestro personaje saltará, de ahí el nombre para nada referencia a otra cosa

    private Vector2 entradaX; //Ahora se declara como Vector2 y no como float ya que en el mapa de acciones dentro de Unity estamos asignando las teclas directamente y eso es lo que queremos consultar, no el valor bruto, sino la posicion del jugador en el momento

    private Boolean saltoPendiente;

    

    private Boolean tocandoSuelo;

    [SerializeField] private float longitudRayCast = 1f; //En este parametro determinaremos el largo que tendrá el "rayo de colision" para detectar el suelo, cosa que nos permitira determinar si el player puede saltar o no
    [SerializeField] private LayerMask capaSuelo;

    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D> ();
    } 
    
    //A la hora de inicializar variables, lo mejor es hacerlo en uno de estos metodos AWAKE, ya que así no depende de que el código este activo y nos permite acceder a sus valores en cualquier momento

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        entradaX = XMovement.action.ReadValue<Vector2>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRayCast, capaSuelo);

        tocandoSuelo = hit.collider != null;

        Color colorRayoColision = tocandoSuelo ? Color.green : Color.red; //esta sentencia es básicamente como un IF, acambiando el color del rayo segun este tocando el suelo o no

        Debug.DrawRay(transform.position, Vector2.down * longitudRayCast, colorRayoColision); //y ahora con este parametro nos estamos encargando de dibujar el rayo de colision segun los colores que asignamos antes, de esta forma podremos aumentar su longitud sin tener que hacerlo tan a ojo

        if (jump.action.WasPressedThisFrame() && tocandoSuelo)
        {

            saltoPendiente = true;

        }
    }

    private void OnEnable() //Se ejecuta mientras el juego esta "activado", activando respectivamente las opciones necesarias para que no se corrompa todo
    {
        if (XMovement == null || XMovement.action == null) {

            Debug.LogError("Aún no se ha asignado una tecla, habilite una en el mapa de acciones correspondiente");
            enabled = false;
            return;

        }

        XMovement.action.Enable();

        if (jump == null || jump.action == null)
        {

            Debug.LogError("Aún no se ha asignado una tecla, habilite una en el mapa de acciones correspondiente");
            enabled = false;
            return;

        }

        jump.action.Enable();
    }

    private void OnDisable() //Se ejecuta mientras el juego esta "desactivado", desahibilitando opciones necesarias para que no se corrompa todo
    {
        if (XMovement != null && XMovement.action != null)
        {
            XMovement.action.Disable();
        }

        {
            if (jump != null && jump.action != null)
            {
                jump.action.Disable();
            }
        }
    }

   

    

    private void FixedUpdate()
    {
        fisica.linearVelocity = new Vector2(entradaX.x * velocity, fisica.linearVelocity.y);

        if (saltoPendiente)
        {
            // fisica.linearVelocity = new Vector2(fisica.linearVelocity.x, 8);

            fisica.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); //Unity ya nos trae opciones para aplicar la fuerza de forma superior o inferior, de ahí el "Vector2.up"
        }

        saltoPendiente = false;
    }
}
