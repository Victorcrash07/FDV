using UnityEngine;

public class SymbolSelector : MonoBehaviour, IInteractable
{
    public CuadroManager manager;
    [Tooltip("El índice de este símbolo. Debe coincidir con el índice correcto en el Manager.")]
    public int myIndex; 

    public string GetInteractionMessage()
    {
        // Se podría mostrar el nombre del símbolo aquí si fuera relevante.
        return "Seleccionar Símbolo (E)";
    }

    public void Interact()
    {
        // Notifica al Manager la selección del jugador
        manager.SelectSymbol(myIndex);
        
        // Opcional: Feedback visual o sonoro al ser seleccionado.
    }
}