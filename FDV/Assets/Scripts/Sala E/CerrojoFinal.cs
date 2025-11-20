
using UnityEngine;

public class CerrojoFinal : MonoBehaviour, IInteractable
{
    public bool isLocked = true;
   
    [Header("Transición Final")]
    [Tooltip("El objeto que contiene el Video Player y el script EndGameController.")]
    public GameObject cinematicTriggerObject;
    public Canvas canvasCinematic; // Referencia al canvas de la cinemática final
    public string GetInteractionMessage()
    {
        return isLocked ? $"Necesitas el código para abrir la puerta" : "Abrir Puerta";
    }

    public void Interact()
    {
        if (isLocked)
        {
           
        }
        else
        {
            OpenDoor();
        }
    }

    
    private void OpenDoor()
    {
        // 3. ACTIVAR LA CINEMÁTICA
    if (cinematicTriggerObject != null)
    {
        // El objeto de la cinemática (que debe estar inactivo) se encenderá.
        cinematicTriggerObject.SetActive(true); 
        canvasCinematic.gameObject.SetActive(true); // Activar el canvas de la cinemática
    }
    }

   

}