using UnityEngine;
// NEW SCRIPT: SettingsManager.cs (Fragmento)

using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer masterMixer; // Referencia a tu Audio Mixer
    public GameObject AccesibilidadLuces;

    // Convierte el valor del slider (0 a 1) a decibelios (-80 a 0)
    public void SetMasterVolume(float volume)
    {
        // El logaritmo permite que el control de volumen sea más suave.
        float valorSeguro = Mathf.Clamp(volume, 0.0001f, 1f);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(valorSeguro) * 20);
    }
    
    // Repite esta función para SetMusicVolume(float volume) y SetFXVolume(float volume)

// SettingsManager.cs (Continuación)

    public CameraLook playerCameraLook; // Asignar el script CameraLook

    public void SetMouseSensitivity(float sensitivity)
    {
        // El slider dará un valor (ej: 0.5 a 5). Debes ajustar el rango en tu UI.
        if (playerCameraLook != null)
        {
            playerCameraLook.mouseSensitivity = sensitivity;
        }
    }

    public void SetVisualHints(bool status)
    {
        if (AccesibilidadLuces != null)
        {
            AccesibilidadLuces.SetActive(status);
            
        }
        else
        {
            Debug.LogWarning("¡Cuidado! No has asignado el GameObject de luces en el Inspector.");
        }
    }
}
