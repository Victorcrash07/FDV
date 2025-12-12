using UnityEngine;

public class ColorAssistManager : MonoBehaviour
{
    // Singleton pattern para acceso global
    public static ColorAssistManager Instance; 
    
    public Color highlightColor = Color.clear; // El color base (inactivo/transparente)
    private const string LIGHT_TAG = "InteractableLight"; // Tag para buscar las luces

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas si es necesario
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 1. Llamado desde el Slider o el Selector de color del Menú de Ajustes
    public void SetHighlightColor(Color newColor)
    {
        highlightColor = newColor;
        
        // Aplicar el cambio inmediatamente a todos los objetos ya instanciados
        UpdateAllInteractableLights();
    }

    // 2. Aplica el color a todas las luces marcadas en la escena actual
    public void UpdateAllInteractableLights()
    {
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        
        foreach (Light light in lights)
        {
            // Solo modifica las luces que tienen el TAG correcto
            if (light.gameObject.CompareTag(LIGHT_TAG))
            {
                light.color = highlightColor;
                
                // Si el color es Color.clear o casi negro, desactiva la luz
                if (highlightColor == Color.clear || highlightColor.grayscale < 0.1f)
                {
                    light.enabled = false;
                }
                else
                {
                    light.enabled = true;
                }
            }
        }
    }
}