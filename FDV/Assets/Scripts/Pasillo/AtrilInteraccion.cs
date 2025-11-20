using UnityEngine;

public class AtrilInteraccion : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    public InventoryItem requiredItem = InventoryItem.Partitura;
    
    [Tooltip("El objeto que se activa para mostrar la partitura en el atril.")]
    public GameObject partituraVisual; 
    
    [Tooltip("El controlador del piano para notificarle que la partitura está puesta.")]
    public PianoController pianoController; 

    private bool isPlaced = false;

    void Start()
    {
        // Asegurarse de que el visual de la partitura esté oculto al inicio
        if (partituraVisual != null) partituraVisual.SetActive(false);

        // Opcional: Persistencia. Si ya la colocó, se mantiene el estado.
        // Si el ítem ya no está en el inventario, asumimos que fue colocado previamente.
     /*   if (InventoryManager.Instance.ContainsItem(requiredItem) == false && pianoController != null)
        {
            isPlaced = true;
            if (partituraVisual != null) partituraVisual.SetActive(true);
            // Si el piano tiene un bool para 'partituraPlaced', se lo asignamos aquí.
        }*/
    }

    // 1. LÓGICA CONDICIONAL DE INTERACCIÓN (El foco de la petición)
    public string GetInteractionMessage()
    {
        if (isPlaced)
        {
            return "Partitura colocada.";
        }
        
        // **Solo muestra el mensaje de colocación si el jugador tiene el item.**
        if (InventoryManager.Instance.ContainsItem(requiredItem))
        {
            return "Colocar Partitura (E)";
        }
        
        // Si no la tiene y no está colocada, no da pistas.
        return ""; 
    }

    public void Interact()
    {
        if (isPlaced) return; // Si ya está, no hacemos nada.

        // 2. VERIFICAR Y COLOCAR
        if (InventoryManager.Instance.ContainsItem(requiredItem))
        {
            InventoryManager.Instance.RemoveItem(requiredItem);
            isPlaced = true;
            
            // 3. Lógica Visual
            if (partituraVisual != null)
            {
                partituraVisual.SetActive(true);
            }
            
            // 4. Notificar al Controlador del Piano (Si es necesario)
            if (pianoController != null)
            {
                // Si PianoController necesita un bool para habilitar la secuencia:
                // pianoController.partituraColocada = true; 
            }

            Debug.Log("Partitura colocada. La melodía está visible.");
        }
        // Si GetInteractionMessage() devuelve "", esta parte del código no debería ser accesible por el jugador
        // ya que no habría mensaje de interacción.
    }
}