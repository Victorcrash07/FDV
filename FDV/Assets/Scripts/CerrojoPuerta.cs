
using UnityEngine;

public class CerrojoPuerta : MonoBehaviour, IInteractable
{
    // La llave que se necesita para abrir
    public InventoryItem requiredKey = InventoryItem.Key;
   
    
    // Si queremos consumir la llave o solo chequear
    public bool consumeKey = true;
    public Transform exitPoint;
    public GameObject playerObject;
    public bool isLocked = true;
    public CerrojoPuerta exitDoorLock; 

    public string GetInteractionMessage()
    {
        return isLocked ? $"Necesitas la {requiredKey.ToString()} para abrir la puerta" : "Abrir Puerta";
    }

    public void Interact()
    {
        if (isLocked)
        {
            CheckAndUnlock();
        }
        else
        {
            OpenDoor();
        }
    }

    private void CheckAndUnlock()
    {
        // Pregunta al Singleton: ¿Tiene el jugador la llave requerida?
        if (InventoryManager.Instance.ContainsItem(requiredKey))
        {
            isLocked = false;
            if (consumeKey)
            {
                InventoryManager.Instance.RemoveItem(requiredKey); // Quita la llave si se usa
            }
            
            // Lógica de juego: Iniciar animación de apertura, reproducir sonido, etc.
            Debug.Log("¡Puerta desbloqueada!");
        }
        else
        {
            Debug.Log("Necesitas la llave adecuada.");
            // Lógica de juego: Reproducir sonido de 'cerrado' o mostrar mensaje de error.
        }
    }

    private void OpenDoor()
    {
        if (exitDoorLock != null)
        {
            exitDoorLock.isLocked = false; 
        }
        PerformTeleport();
    }

    private void PerformTeleport()
    {
        if (playerObject == null || exitPoint == null)
        {
            Debug.LogError("ERROR: ¡Falta el Player Object o el Exit Point! Revisa el Inspector.");
            return;
        }

        // 1. Desactivar temporalmente los scripts de movimiento/cámara
        PlayerMovement playerMovementScript = playerObject.GetComponent<PlayerMovement>();
        CharacterController controller = playerObject.GetComponent<CharacterController>();
        MonoBehaviour mouseLookScript = playerObject.GetComponentInChildren<CameraLook>();
        
        // Desactivamos temporalmente el movimiento y el CharacterController
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;
        if (controller != null) controller.enabled = false;

        // 2. TELETRANSPORTE (Mueve la posición y rotación del jugador)
        playerObject.transform.position = exitPoint.position;
        playerObject.transform.rotation = exitPoint.rotation;
        
        // 3. Reactivamos el movimiento
        if (controller != null) controller.enabled = true;
        if (mouseLookScript != null) mouseLookScript.enabled = true;
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        
        Debug.Log($"Jugador teletransportado a la nueva sala.");
    }

}