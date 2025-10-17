using UnityEngine;

public class Armario : MonoBehaviour, IInteractable
{
    // Posición dentro del armario.
    public Transform hidePosition; 
    
    // Referencia al objeto del jugador.
    public GameObject playerObject;

    private bool isHiding = false;
    private CharacterController playerController;
    private MonoBehaviour mouseLookScript;

    void Start()
    {
        playerController = playerObject.GetComponent<CharacterController>();
        mouseLookScript = playerObject.GetComponentInChildren<CameraLook>();

    }

    public string GetInteractionMessage()
    {
        return isHiding ? "Salir del armario (E)" : "Esconderse (E)";
    }

    public void Interact()
    {
        if (isHiding)
        {
            ExitLocker();
        }
        else
        {
            EnterLocker();
        }
    }

    private void EnterLocker()
    {
        //Desactiva el controlador del jugador para el movimiento normal
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        //Bloquear la cámara
         mouseLookScript.enabled = false;
        //Desactivar el CharacterController para forzar el teletransporte
        playerController.enabled = false;
        
        //Teletransporte
        playerObject.transform.position = hidePosition.position;
        playerObject.transform.rotation = hidePosition.rotation;
        
        //Volvemos a activar el CharacterController después del teletransporte.
        //Esto le permite manejar las colisiones mientras está dentro.
        playerController.enabled = true; 
        isHiding = true;
    }

    private void ExitLocker()
    {
        isHiding = false;
    //Reinicia la entrada para que si le estabas dando a la w no salgas disparado
        Input.ResetInputAxes();
        playerController.enabled = false;
    
   
    
    // Usamos la rotación de hideposition para calcular la dirección de salida
    // Mueve al jugador 1.5 unidades hacia adelante desde hideposition
    Vector3 exitPosition = hidePosition.position + hidePosition.forward * 1.5f;

    playerObject.transform.position = exitPosition;
    playerObject.transform.rotation = hidePosition.rotation; // Rota al jugador para que mire hacia afuera
    // desbloquear la cámara
    mouseLookScript.enabled = true;
    playerController.enabled = true;
    playerObject.GetComponent<PlayerMovement>().enabled = true;
    playerController.Move(Vector3.up * 0.1f);
    
    }
}