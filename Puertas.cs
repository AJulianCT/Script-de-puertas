using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Tiempo que la puerta se mantiene abierta
    public float timeToClose = 3.0f;

    // Ángulo de apertura de la puerta
    public float openAngle = 90f;
    public float openSpeed = 2f;
    
    // Estado inicial y posición de la puerta
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // Guardamos la rotación cerrada
        closedRotation = transform.rotation;
        
        // Calculamos la rotación de apertura de la puerta
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    // Este método se llama cuando el personaje entra en el Collider de la puerta
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            // Si el objeto que colisiona tiene el tag "Player" y la puerta no está abierta, se abre
            StartCoroutine(OpenDoor());
        }
    }

    // Corrutina para abrir la puerta y luego cerrarla después de un tiempo
    private IEnumerator OpenDoor()
    {
        isOpen = true;
        float timeElapsed = 0f;

        // Animación de abrir la puerta
        while (timeElapsed < openSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, timeElapsed / openSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la puerta está completamente abierta
        transform.rotation = openRotation;

        // Esperar antes de cerrar la puerta
        yield return new WaitForSeconds(timeToClose);

        // Cerrar la puerta
        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        float timeElapsed = 0f;

        // Animación de cerrar la puerta
        while (timeElapsed < openSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, timeElapsed / openSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la puerta está completamente cerrada
        transform.rotation = closedRotation;
        isOpen = false;
    }
}