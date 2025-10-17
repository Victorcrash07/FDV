using UnityEngine;

public class CameraLook : MonoBehaviour
{
    // Sensibilidad del ratón
    public float mouseSensitivity = 100f;

    // Referencia al Transform del jugador (para rotación horizontal)
    public Transform playerBody;

    // Variable para almacenar la rotación vertical de la cámara
    private float xRotation = 0f;

    void Start()
    {
        // Bloquea el cursor en el centro de la pantalla para que no se vea
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtiene la entrada del ratón en los ejes X e Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación Horizontal (rotamos todo el cuerpo del jugador, no solo la cámara)
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotación Vertical (restamos mouseY porque el eje Y del ratón es invertido por defecto)
        xRotation -= mouseY;

        // Limita la rotación vertical para no poder "dar la vuelta" por completo
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica la rotación vertical solo a la cámara. ( Usamos Quaternion.Euler para convertir el ángulo a una rotación)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
