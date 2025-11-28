using UnityEngine;

public class PartituraItem : MonoBehaviour, IInteractable
{
    // Usamos el ID del inventario para este item
    public InventoryItem partituraID = InventoryItem.Partitura; 

    public string GetInteractionMessage()
    {
        return "Coger (E)";
    }

    public void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            // Añadir la partitura al inventario
            InventoryManager.Instance.AddItem(partituraID);
            
            // Hacer que el objeto desaparezca de la escena
            gameObject.SetActive(false); 
        }
    }
}
