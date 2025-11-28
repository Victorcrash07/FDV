using UnityEngine;

public class CartaHermano : MonoBehaviour, IInteractable
{
    public string GetInteractionMessage()
    {
        return "Leer carta (E)";
    }

    public void Interact()
    {
        // Si la carta ya está abierta, no hacemos nada
        if (CartaUIManager.Instance != null && CartaUIManager.Instance.CartaAbierta)
            return;

        if (CartaUIManager.Instance != null)
        {
            CartaUIManager.Instance.AbrirCarta();
        }
        else
        {
            Debug.LogError("No hay CartaUIManager en la escena");
        }
    }
}
