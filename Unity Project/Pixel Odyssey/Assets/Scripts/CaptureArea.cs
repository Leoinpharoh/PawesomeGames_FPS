using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAreaLogic : MonoBehaviour
{
    public string capturingTag = "Player"; // The tag of the capturing object
    private bool isCaptured = false;
    private float captureProgress = 1f;
    public float captureTime = 10f; // Time required to capture the area
    public GameObject captureAreaObject; // The object to destroy after capture
    [SerializeField] public int damageAmount; // The amount of damage to apply
    [SerializeField] public float damageInterval; // Time interval between damage applications
    private HashSet<GameObject> playersInZone = new HashSet<GameObject>(); // Track players inside the capture area
    private Coroutine captureCoroutine; // Reference to the capture coroutine

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(capturingTag) && !isCaptured)
        {
            Debug.Log("Player entered capture area");
            playersInZone.Add(other.gameObject);
            captureCoroutine = StartCoroutine(CaptureCoroutine(other.gameObject));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(capturingTag) && !isCaptured)
        {
            Debug.Log("Player exited capture area");
            playersInZone.Remove(other.gameObject);

            if (captureCoroutine != null)
            {
                StopCoroutine(captureCoroutine);
                captureProgress = 0f; // Reset capture progress
            }

            StartCoroutine(ApplyDamageOutsideZone(other.gameObject));
        }
    }

    private System.Collections.IEnumerator CaptureCoroutine(GameObject capturingObject)
    {
        while (captureProgress < captureTime)
        {
            captureProgress += Time.deltaTime;
            yield return null;
        }

        CaptureComplete();
    }

    private void CaptureComplete()
    {
        isCaptured = true;
        Debug.Log("Area captured!");
        // Additional logic for when the area is captured

        if (captureAreaObject != null)
        {
            Destroy(captureAreaObject);
        }
        else
        {
            Debug.LogWarning("No object specified to destroy.");
        }
    }

    private System.Collections.IEnumerator ApplyDamageOutsideZone(GameObject player)
    {
        EDamage damageable = player.GetComponent<EDamage>();

        if (damageable != null)
        {
            while (!playersInZone.Contains(player) && !isCaptured)
            {
                damageable.poisonDamage( damageAmount,damageInterval); // Assuming the hit position is the player's current position
                yield return new WaitForSeconds(damageInterval);
            }
        }
        else
        {
            Debug.LogWarning("The player does not implement IDamage interface.");
        }
    }
}
