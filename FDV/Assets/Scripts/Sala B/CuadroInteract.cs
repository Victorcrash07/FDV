using UnityEngine;

public class CuadroInteract : MonoBehaviour, IInteractable
{
    // VARIABLES DE ESTADO Y OBJETOS GLOBALES (AHORA EN ESTE SCRIPT)
    private int fragmentosColocados = 0; 
    private const int TOTAL_FRAGMENTOS = 2;
    
    [Header("Objetos Controlados")]
    [Tooltip("El GameObject que se activa cuando se coloca el Fragmento 1.")]
    public GameObject fragmentoVisual1; 
    [Tooltip("El GameObject que se activa cuando se coloca el Fragmento 2.")]
    public GameObject fragmentoVisual2; 

    public GameObject incompletePicture; 
    public GameObject completePicture;   
    public CerrojoPuerta exitDoorLock;   
    public CerrojoPuerta PuertaD;
    [Header("Audio")]
    public AudioSource audioSource; // ¡Pon este componente en el mismo GameObject del cuadro!
   // public AudioClip correctSound; // Sonido de éxito al completar el puzle
    
    [Header("Fragmentos Requeridos")]
    public InventoryItem requiredFragment1 = InventoryItem.FragmentoCuadro1; 
    public InventoryItem requiredFragment2 = InventoryItem.FragmentoCuadro2; 

    // Estados para saber qué fragmentos ya están colocados
    private bool fragment1Placed = false;
    private bool fragment2Placed = false;
    private InventoryItem PuzzleComplete = InventoryItem.PuzzleCuadroCompleto; 

    void Start()
    {
        // Asignación de AudioSource si falta
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        
        // El cuadro completo debe estar oculto al inicio
        if (completePicture != null) completePicture.SetActive(false);
    }

    public string GetInteractionMessage()
    {
        // ... (La lógica de mensaje se mantiene igual para indicar qué falta) ...
        int placedCount = (fragment1Placed ? 1 : 0) + (fragmentoVisual2.activeInHierarchy ? 1 : 0);
        int missingCount = TOTAL_FRAGMENTOS - placedCount;
        
        if (missingCount == 0) return "Cuadro completo.";
        
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

        // LÓGICA DE COLOCACIÓN Y CONTEO
        
        // 1. Intentar colocar el FRAGMENTO 1
        if (!fragment1Placed && InventoryManager.Instance.ContainsItem(requiredFragment1))
        {
            PlaceAndCheck(requiredFragment1, ref fragment1Placed, fragmentoVisual1);
        }
        
        // 2. Intentar colocar el FRAGMENTO 2
        else if (!fragment2Placed && InventoryManager.Instance.ContainsItem(requiredFragment2))
        {
            PlaceAndCheck(requiredFragment2, ref fragment2Placed, fragmentoVisual2);
        }
    }
    
    // Método auxiliar para limpiar la lógica de colocación
    private void PlaceAndCheck(InventoryItem item, ref bool isPlacedFlag, GameObject visual)
    {
        InventoryManager.Instance.RemoveItem(item);
        
        // Lógica Visual
        if (visual != null) visual.SetActive(true); 
        isPlacedFlag = true;

        fragmentosColocados++;
        
        Debug.Log($"Fragmento {item.ToString()} colocado. Total: {fragmentosColocados}/{TOTAL_FRAGMENTOS}");

        if (fragmentosColocados == TOTAL_FRAGMENTOS)
        {
            // EL PUZLE SE HA COMPLETADO. DESBLOQUEO DIRECTO.
            RevealAndUnlock();
        }
    }

    private void RevealAndUnlock()
    {
        // 1. AUDIO DE ÉXITO
         if (audioSource != null )//&& correctSound != null)
         {
                audioSource.Play();
         }
        
        // 2. DESBLOQUEO DE LA PUERTA
        if (exitDoorLock != null)
        {
            // Nota: isLocked debe ser público en CerrojoPuerta.cs
            InventoryManager.Instance.AddItem(PuzzleComplete);
            exitDoorLock.isLocked = false;
            PuertaD.isLocked = false;
        }
        // 3. REVELACIÓN VISUAL FINAL
        if (incompletePicture != null) incompletePicture.SetActive(false);
        if (completePicture != null) completePicture.SetActive(true);
        fragmentoVisual1.SetActive(false);
        fragmentoVisual2.SetActive(false);
        
        
        
       
        // 4. Desactivar el componente de interacción para que no se pueda interactuar más
        GetComponent<Collider>().enabled = false;
        
        // **QUITAMOS LA LÓGICA DEL SYMBOL SELECTOR Y LOS ARRAYS DE SÍMBOLOS**
    }
}