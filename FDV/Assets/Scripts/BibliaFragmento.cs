using UnityEngine;

public class BibliaFragmento : MonoBehaviour, IInteractable
{
    [TextArea(3, 10)]
    public string loreText = "Aquí va el texto inquietante del fragmento de la biblia.";
    
    private bool isRead = false;

    public string GetInteractionMessage()
    {
        return isRead ? "Fragmento de la Biblia (Leído)" : "Leer Fragmento de la Biblia (E)";
    }

    public void Interact()
    {
        if (!isRead)
        {
            // Lógica para mostrar el texto en la UI (asume que hay un sistema de texto global)
            // Example: UIManager.Instance.ShowLoreText(loreText);
            
            Debug.Log($"Leyendo Lore: {loreText}");
            isRead = true;
            
            // Si quieres que el objeto desaparezca después de leerlo:
            // gameObject.SetActive(false); 
        }
    }
}