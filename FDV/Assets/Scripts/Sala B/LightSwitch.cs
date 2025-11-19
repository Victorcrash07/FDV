using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light targetLight; // La luz que controla el interruptor
    public GameObject targetObject; // Opcional: El cuadro incompleto, para activarlo al darle luz

    private bool isOn = false;

    public string GetInteractionMessage()
    {
        return isOn ? "Apagar Luz (E)" : "Encender Luz (E)";
    }

    public void Interact()
    {
        isOn = !isOn;
        
        if (targetLight != null)
        {
            targetLight.enabled = isOn;
        }
        
        if (targetObject != null)
        {
             // Muestra el cuadro si la luz está encendida
            targetObject.SetActive(isOn);
        }
        
        // Opcional: Reproducir sonido de interruptor.
    }
}
