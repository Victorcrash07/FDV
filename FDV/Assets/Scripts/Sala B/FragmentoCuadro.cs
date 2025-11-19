using UnityEngine;

public class FragmentoCuadro : MonoBehaviour, IInteractable
{
    // Define qué fragmento es este
    public InventoryItem fragmentID; 

    public string GetInteractionMessage()
    {
        return "Coger fragmento de cuadro (E)";
    }

    public void Interact()
    {
        if (InventoryManager.Instance != null)
        {
            // Añadir el fragmento al inventario (el registro global)
            InventoryManager.Instance.AddItem(fragmentID);
            
            // Hacer que el objeto desaparezca de la escena
            gameObject.SetActive(false); 
        }
    }
}