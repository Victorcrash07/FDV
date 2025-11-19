using UnityEngine;

public class Cajon : MonoBehaviour, IInteractable 
{
      public Animator drawerAnimator; // Referencia al componente Animator del cajón
    public string openAnimationName = "Abrir";
    
    public bool fragmentClaimed = false; 
    private bool isOpen = false;
    public GameObject fragmentoVisual;


    public string GetInteractionMessage()
    {
        return isOpen ? "Cerrar Cajón (E)" : "Abrir Cajón (E)";
    }
    
    public void Interact()
    {
        if (drawerAnimator != null)
        {
            if (!isOpen)
            {
                // Abrir el cajón
                drawerAnimator.SetTrigger(openAnimationName);
                isOpen = true;
                
                if (fragmentoVisual != null && !fragmentClaimed)
                {

                    fragmentoVisual.SetActive(true); 
                    fragmentClaimed = true; 
                    Debug.Log("Fragmento 1 revelado en el cajón.");
                }
            }
            else
            {
                // Cerrar el cajón
                drawerAnimator.SetTrigger("Cerrar"); // Asumiendo que tienes un trigger 'Close'
                isOpen = false;
            }
        }
        else
        {
            Debug.LogError("El Drawer.cs necesita una referencia al Animator.");
        }
    }
}