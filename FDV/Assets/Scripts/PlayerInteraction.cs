using UnityEngine;
using UnityEngine.UI; 

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // Distancia máxima para interactuar.
    public KeyCode interactionKey = KeyCode.E; // Tecla para interactuar.

    public Image crosshair; // Referencia a tu puntero/crosshair.
    public Text interactionText; // Texto para mostrar el mensaje (ej: "Coger llave").

    void Update()
    {
        // Creamos un rayo que sale desde el centro de la cámara hacia adelante.
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; // Variable para almacenar la información del objeto golpeado.

        // Lanzamos el rayo.
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Verificamos si el objeto golpeado tiene un componente que implemente IInteractable.
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // El objeto es interactuable.
                crosshair.color = Color.green; // Cambia el color del puntero a verde.
                interactionText.text = interactable.GetInteractionMessage(); // Muestra el mensaje.
                interactionText.gameObject.SetActive(true);

                // Si el jugador presiona la tecla de interacción...
                if (Input.GetKeyDown(interactionKey))
                {
                    // ...llamamos a su método Interact().
                    interactable.Interact();
                }
            }
            else
            {
                // El objeto no es interactuable.
                ResetInteractionUI();
            }
        }
        else
        {
            // No estamos mirando a nada.
            ResetInteractionUI();
        }
    }

    // Método para resetear la UI a su estado normal.
    void ResetInteractionUI()
    {
        crosshair.color = Color.white;
        interactionText.gameObject.SetActive(false);
    }
}