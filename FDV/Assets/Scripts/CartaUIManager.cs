using UnityEngine;

public class CartaUIManager : MonoBehaviour
{
    public static CartaUIManager Instance;

    [Header("UI")]
    public GameObject panelCarta;   // El panel donde está el texto

    [Header("Player")]
    public PlayerMovement playerMovement;
    public CameraLook cameraLook;

    private bool cartaAbierta = false;
    public bool CartaAbierta => cartaAbierta;   // para consultarlo desde otros scripts

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Si la carta está abierta y el jugador pulsa E, cerramos
        if (cartaAbierta && Input.GetKeyDown(KeyCode.E))
        {
            CerrarCarta();
        }
    }

    public void AbrirCarta()
    {
        if (cartaAbierta) return;   // por si acaso

        cartaAbierta = true;

        if (panelCarta != null)
            panelCarta.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (cameraLook != null)
            cameraLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Carta ABRIENDO");
    }

    public void CerrarCarta()
    {
        if (!cartaAbierta) return;

        cartaAbierta = false;

        if (panelCarta != null)
            panelCarta.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (cameraLook != null)
            cameraLook.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Carta CERRANDO");
    }
}
