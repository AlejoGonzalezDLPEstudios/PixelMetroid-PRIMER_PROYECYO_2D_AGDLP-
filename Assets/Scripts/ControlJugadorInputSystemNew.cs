using UnityEngine;
using UnityEngine.InputSystem; // Por si acaso el "InputSystem" da error al intentarlo poner en el codigo 

public class ControlJugadorInputSystemNew : MonoBehaviour
{
    public InputAction moveAction;
    public float velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable(); //Esto es lo que activa la variable del movimiento que declaramos antes en el InputAction, sin esto por muy bien que este el código el movimiento NO sucederá

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;

        //Los valores anteriores los probamos por simple informacion, pero es una forma de asignar los FPS directamente desde código
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>(); //Aqui le asigna a "Move" los valores que esta consiguiendo al presionar la tecla de movimiento que asignemos

        Vector2 position = (Vector2)transform.position + move*velocity*Time.deltaTime; //"transform.position" es de tipo Vector3 (X, Y, Z) por eso lo casteamos a Vector2, para que no tenga en cuenta Z
        
        //Anteriormente, la parte final de las multiplicaciones fue añadida despues, esto es para que podamos hacerlo independiente de los FPS del equipo y asi evitamos que vaya super rapido 
        //o super lento, aunque esto solo habría que aplicarlo si es que si o si queremos que el movimiento funcione estando en el "Update" base en vez del "Fixed Update"

        transform.position = position; //Antes sumamos la posicion actual con lo añadido, aqui pasamos ese valor a la variable que creamos antes
    }
}
