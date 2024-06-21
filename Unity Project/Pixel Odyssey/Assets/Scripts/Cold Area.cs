using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdArea : MonoBehaviour
{
    public SaveSystem saveSystem;
    public PlayerManager playerManager;
    bool OSUnlocked;
    int OSAmount;
    public int damageEffectAmount = 1;
    public int damageAmount = 2;
    public float damageDuration = 12;
    public bool DamageOverTime = false;
    public bool PlayerInZone = false;

    // Start is called before the first frame update
    void Start()
    {
        OSUnlocked = saveSystem.playerData.OvershieldUnlocked;
        OSAmount = playerManager.OS;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !DamageOverTime)
        {
            PlayerInZone = true;

            if (!OSUnlocked || playerManager.OS <= 0)
            {
                StartCoroutine(damage(other.gameObject));
            }
            StartCoroutine(Idamager(other.gameObject));
        }

    }

    private void OnDisable()
    {
        DamageOverTime = false;
        PlayerInZone = false;
    }

    IEnumerator damage(GameObject player)
    {
        EDamage damageable = player.GetComponent<EDamage>();
        DamageOverTime = true;
        while (PlayerInZone)
        {
            if (damageable != null)
            {
                damageable.freezeDamage(damageAmount, damageDuration); // Assuming the hit position is the player's current position
            }
            yield return new WaitForSeconds(damageDuration);
            DamageOverTime = false;
        }

    }

    IEnumerator Idamager(GameObject player)
    {

        IDamage Idamageable = player.GetComponent<IDamage>();

        if (Idamageable != null)
        {
            Idamageable.takeDamage(damageAmount, Vector3.zero);
        }
        yield return new WaitForSeconds(damageDuration);
    }
}
