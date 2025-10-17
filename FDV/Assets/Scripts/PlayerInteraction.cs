using UnityEngine;
using UnityEngine.UI; 

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // Distancia máxima para interactuar
    public KeyCode interactionKey = KeyCode.E; // Tecla para interactuar

    public Image crosshair; // Referencia a al puntero
    public Text interactionText; // Texto para mostrar el mensaje 
    public LayerMask interactionMask;
    
    private Armario currentLocker; //Sirve para interactuar con el armario 
    void Update()
    {
      // Tiene prioridad el escondite a la hora de interactuar
    // Solo si currentLocker no es null, significa que estamos escondidos
    if (currentLocker != null)
    {
        // Si presionamos la tecla de interacción, salimos
        if (Input.GetKeyDown(interactionKey))
        {
            currentLocker.Interact(); // Esto llama a ExitLocker()
            currentLocker = null;     // Resetear la referencia al armario (para diferentes armarios)
            ResetInteractionUI();     // Resetear el puntero/texto
        }
        return; // Salimos de Update para ignorar el Raycast mientras estamos dentro 
    }
       
        // Creamos un rayo que sale desde el centro de la cámara hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; // Variable para almacenar la información del objeto golpeado

        // Lanzamos el rayo
       if (Physics.Raycast(ray, out hit, interactionDistance, interactionMask))
        {
            // Verificamos si el objeto golpeado tiene un componente que implemente IInteractable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null) // El objeto es interactuable
            {
                crosshair.color = Color.green; // Cambia el color del puntero a verde
                interactionText.text = interactable.GetInteractionMessage(); // Muestra el mensaje
                interactionText.gameObject.SetActive(true);

                // Si el objeto es un armario y presionamos la tecla de entrada
            if (interactable is Armario locker && Input.GetKeyDown(interactionKey))
            {
                currentLocker = locker;    //Guardamos la referencia antes de interactuar
                locker.Interact();         // Entramos al armario
                interactionText.text = interactable.GetInteractionMessage(); //Actualizamos el mensaje(para que salga el de salida)
            }

                // El jugador presiona la tecla de interacción entoces llamamos a su método Interact()
                else if (Input.GetKeyDown(interactionKey))
                {  
                    interactable.Interact();
                }
            }
            else
            {
                // El objeto no es interactuable
                ResetInteractionUI();
            }
        }
        else
        {
            // No estamos mirando a nada
            ResetInteractionUI();
        }
    }


    // Método para resetear la UI a su estado normal
    void ResetInteractionUI()
    {
        crosshair.color = Color.white;
        interactionText.gameObject.SetActive(false);
    }
}