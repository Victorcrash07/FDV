using UnityEngine;


public class CuadroManager : MonoBehaviour
{
    // REFERENCIAS Y ESTADOS
    private int fragmentosColocados = 0; // Contará los fragmentos que ya se USARON
    private const int TOTAL_FRAGMENTOS = 2;
    private bool symbolsActive = false;

    [Header("Referencias de Objetos")]
    public GameObject incompletePicture; // El cuadro antes de tener fragmentos
    public GameObject completePicture;   // El cuadro con los fragmentos puestos
    public CerrojoPuerta exitDoorLock;   // La cerradura de la puerta de salida
    public AudioSource source; // Componente AudioSource en el Manager o en la Puerta
    public AudioClip correctSound; // Sonido de mecanismo o 'clic' para indicar éxito
    //public AudioClip failSound;    // Sonido de error (opcional)
    

    // Array de los 3 o 4 símbolos que el jugador debe seleccionar (Tendrán SymbolSelector.cs)
    public GameObject[] selectionSymbols; 
    // Índice del símbolo correcto (Ej: 0, 1, o 2)
    public int correctSymbolIndex = 0; 
    private InventoryItem PuzzleComplete = InventoryItem.PuzzleCuadroCompleto; 
    void Start()
    {
        // Asegurarse de que el cuadro completo y los símbolos estén ocultos al inicio.
        if (completePicture != null) completePicture.SetActive(false);
        DeactivateSymbols();
        
        if (exitDoorLock == null)
            Debug.LogError("Error: Asigna el CerrojoPuerta de salida al Manager.");
    }

    // Método llamado por el script CuadroInteract.cs cuando el jugador coloca UN fragmento
    public void PlaceFragment()
    {
        fragmentosColocados++;
        Debug.Log($"Fragmento colocado con éxito. Total: {fragmentosColocados}/{TOTAL_FRAGMENTOS}");

        if (fragmentosColocados == TOTAL_FRAGMENTOS)
        {
            // Puzle de colocación completado. Pasamos a la fase de selección.
            RevealPicture();
        }
    }

    private void RevealPicture()
    {
         InventoryManager.Instance.AddItem(PuzzleComplete);
        if (incompletePicture != null) incompletePicture.SetActive(false);
        if (completePicture != null) completePicture.SetActive(true);

        // Activa los objetos de selección (el puzle final)
        ActivateSymbols();
        
        Debug.Log("Cuadro completado. ¡Elige el símbolo correcto!");
    }
    
    // Método llamado por los SymbolSelector.cs
    public void SelectSymbol(int index)
    {
        if (!symbolsActive) return;

        if (index == correctSymbolIndex)
        {
          if (exitDoorLock != null)
            {
                exitDoorLock.isLocked = false; 
                // Opcional: Esto actualiza el mensaje de la puerta a "Abrir Puerta"
                 exitDoorLock.GetInteractionMessage(); 
            }
            
            // 2. RETROALIMENTACIÓN DE AUDIO
            if (source != null && correctSound != null)
            {
                source.PlayOneShot(correctSound);
            }
            
            // 3. Desactivamos los símbolos (para que no se pueda volver a interactuar)
            DeactivateSymbols(); 
        }
        else
        {
            Debug.Log("Símbolo incorrecto. Intenta de nuevo.");
          /*  if (source != null && failSound != null)
            {
                source.PlayOneShot(failSound);
            }*/
        }
    }
    
    private void ActivateSymbols()
    {
        symbolsActive = true;
        foreach (GameObject symbol in selectionSymbols)
        {
            if (symbol != null) symbol.SetActive(true);
        }
    }

    private void DeactivateSymbols()
    {
        symbolsActive = false;
        foreach (GameObject symbol in selectionSymbols)
        {
            // Ocultamos los símbolos o cambiamos su collider para que no se puedan seleccionar más.
            if (symbol != null) symbol.SetActive(false); 
        }
    }
}
