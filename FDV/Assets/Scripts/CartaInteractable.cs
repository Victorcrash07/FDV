using UnityEngine;

public class CartaInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject cartaPanel; // FondoCarta (panel raíz)

    [Header("Player refs")]
    [SerializeField] private MonoBehaviour playerMovement; // tu PlayerMovement
    [SerializeField] private MonoBehaviour cameraLook;     // tu CameraLook

    [Header("Opcional")]
    [SerializeField] private KeyCode closeKey = KeyCode.E;
    [SerializeField] private KeyCode closeAltKey = KeyCode.Escape;

    private bool isOpen;

    private void Start()
    {
        if (cartaPanel != null) cartaPanel.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if (!isOpen) return;

        // Cerrar con E o ESC
        if (Input.GetKeyDown(closeKey) || Input.GetKeyDown(closeAltKey))
            Close();
    }

    public void Interact()
    {
        if (!isOpen) Open();
        else Close();
    }

    public string GetInteractionMessage()
    {
        return isOpen ? "Cerrar carta (E / Esc)" : "Leer carta (E)";
    }

    private void Open()
    {
        isOpen = true;
        if (cartaPanel != null) cartaPanel.SetActive(true);

        // bloquear controls
        if (playerMovement != null) playerMovement.enabled = false;
        if (cameraLook != null) cameraLook.enabled = false;

        // cursor libre para leer
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Close()
    {
        isOpen = false;
        if (cartaPanel != null) cartaPanel.SetActive(false);

        // devolver controls
        if (playerMovement != null) playerMovement.enabled = true;
        if (cameraLook != null) cameraLook.enabled = true;

        // cursor como FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Evita “inputs pegados” (como hacéis con el armario)
        Input.ResetInputAxes();
    }
}
