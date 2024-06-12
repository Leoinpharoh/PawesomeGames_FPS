using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
}