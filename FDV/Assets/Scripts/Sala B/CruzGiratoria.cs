using UnityEngine;

public class CruzGiratoria : MonoBehaviour, IInteractable
{

    public Transform crossTransform; // El transform de la cruz para rotarla
    
    private bool fragmentClaimed = false; 
    public GameObject fragmentoVisual;
    public string GetInteractionMessage()
    {
        return fragmentClaimed ? "Cruz girada" : "Girar Cruz (E)";
    }
    
    public void Interact()
    {
        if (!fragmentClaimed)
        {
            // Rotar la cruz 90 grados (simulando un mecanismo)
            if (crossTransform != null)
            {
                // Puedes usar crossTransform.Rotate(Vector3.forward, 90f);
                // O usar una corrutina para una rotación suave, pero esto es instantáneo:
                crossTransform.rotation *= Quaternion.Euler(90, 0, 0); 
            }
            
           if (fragmentoVisual != null)
            {
                fragmentoVisual.SetActive(true); // El fragmento aparece en la pared
            }
            
            fragmentClaimed = true;
            Debug.Log("Al girar la cruz, el fragmento 2 ha aparecido.");
        }
    }
}