using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoPlayer2dSaltoMetodo2 : MonoBehaviour
{
    [Header("Movimiento")]

    public float velocity;

    [SerializeField] private InputActionReference XMovement; 

    

    [Header("Salto")]

    [SerializeField] private InputActionReference jump; 
    public float jumpForce = 5f; 

    private Vector2 entradaX; 

    private Boolean saltoPendiente;

    [Header("Chequeo del suelo")]

    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private Transform bolaColision; //Objeto vacio que se situa en los pies, tendrá forma de bola (Metodo salto 2)
    [SerializeField] private LayerMask capaSuelo;



    private Rigidbody2D fisica;

    private SpriteRenderer renderizador; //este componente nos ayudará para poder voltear al personaje al moverse de un lado u otro

    private Animator animador;
    private float umbralAnimacion = 0.1f;

    private Vector2 entradaMovimiento;
    private Boolean tocandoSuelo;




    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();

        renderizador = GetComponent<SpriteRenderer>();

        animador = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        entradaMovimiento = XMovement.action.ReadValue<Vector2>();

        tocandoSuelo = Physics2D.OverlapCircle(bolaColision.position, groundDistance, capaSuelo);

        if(jump.action.WasPressedThisFrame() && tocandoSuelo)
        {
            saltoPendiente = true;
        }

        AnimarJugador();
    }

    private void AnimarJugador()
    {
        if (animador == null)
        {
            return;
        }

        if (!tocandoSuelo)
        {
            animador.Play("JugadorSaltando"); //Si el jugador NO esta tocando el suelo, reporducira la animación de salto
        }

        else if (Math.Abs(fisica.linearVelocity.x) > umbralAnimacion )
        {
            animador.Play("JugadorCorriendo"); //Si la velocidad lineal (o mas bien su valor absoluto) es mayor a 0, pues el jugador se esta moviendo y por ende, debemos reproducir la animacion de correr

        }

        else
        {
            animador.Play("JugadorParada");
        }
    }

    private void FixedUpdate() // Aqui solo definimos acciones que tengan que ver directamente CON LAS FÍSICAS
    {
        fisica.linearVelocity = new Vector2(entradaMovimiento.x * velocity, fisica.linearVelocity.y);

        if(saltoPendiente)
        {
            fisica.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        saltoPendiente = false;

        OrientarSprite();

    }

    private void OrientarSprite()
    {
        if (renderizador == null) // Si es valor de renderizado es nulo, nos salimos del programa
        {
            return;
        }

        if (fisica.linearVelocity.x > 0) // Si la velocidad lineal del jugador es mayor a cero, significa que se esta moviendo a la derecha, por ende, no activamos el flipeo del sprite
        {
            renderizador.flipX = false;
        }

        if (fisica.linearVelocity.x < 0) // por otro lado, si la velocidad lineal del jugador es menor a cero, significa que se esta moviendo a la izquierda, por ende, SI tenemos que activar el flipeo del sprite
        {
            renderizador.flipX = true;
        }
    }

    private void OnEnable() //Se ejecuta mientras el juego esta "activado", activando respectivamente las opciones necesarias para que no se corrompa todo
    {
        if (XMovement == null || XMovement.action == null)
        {

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

    private void OnDrawGizmosSelected()
    {
        if (bolaColision != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(bolaColision.position, groundDistance);
        }
    }
}
