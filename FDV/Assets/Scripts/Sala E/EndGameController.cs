using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; 

public class EndGameController : MonoBehaviour
{
    [Tooltip("El nombre de la escena de Créditos o Menú Principal.")]
    public string nextSceneName = "CreditsScene"; 
    
    private VideoPlayer videoPlayer;
    public GameObject cinematicTriggerObject;
    public Canvas canvasCinematic;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            // [CLAVE] Suscribir un método para cuando el video termine de reproducirse
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        
        // Opcional: Deshabilitar el input del jugador mientras la cinemática está activa
        // PlayerController.DisableMovement(); 
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // 1. Cargar la escena de destino
        canvasCinematic.gameObject.SetActive(false); 
        cinematicTriggerObject.SetActive(false); 
        
        LoadNextScene();
    }

    private void LoadNextScene()
    {
         
        Time.timeScale = 1f; // Asegurar que el tiempo esté normalizado
        SceneManager.LoadScene(nextSceneName);
    }
}