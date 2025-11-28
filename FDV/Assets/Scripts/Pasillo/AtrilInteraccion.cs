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
    public Camera playerCamera; 
    public Camera TheCamera; 
    private CharacterController playerController;
    private MonoBehaviour mouseLookScript;
    public GameObject playerObject;
    private bool isViewingLore = false;
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
        if (TheCamera != null) TheCamera.enabled = false;
        if (playerCamera == null) playerCamera = Camera.main;
        playerController = playerObject.GetComponent<CharacterController>();
        mouseLookScript = playerObject.GetComponentInChildren<CameraLook>();
       
    }

    // 1. LÓGICA CONDICIONAL DE INTERACCIÓN (El foco de la petición)
    public string GetInteractionMessage()
    {
        if (isViewingLore)
        {
            return "Salir de la vista (E)";
        }
        if (isPlaced)
        {   
            if(requiredItem == InventoryItem.Receta1 || requiredItem == InventoryItem.Receta2 || requiredItem == InventoryItem.Receta3)
            {
                return "Receta colocada.";
            }
             if(requiredItem == InventoryItem.Biblia1 || requiredItem == InventoryItem.Biblia2 || requiredItem == InventoryItem.Biblia3)
            {
                return "Página biblia colocada.";
            }
            else{return "Partitura colocada."; }
            
        }
        
        // **Solo muestra el mensaje de colocación si el jugador tiene el item.**
        if (InventoryManager.Instance.ContainsItem(requiredItem))
        {
            if(requiredItem == InventoryItem.Receta1 || requiredItem == InventoryItem.Receta2 || requiredItem == InventoryItem.Receta3)
            {
                return "Colocar Receta(E)";
            }else if(requiredItem == InventoryItem.Biblia1 || requiredItem == InventoryItem.Biblia2 || requiredItem == InventoryItem.Biblia3)
            {
                return "Colocar Página biblia (E)";
            }
            else{return "Colocar Partitura (E)";}
        }
        
        // Si no la tiene y no está colocada, no da pistas.
        return ""; 
    }

    public void Interact()
    {
        if (isPlaced) {if (requiredItem != InventoryItem.Partitura) 
            {
                if (isViewingLore)
                {
                    ExitLoreMode();
                }
                else
                {
                    EnterLoreMode();
                }
            }
            return;}

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
            // 3. [LÓGICA CLAVE] Si es Receta o Biblia, entrar en modo de vista de cerca.
            if (requiredItem != InventoryItem.Partitura)
            {
                EnterLoreMode();
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

    private void EnterLoreMode()
{
    isViewingLore = true;
    
    // 1. Deshabilitar scripts de control
    if (playerObject != null && playerObject.GetComponent<PlayerMovement>() != null)
        playerObject.GetComponent<PlayerMovement>().enabled = false;
    
    if (mouseLookScript != null) 
        mouseLookScript.enabled = false; 
    
    if (playerController != null)
        playerController.enabled = false; 

    // 2. Cámaras
    if (playerCamera != null) playerCamera.enabled = false;
    if (TheCamera != null) TheCamera.enabled = true; // Activa la cámara de primer plano
    
    // 3. Cursor (Mostrar y liberar para interacción de UI si hubiera, o solo para el look)
    /*Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;*/
}

private void ExitLoreMode()
{
    isViewingLore = false;
    
    // 1. Restauración de Cámaras
    if (TheCamera != null) TheCamera.enabled = false;
    if (playerCamera != null) playerCamera.enabled = true;

    // 2. Restaurar el control del jugador
    if (playerObject != null && playerObject.GetComponent<PlayerMovement>() != null) 
        playerObject.GetComponent<PlayerMovement>().enabled = true;
    
    if (mouseLookScript != null) 
        mouseLookScript.enabled = true;
    
    if (playerController != null) 
        playerController.enabled = true;

    // 3. Restaurar el cursor
   /* Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;*/
    
    Debug.Log("Saliendo del modo de vista de cerca.");
}
}