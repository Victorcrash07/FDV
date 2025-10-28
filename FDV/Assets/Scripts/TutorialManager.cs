using UnityEngine;
using TMPro;
using System.Collections;
public enum TutorialAction
{
    Delante,
    Atrás,
    Izquierda,
    Derecha,
    RotarCamara,
    interactuar,
    StartGame
}

[System.Serializable]
public struct TutorialStep
{
    [Tooltip("El texto que se mostrará al jugador.")]
    public string instructionText;
    
    [Tooltip("La acción que el jugador debe completar para pasar al siguiente paso.")]
    public TutorialAction requiredAction;
}
public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    
    public TutorialStep[] steps;

    private int currentStepIndex = 0;

    /// <summary>
    /// Devuelve el índice del paso actual (solo lectura).
    /// </summary>
    public int CurrentStepIndex => currentStepIndex;

    /// <summary>
    /// Comprueba si la acción requerida en el paso actual coincide con la acción solicitada.
    /// Devuelve false si no hay pasos válidos o el índice está fuera de rango.
    /// </summary>
    public bool IsCurrentRequiredAction(TutorialAction action)
    {
        if (steps == null || steps.Length == 0 || currentStepIndex >= steps.Length)
            return false;

        return steps[currentStepIndex].requiredAction == action;
    }
    
    void Start()
    {
        DisplayCurrentStep();
    }

    void Update()
    {
        // Si ya terminamos el tutorial, salimos
        if (currentStepIndex >= steps.Length) return;

        // Comprueba la acción requerida para el paso actual
        CheckAction(steps[currentStepIndex].requiredAction);
    }
    
    // Muestra el texto de la instrucción actual
    private void DisplayCurrentStep()
    {
        if (tutorialText == null || steps == null || steps.Length == 0 || currentStepIndex >= steps.Length)
        {
            return;
        }

        tutorialText.text = steps[currentStepIndex].instructionText;
    }

    // Avanza al siguiente paso
    public void NextStep()
    {
        currentStepIndex++;
        
        if (currentStepIndex < steps.Length)
        {
            DisplayCurrentStep();
        }
        else
        {
            // El tutorial ha terminado
           tutorialText.text = "¡Tutorial completado! ¡Escapa de la sala!";
            
            // 1. Iniciamos la Coroutine para que el texto desaparezca en 5 segundos
            StartCoroutine(DisableTextAfterDelay(5f));
            this.enabled = false; 
        }
    }

    // Coroutine: Espera un tiempo y luego desactiva el objeto de texto
    private IEnumerator DisableTextAfterDelay(float delay)
    {
        // Espera la cantidad de segundos especificada
        yield return new WaitForSeconds(delay);

        // Comprueba si el texto sigue existiendo antes de desactivarlo
        if (tutorialText != null)
        {
            // Desactivamos el GameObject completo para ocultar el texto
            tutorialText.gameObject.SetActive(false); 
        }
    }

    // Lógica principal para comprobar las entradas
    private void CheckAction(TutorialAction action)
    {
        bool actionCompleted = false;

        switch (action)
        {
            case TutorialAction.Delante:
                actionCompleted = Input.GetKey(KeyCode.W);
                break;
            case TutorialAction.Atrás:
                actionCompleted = Input.GetKey(KeyCode.S);
                break;
            case TutorialAction.Izquierda:
                actionCompleted = Input.GetKey(KeyCode.A);
                break;
            case TutorialAction.Derecha:
                actionCompleted = Input.GetKey(KeyCode.D);
                break;
            case TutorialAction.RotarCamara:
                // Comprueba si ha habido un movimiento significativo del ratón
                actionCompleted = (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0);
                break;
            case TutorialAction.interactuar:
                // Para la interacción, necesitamos que el Raycast golpee algo y se presione 'E'
                // Esto es más complejo, lo gestionaremos en el paso 4.
                break;
            case TutorialAction.StartGame:
                // Último paso, puede ser que el jugador presione cualquier tecla para empezar.
                actionCompleted = Input.anyKeyDown;
                break;
        }

        if (actionCompleted)
        {
            NextStep();
        }
    }
}
