using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para el botón de Salir/Menú Principal

public class PauseMenu : MonoBehaviour
{
    // Una variable estática para saber en qué estado está el juego
    public static bool GameIsPaused = false; 

    // Referencia al GameObject del panel de menú
    public GameObject pauseMenuUI; 
    public GameObject settingsMenuUI;
    // PauseMenu.cs (Añadir al inicio)

    public GameObject tutorialManager; 

// ... (En el botón del Menú Rápido, el evento OnClick llama a: 
// tutorialManager.RestartTutorial())
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
        settingsMenuUI.SetActive(false);
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

    public void StartGame()
    {
        // 1. Aseguramos que el tiempo fluya normalmente
        Time.timeScale = 1f; 
        
        // 2. Carga la escena principal del juego
        SceneManager.LoadScene("Casa_Juego"); 
        
        Debug.Log("Iniciando juego...");
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

    // PauseMenu.cs (Nuevo método)

    public void OpenSettings()
    {
        if (pauseMenuUI != null)
        {
            // 1. Ocultar el panel principal
            pauseMenuUI.SetActive(false);
        }
        
        if (settingsMenuUI != null)
        {
            // 2. Mostrar el panel de ajustes
            settingsMenuUI.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsMenuUI != null)
        {
            // 1. Ocultar el panel de ajustes
            settingsMenuUI.SetActive(false);
        }
        
        if (pauseMenuUI != null)
        {
            // 2. Mostrar el panel principal
            pauseMenuUI.SetActive(true);
        }
    }

    public void ReiniciarTutorial()
    {
        if (tutorialManager != null)
        {
            TutorialManager tutorialScript = tutorialManager.GetComponent<TutorialManager>();
            if (tutorialScript != null)
            {
                tutorialScript.RestartTutorial();
            }
            else
            {
                Debug.LogWarning("El componente TutorialManager no se encontró en el GameObject asignado.");
            }
        }
        else
        {
            Debug.LogWarning("El GameObject del TutorialManager no está asignado en el PauseMenu.");
        }  
    }


}
