using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


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

    [Header("Re-Spawn")]

    [SerializeField] private Transform spawnPoint;


    [Header("Sistema de puntos")]

    [SerializeField] private int puntuacion;

    [Header("Sistema de Vida")]

    [SerializeField] private int salud = 10;
    [SerializeField, Range(1, 15)] private float tiempoInvulnerable = 1f;

    private Color colorOriginal;
    private Boolean vulnerable = true;

    public int Vidas => salud; //Esto es lo mismo que implementar un GET, nos sirve para ver el valor de un atributo privado desde afuera de la clase en cualquier archivo de codigo

    [Header("Tiempo")]

    private float tiempoInicio;
    private float tiempoReal;

    [SerializeField] private int tiempoEmpleado;

    [SerializeField, Min(1)] private int tiempoLimiteNvl = 100;



    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();

        renderizador = GetComponent<SpriteRenderer>();

        animador = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (renderizador != null)
        {
            colorOriginal = renderizador.color;
        } else { Debug.LogError("Al jugador le hace falta su SpriteRenderer, por ende no poseemos forma de cambar su color");  }


        tiempoInicio = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        entradaMovimiento = XMovement.action.ReadValue<Vector2>();

        SaltoJugador();

        AnimarJugador();

        comprobarFinTiempoJuego();
    }

    private void comprobarFinTiempoJuego()
    {
        tiempoReal = Time.time - tiempoInicio;
        tiempoEmpleado= Mathf.FloorToInt(tiempoReal);

        if (tiempoEmpleado >= tiempoLimiteNvl)
        {
            FinJuego();
            return;
        }
    }

    private void SaltoJugador()
    {
        tocandoSuelo = Physics2D.OverlapCircle(bolaColision.position, groundDistance, capaSuelo);

        if (jump.action.WasPressedThisFrame() && tocandoSuelo)
        {
            saltoPendiente = true;
        }
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
        MoverJugador();

        OrientarSprite();

    }

    private void MoverJugador()
    {
        fisica.linearVelocity = new Vector2(entradaMovimiento.x * velocity, fisica.linearVelocity.y);

        if (saltoPendiente)
        {
            fisica.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        saltoPendiente = false;
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

    public void ReaparecerJugador() //Con este metodo es el que luego utilizaremos en el script de los enemigos para detectar la colision de los coliders, y de ese modo, hacer que pueda recibir "daño"
    {
        fisica.position = (Vector2)spawnPoint.position;
        fisica.linearVelocity = Vector2.zero;

        if (spawnPoint == null)
        {
            Debug.LogError("Aun no se agrega un objeto 'SpawnPoint', solucionalo y vuelve a intentar", this);
            enabled = false;
            return;
        }
    }

    public void IncrementoPutuacion(int gananciaPuntuacion)
    {
        if (gananciaPuntuacion <= 0) return;

        puntuacion = puntuacion + gananciaPuntuacion;
    }

    public void quitarVida()
    {
        if (vulnerable == false) { return; } //Con esto nos encargamos de que SOLO puedan quitarnos una vida hasta que se nos acabe la invulnerabilidad, cuando ese no sea el caso nos podran quitar vida con normalidad

        vulnerable = false;
        salud--;

        if (renderizador != null)
        {
            renderizador.color = Color.mediumVioletRed;
        }
        
        if (salud <= 0)
        {
            FinJuego();
            return;
        }

        Invoke(nameof(HacerVulnerable), tiempoInvulnerable);
    }

    private void HacerVulnerable()
    {
        vulnerable = true;

        if (renderizador != null)
        {
            renderizador.color = colorOriginal;
            
        }
    }

    

    private void FinJuego()
    {
        Debug.Log("GAME OVER");
        gameObject.SetActive(false);
    }
}
