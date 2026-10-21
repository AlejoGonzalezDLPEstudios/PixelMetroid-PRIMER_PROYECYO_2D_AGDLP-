using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ControlEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocity = 3f;
    public Vector2 posicionFinal;
    private Vector2 posicionInicial;
    private Boolean movimientoHaciaArriba = true;

    [SerializeField] private float margenTolerancia = 0.0f;

    private Rigidbody2D fisica;


    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionInicial = fisica.position;
        
    }

    private void FixedUpdate()
    {
        Vector2 posicionObjetivo;

        if (velocity <= 0) { return; }

        if (movimientoHaciaArriba)
        {
            posicionObjetivo = posicionFinal;
        } else
        {
            posicionObjetivo = posicionInicial;
        }


        if (Vector2.Distance(fisica.position, posicionObjetivo) <= margenTolerancia)
        {
            movimientoHaciaArriba = !movimientoHaciaArriba; //esto es como cambiar el valor de falso a verdadero con un IF, ya que ponerlo así "!movimientoHaciaArriba" con la exclamacion, invierte el resultado

            posicionObjetivo = movimientoHaciaArriba ? posicionFinal : posicionInicial;

            fisica.MovePosition(Vector2.MoveTowards(fisica.position, posicionObjetivo, velocity));
        }
    }
}
