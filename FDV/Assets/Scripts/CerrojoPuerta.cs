
using UnityEngine;
using UnityEngine.SceneManagement;
public class CerrojoPuerta : MonoBehaviour, IInteractable
{
    // La llave que se necesita para abrir
    public InventoryItem requiredKey = InventoryItem.Key;
    public string nextSceneName = "SalaPrincipal";
    
    // Si queremos consumir la llave o solo chequear
    public bool consumeKey = true;

    private bool isLocked = true;

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
        SceneManager.LoadScene(nextSceneName);
         Debug.Log("Atravesar la puerta");
    }
}