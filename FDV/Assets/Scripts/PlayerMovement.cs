using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   private CharacterController controller;

    // Variables de configuración
    public float speed = 12f;
    public float gravity = -9.81f; 

    // Variables de suelo (necesarias para saber cuándo dejar de caer)
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    private Vector3 velocity; 
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Detección de Suelo
        // Esto es esencial para evitar que la gravedad acumule velocidad infinita.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Si toca el suelo Y está cayendo (velocidad negativa), resetea la velocidad.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Lo 'pega' ligeramente al suelo para estabilidad.
        }

        // 2. Movimiento Horizontal (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Vector de movimiento basado en la dirección a la que mira el jugador.
        Vector3 move = transform.right * x + transform.forward * z;

        // Aplica el movimiento horizontal
        controller.Move(move * speed * Time.deltaTime);

        // 3. Aplicación de Gravedad
        // Suma la gravedad a la velocidad vertical en cada frame.
        velocity.y += gravity * Time.deltaTime;

        // Aplica la velocidad vertical (caída/gravedad).
        controller.Move(velocity * Time.deltaTime);
    }
}
