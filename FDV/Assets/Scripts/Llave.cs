using UnityEngine;

public class Llave : MonoBehaviour, IInteractable
{
    public string GetInteractionMessage()
    {
        return "Coger llave";
    }

    public void Interact()
    {
        Debug.Log("¡Llave recogida!");
        // Aquí iría la lógica para añadir la llave al inventario del jugador.
        gameObject.SetActive(false); // La hacemos desaparecer.
    }
}
