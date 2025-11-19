using UnityEngine;

public class CuadroInteract : MonoBehaviour, IInteractable
{
    public CuadroManager manager;
    public GameObject fragmentoVisual1; 
    public GameObject fragmentoVisual2; 

    // Asegúrate de que estos Enums existan en tu InventoryItem.cs
    public InventoryItem requiredFragment1 = InventoryItem.FragmentoCuadro1; 
    public InventoryItem requiredFragment2 = InventoryItem.FragmentoCuadro2; 

    // Estado para saber qué fragmentos ya están colocados
    private bool fragment1Placed = false;
    private bool fragment2Placed = false;

    void Start()
    {
        // Asignación de Manager de respaldo
        if (manager == null) manager = FindAnyObjectByType<CuadroManager>();
    }

    public string GetInteractionMessage()
    {
        int placedCount = (fragment1Placed ? 1 : 0) + (fragment2Placed ? 1 : 0);
        int missingCount = 2 - placedCount;

        if (missingCount == 0) return "Cuadro completo.";
        
        // Comprobar si el jugador tiene ALGUNO de los fragmentos que faltan
        bool hasMissingFragment = 
            (!fragment1Placed && InventoryManager.Instance.ContainsItem(requiredFragment1)) ||
            (!fragment2Placed && InventoryManager.Instance.ContainsItem(requiredFragment2));

        if (hasMissingFragment)
        {
            return "Colocar fragmento (E)";
        }
        return $"Faltan {missingCount} fragmentos.";
    }

    public void Interact()
    {
        if (fragment1Placed && fragment2Placed) return; // Si ya está completo, no interactúa.

        // LÓGICA DE COLOCACIÓN: Intentamos colocar los fragmentos en orden de prioridad
        
        // 1. Intentar colocar el FRAGMENTO 1
        if (!fragment1Placed && InventoryManager.Instance.ContainsItem(requiredFragment1))
        {
            InventoryManager.Instance.RemoveItem(requiredFragment1);
            
            // Notificar al Manager
            manager.PlaceFragment(); 
            fragment1Placed = true;
            
            // Lógica Visual: Mostrar la pieza 1 colocada
            if (fragmentoVisual1 != null) fragmentoVisual1.SetActive(true); 

            Debug.Log($"Fragmento {requiredFragment1.ToString()} colocado con éxito.");
            return; // Salir después de colocar uno.
        }
        
        // 2. Intentar colocar el FRAGMENTO 2
        else if (!fragment2Placed && InventoryManager.Instance.ContainsItem(requiredFragment2))
        {
            InventoryManager.Instance.RemoveItem(requiredFragment2);
            
            // Notificar al Manager
            manager.PlaceFragment(); 
            fragment2Placed = true; // Corregido el nombre de la variable
            
            // Lógica Visual: Mostrar la pieza 2 colocada
            if (fragmentoVisual2 != null) fragmentoVisual2.SetActive(true); 

            Debug.Log($"Fragmento {requiredFragment2.ToString()} colocado con éxito.");
            return; // Salir después de colocar uno.
        }
        
        // Si interactúa pero no tiene los ítems:
        else
        {
             Debug.Log("No tienes el fragmento necesario o ya has colocado ambos.");
        }
    }
}