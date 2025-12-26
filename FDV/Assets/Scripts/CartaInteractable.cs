using UnityEngine;

public class CartaInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject cartaPanel; // FondoCarta

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MonoBehaviour cameraLook; // tu script de cámara
    [SerializeField] private PlayerInteraction playerInteraction;

    [Header("Keys")]
    [SerializeField] private KeyCode closeKey = KeyCode.E;
    [SerializeField] private KeyCode closeAltKey = KeyCode.Escape;

    [Header("Audio")]
    [SerializeField] private AudioSource cartaAudio;

    private bool isOpen = false;

    private void Start()
    {
        cartaPanel.SetActive(false);
    }

    private void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(closeKey) || Input.GetKeyDown(closeAltKey))
        {
            CloseCarta();
        }
    }

    public void Interact()
    {
        Debug.Log("CartaInteractable.Interact() llamado");
        if (!isOpen)
            OpenCarta();
        else
            CloseCarta();
    }

    public string GetInteractionMessage()
    {
        return isOpen ? "Cerrar carta (E / Esc)" : "Leer carta (E)";
    }

    private void OpenCarta()
    {
        Debug.Log("OpenCarta() - Activando panel: " + (cartaPanel != null ? cartaPanel.name : "NULL"));
        isOpen = true;
        cartaPanel.SetActive(true);
        playerInteraction.isReading = true;

        if (playerInteraction != null)
            playerInteraction.ResetInteractionUI();

        // Reproducir audio si está asignado
        if (cartaAudio != null) cartaAudio.Play();

        // Bloquear jugador
        playerMovement.enabled = false;
        cameraLook.enabled = false;
        playerInteraction.enabled = false;

        // Cursor libre
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseCarta()
    {
        isOpen = false;
        cartaPanel.SetActive(false);
        playerInteraction.isReading = false;

        // Parar audio si está sonando
        if (cartaAudio != null)
            cartaAudio.Stop();

        // Reactivar jugador
        playerMovement.enabled = true;
        cameraLook.enabled = true;
        playerInteraction.enabled = true;

        // Cursor FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Input.ResetInputAxes();
    }
}
