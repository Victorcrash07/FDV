using UnityEngine;

public class CartaUIManager : MonoBehaviour
{
    public static CartaUIManager Instance;

    [Header("Referencias UI")]
    public GameObject panelCarta;
    public GameObject fondoOscuro;

    [Header("Audio")]
    public AudioSource audioCarta;

    [Header("Player Control")]
    public PlayerMovement playerMovement;
    public CameraLook cameraLook;

    private bool cartaAbierta = false;

    private void Awake()
    {
        Instance = this;
    }

    public void AbrirCarta()
    {
        if (cartaAbierta) return;

        cartaAbierta = true;

        panelCarta.SetActive(true);
        fondoOscuro.SetActive(true);

        if (audioCarta != null)
            audioCarta.Play();

        // Desactivar movimiento del jugador
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (cameraLook != null)
            cameraLook.enabled = false;

        // Liberar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CerrarCarta()
    {
        if (!cartaAbierta) return;

        cartaAbierta = false;

        panelCarta.SetActive(false);
        fondoOscuro.SetActive(false);

        if (audioCarta != null)
            audioCarta.Stop();

        // REACTIVAR movimiento del jugador
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (cameraLook != null)
            cameraLook.enabled = true;

        // Bloquear cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
