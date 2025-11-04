using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para el botón de Salir/Menú Principal

public class PauseMenu : MonoBehaviour
{
    // Una variable estática para saber en qué estado está el juego
    public static bool GameIsPaused = false; 

    // Referencia al GameObject del panel de menú
    public GameObject pauseMenuUI; 

    void Update()
    {
        // La tecla estándar para pausar es Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // 1. Reanudar el Juego
    public void Resume()
    {
        // Oculta el panel del menú
        pauseMenuUI.SetActive(false); 
        
        // El tiempo vuelve a fluir a velocidad normal
        Time.timeScale = 1f; 
        
        GameIsPaused = false;
        
        // Opcional: Desbloquea el cursor si estaba bloqueado por el menú
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    // 2. Pausar el Juego
    void Pause()
    {
        // Muestra el panel del menú
        pauseMenuUI.SetActive(true); 
        
        // Detiene el tiempo en el juego (Time.timeScale = 0f)
        Time.timeScale = 0f; 
        
        GameIsPaused = true;
        
        // Desbloquea el cursor para poder interactuar con los botones
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    // 3. Botón para Salir/Ir al Menú Principal
    public void LoadMenu()
    {
        // Primero aseguramos que el tiempo vuelva a la normalidad antes de cambiar de escena
        Time.timeScale = 1f; 
        
        // Carga la escena de tu menú principal
        SceneManager.LoadScene("MainMenu"); 
        
        Debug.Log("Cargando Menú Principal...");
    }

    // 4. Botón para Salir de la Aplicación (solo funciona en build)
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
