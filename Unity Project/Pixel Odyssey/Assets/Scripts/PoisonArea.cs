using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    [SerializeField] public int damageAmount; // The amount of damage to apply
    [SerializeField] public float damageInterval; // Time interval between damage applications

    public SaveSystem saveSystem;
    bool OSUnlocked;
    bool PlayerInZone = false;

    private void Start()
    {
        OSUnlocked = saveSystem.playerData.OvershieldUnlocked;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInZone = true;

            if (!OSUnlocked)
            {
                StartCoroutine(damage(other.gameObject));
            }
        }
        
    }

    IEnumerator damage(GameObject player)
    {
        EDamage damageable = player.GetComponent<EDamage>();

        while (PlayerInZone)
        {
            if (damageable != null)
            {
                damageable.poisonDamage(damageAmount, damageInterval); // Assuming the hit position is the player's current position
            }
            yield return new WaitForSeconds(damageInterval);
        }
        
    }
}
