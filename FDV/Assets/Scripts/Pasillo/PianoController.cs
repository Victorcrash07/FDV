using UnityEngine;
using System.Collections.Generic;

public class PianoController : MonoBehaviour, IInteractable
{
    [Header("Configuración del Puzzle")]
    [Tooltip("Secuencia numérica correcta (Do=1, Re=2, etc.)")]
    public List<int> correctSequence = new List<int> { 4, 3, 4, 2, 3, 1, 2 }; // Ejemplo: Do-Mi-Sol-Fa-Re-Do
    
    [Tooltip("Tiempo máximo entre notas para que la secuencia sea válida (ej: 1.5s).")]
    public float maxTimeBetweenNotes = 1.5f;

    [Header("Referencias de Control")]
    public Camera playerCamera;         // La cámara del jugador (para apagarla)
    public Camera pianoCamera;          // La cámara que enfoca el teclado (para encenderla)
    public CerrojoPuerta doorToUnlock;  // La puerta de la Sala C
    public GameObject playerObject;
    private CharacterController playerController;
    private MonoBehaviour mouseLookScript;
    [Header("Componentes de Audio")]
    public AudioSource pianoAudioSource; // El componente AudioSource en el Piano
    [Tooltip("Clips de notas, indexados por Do=1, Re=2, etc.")]
    public AudioClip[] noteSounds;      // Array de 7 clips (Do a Si)
    public AudioSource unlockSound;       // Sonido de cerradura desbloqueada/éxito
    [Header("Animación de Teclas")]
    [Tooltip("Animators de las teclas. El orden DEBE ser Do=0, Re=1, ... Si=6.")]
    public Animator[] keyAnimators; // Array de 7 Animators

    // [NUEVO] Persistencia del puzzle
    [Header("Persistencia del Estado")]
    public bool puzzleSolved = false;
    // VARIABLES DE ESTADO
    private List<int> playerInput = new List<int>();
    private float lastNoteTime;
    private bool pianoMode = false; // ¿Está el jugador en modo de tocar el piano?

    void Start()
    {
        // Asegurarse de que el piano esté apagado al inicio
        pianoCamera.enabled = false;
        // Asignación de Player Camera (si no está ya en el Inspector)
        if (playerCamera == null) playerCamera = Camera.main;
       
        playerController = playerObject.GetComponent<CharacterController>();
        mouseLookScript = playerObject.GetComponentInChildren<CameraLook>();
    }
    public string GetInteractionMessage()
    {
        return pianoMode ? "Salir del Piano (F)" : "Tocar Piano (E)";
    }

    public void Interact()
    {
        if (!pianoMode)
        {
            EnterPianoMode();
        }
    }

    // [NUEVO MÉTODO] Habilita la cámara del piano y la entrada numérica
    private void EnterPianoMode()
    {
        pianoMode = true;
        
        // 1. Apagar la cámara del jugador y encender la del piano
        if (playerCamera != null) playerCamera.enabled = false;
        pianoCamera.enabled = true;
        
        // 2. Liberar el cursor y bloquear el movimiento del jugador
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Desactiva el controlador del jugador para el movimiento normal
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        //Bloquear la cámara
        mouseLookScript.enabled = false;
    
        // 3. Resetear la secuencia SOLO si el puzle no está resuelto
        if (!puzzleSolved)
        {
            playerInput.Clear();
            Debug.Log("Modo Piano Activo. Presiona 1-7 para tocar.");
        }
        else
        {
            Debug.Log("Modo Piano Activo. Puzle ya resuelto.");
        }
    }
    
    // [NUEVO MÉTODO] Vuelve al modo normal de juego
    private void ExitPianoMode()
    {
        pianoMode = false;
        
        // 1. Apagar la cámara del piano y encender la del jugador
        pianoCamera.enabled = false;
        if (playerCamera != null) playerCamera.enabled = true;
        
        // 2. Bloquear el cursor y restaurar el movimiento
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseLookScript.enabled = true;
        playerController.enabled = true;
        playerObject.GetComponent<PlayerMovement>().enabled = true;
        
        playerInput.Clear();
    }
    void Update()
    {
        // 1. Salir del modo piano (Usando la tecla F)
        if (pianoMode && Input.GetKeyDown(KeyCode.F))
        {
            ExitPianoMode();
            return;
        }

        // 2. Capturar notas SOLO en modo piano
        if (pianoMode)
        {
            for (int i = 1; i <= 7; i++) // Del 1 al 7 (Do a Si)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)))
                {
                    CheckNote(i);
                    break;
                }
            }
        }
    }

    private void CheckNote(int note)
    {
        // 1. REPRODUCIR SONIDO DE NOTA
        if (pianoAudioSource != null && note > 0 && note <= noteSounds.Length)
        {
            pianoAudioSource.PlayOneShot(noteSounds[note - 1]); // Usamos [note - 1] porque el array va de 0 a 6
        }
         // 2. [NUEVA LÓGICA] ACTIVAR ANIMACIÓN
        if (keyAnimators != null && note > 0 && note <= keyAnimators.Length)
        {
            Animator keyAnimator = keyAnimators[note - 1];
            if (keyAnimator != null)
            {
                // Dispara el Trigger 'Press' que hemos creado en el Animator Controller
                keyAnimator.SetTrigger("Press"); 
            }
        }

        float currentTime = Time.time;

        // Limpiar la secuencia si el tiempo entre notas es demasiado largo
        if (playerInput.Count > 0 && currentTime - lastNoteTime > maxTimeBetweenNotes)
        {
            playerInput.Clear();
            Debug.Log("Tiempo agotado. Secuencia reiniciada.");
        }
        
        // 1. Añadir la nota actual
        playerInput.Add(note);
        lastNoteTime = currentTime;
        
        // Opcional: Sonido de la nota (AudioSource.PlayOneShot(notes[note]))
        Debug.Log($"Nota tocada: {note}"); 

        // 2. Verificación de la secuencia
        if (playerInput.Count > correctSequence.Count)
        {
            // La secuencia es demasiado larga, reiniciar
            playerInput.Clear(); 
            Debug.Log("Secuencia incorrecta, demasiado larga.");
        }
        else if (playerInput.Count == correctSequence.Count)
        {
            // Secuencia de la longitud correcta, verificar si coincide
            CheckFullSequence();
        }
        else
        {
            // Verificar si el inicio de la secuencia es correcto
            CheckPartialSequence();
        }
    }
    
    private void CheckPartialSequence()
    {
        for (int i = 0; i < playerInput.Count; i++)
        {
            if (playerInput[i] != correctSequence[i])
            {
                // El jugador cometió un error en el medio, reiniciar
                playerInput.Clear();
                Debug.Log("Error en la secuencia. Reiniciando.");
                return;
            }
        }
    }

    private void CheckFullSequence()
    {
        // Usamos el método de comparación de la verificación parcial
        CheckPartialSequence(); 

        if (playerInput.Count == correctSequence.Count)
        {
            // Si el jugador llegó hasta aquí, la secuencia es correcta
            UnlockDoor();
        }
    }
    
    private void UnlockDoor()
    {
        if (doorToUnlock != null)
        {
             if (unlockSound != null && puzzleSolved == false)
         {
                unlockSound.Play();
         }
            doorToUnlock.isLocked = false;
            puzzleSolved = true;
            Debug.Log("¡Secuencia correcta! La puerta se ha desbloqueado.");
            // Opcional: Sonido de desbloqueo de cerradura
            ExitPianoMode();
        }
    }
}