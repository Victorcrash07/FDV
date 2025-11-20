using UnityEngine;

public class FinalLock : MonoBehaviour, IInteractable
{
    [Header("Configuración del Puzzle")]
    [Tooltip("El código correcto (Ej: '3142' basado en la receta de la Sala C).")]
    public string correctCode = "3142";

    public CerrojoFinal exitDoorLock;  
    [Header("Referencias de Componentes")]
    
    public Camera playerCamera; 
    public Camera codeInputCamera;  // Cámara que enfoca el teclado/caja
    public GameObject playerObject;
    private CharacterController playerController;
    private MonoBehaviour mouseLookScript;

    public AudioSource audioSource;
    
    // Variables de estado
    private string playerAttempt = "";
    private bool isUnlocked = false;
    private bool inputMode = false; // ¿Está el jugador mirando la caja y listo para escribir?

    void Start()
    {
        
        
        // Configuramos las cámaras
        if (codeInputCamera != null) codeInputCamera.enabled = false;
        if (playerCamera == null) playerCamera = Camera.main;
        playerController = playerObject.GetComponent<CharacterController>();
        mouseLookScript = playerObject.GetComponentInChildren<CameraLook>();
      
    }

    public string GetInteractionMessage()
    {
        if (isUnlocked) return "Puerta Abierta";
        return inputMode ? "Salir (F) o probar combinacion numerica con enter" : "Introducir Código (E)";
    }

    public void Interact()
    {
        if (isUnlocked) return; 

        if (!inputMode)
        {
            EnterInputMode();
        }
        else
        {
            ExitInputMode();
        }
    }
    
    // ----------------------------------------------------
    // Lógica de Entrada (Similar a PianoController)
    // ----------------------------------------------------

    private void EnterInputMode()
    {
        inputMode = true;
        playerAttempt = ""; // Limpiar el intento anterior
        
        // Bloqueo de Cámara y Movimiento
        if (playerCamera != null) playerCamera.enabled = false;
        if (codeInputCamera != null) codeInputCamera.enabled = true;
        
        //Desactiva el controlador del jugador para el movimiento normal
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        //Bloquear la cámara
        mouseLookScript.enabled = false;
        
        Debug.Log("Modo de código activado. Presiona números 0-9 y 'Enter' para enviar.");
    }

    private void ExitInputMode()
    {
        inputMode = false;
        
        // Restauración de Cámaras y Movimiento
        if (codeInputCamera != null) codeInputCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;
        
        mouseLookScript.enabled = true;
        playerController.enabled = true;
        playerObject.GetComponent<PlayerMovement>().enabled = true;
        
        Debug.Log("Modo de código desactivado.");
    }

    void Update()
    {
        if (inputMode && !isUnlocked)
        {
            HandleNumericInput();
        }
    }

    private void HandleNumericInput()
    {
        // Capturar números del 0 al 9
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)) ||
                Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Keypad" + i)))
            {
                if (playerAttempt.Length < correctCode.Length)
                {
                    playerAttempt += i.ToString();
                    // Opcional: Visualiza el intento en la UI o en la consola
                    Debug.Log("Código: " + playerAttempt);
                }
                return;
            }
        }
        
        // Borrar el último dígito
        if (Input.GetKeyDown(KeyCode.Backspace) && playerAttempt.Length > 0)
        {
            playerAttempt = playerAttempt.Substring(0, playerAttempt.Length - 1);
            Debug.Log("Código: " + playerAttempt);
            return;
        }

        // Verificar el código al presionar Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckCode();
        }
        
        // Salir con 'F' (para que el jugador pueda interactuar para salir)
        if (Input.GetKeyDown(KeyCode.F))
        {
            ExitInputMode();
        }
    }
    
    private void CheckCode()
    {
        if (playerAttempt == correctCode)
        {
            isUnlocked = true;
            Debug.Log("¡Código Correcto! Caja Abierta.");
            UnlockBox();
        }
        else
        {
            playerAttempt = ""; // Limpiar el intento
            Debug.Log("Código Incorrecto. Intentar de nuevo.");
            // Opcional: Sonido de error o vibración
        }
    }

    private void UnlockBox()
    {
         if (audioSource != null )//&& correctSound != null)
         {
                audioSource.Play();
         }
        exitDoorLock.isLocked = false;
        ExitInputMode();
        // Nota: El jugador ahora tiene que interactuar con la recompensa (rewardVisual) para cogerla.
    }
}