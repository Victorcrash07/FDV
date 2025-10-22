using UnityEngine;

public class Llave : MonoBehaviour, IInteractable
{
   // Definimos que item es esta llave
    public InventoryItem thisKey = InventoryItem.Key; 
    public string GetInteractionMessage()
    {
        return "Coger llave";
    }

    public void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(thisKey); // Registra la llave
            gameObject.SetActive(false); // La hacemos desaparecer
        }
    }
}
