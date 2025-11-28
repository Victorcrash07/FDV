using UnityEngine;

public class CartaHermano : MonoBehaviour, IInteractable
{
    public string GetInteractionMessage()
    {
        return "Leer carta (E)";
    }

    public void Interact()
    {
        // Llamamos al UI Manager para abrir la carta
        CartaUIManager.Instance.AbrirCarta();
        Debug.Log("INTERACT() DE LA CARTA SE HA EJECUTADO");
    }
}
