using UnityEngine;
using System.Collections;

public class GoteoGrifo : MonoBehaviour
{
    [Header("Referencias")]
    public ParticleSystem gotas;
    public AudioSource audioGoteo;

    [Header("Intervalo de goteo (segundos)")]
    public float minInterval = 2f;
    public float maxInterval = 4f;

    private void Start()
    {
        StartCoroutine(GoteoLoop());
    }

    private IEnumerator GoteoLoop()
    {
        while (true)
        {
            // Espera un tiempo aleatorio entre min y max
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Emitir 1 gota
            if (gotas != null)
            {
                gotas.Emit(1);
            }

            // Sonido de goteo
            if (audioGoteo != null)
            {
                audioGoteo.Play();
            }
        }
    }
}
