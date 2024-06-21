using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerFlameBurn : MonoBehaviour
{
    public SaveSystem saveSystem;
    public PlayerManager playerManager;
    bool OSUnlocked;
    int OSAmount;
    public int damageEffectAmount = 3;
    public int damageAmount = 20;
    public float damageDuration = 1;
    // Start is called before the first frame update
    void Start()
    {
        OSUnlocked = saveSystem.playerData.OvershieldUnlocked;
        OSAmount = playerManager.OS;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            if (!OSUnlocked && OSAmount > 0)
            {
                StartCoroutine(Edamage(other.gameObject));
            }
        }
        StartCoroutine(Idamager(other.gameObject));
    }
    IEnumerator Edamage(GameObject player)
    {
        EDamage damageable = player.GetComponent<EDamage>();

        if (damageable != null)
        {
            damageable.poisonDamage(damageEffectAmount, damageDuration);
        }
        yield return new WaitForSeconds(damageDuration);

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
