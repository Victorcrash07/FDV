using UnityEngine;

public class PasilloPianoTrigger : MonoBehaviour
{
    public AudioSource pianoSource;
    private bool alreadyPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPlayed) return;

        if (other.CompareTag("Player"))
        {
            pianoSource.Play();
            alreadyPlayed = true;
        }
    }
}
