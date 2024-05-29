using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMine : MonoBehaviour
{
    [SerializeField] enum TrapType { Normal, Poisoned, Burning, Freezing, Slowed, Confused }
    [SerializeField] TrapType type;
    [SerializeField] int DMG;
    [SerializeField] float duration;

    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case TrapType.Normal:
                other.gameObject.GetComponent<PlayerManager>().poisoned = false;
                other.gameObject.GetComponent<PlayerManager>().burning = false;
                other.gameObject.GetComponent<PlayerManager>().freezing = false;
                other.gameObject.GetComponent<PlayerManager>().slowed = false;
                other.gameObject.GetComponent<PlayerManager>().confused = false;
                other.gameObject.GetComponent<PlayerManager>().Normal = true;
                other.gameObject.GetComponent<PlayerManager>().StopAllCoroutines();
                GameManager.Instance.playerEffect("Normal");
                break;
            case TrapType.Poisoned:
                other.gameObject.GetComponentInChildren<PlayerManager>().poisonDamage(DMG, duration);
                break;
            case TrapType.Burning:
                other.gameObject.GetComponentInChildren<PlayerManager>().burnDamage(DMG, duration);
                break;
            case TrapType.Freezing:
                other.gameObject.GetComponentInChildren<PlayerManager>().freezeDamage(DMG, duration);
                break;
            case TrapType.Slowed:
                other.gameObject.GetComponentInChildren<PlayerManager>().slowDamage(DMG, duration);
                break;
            case TrapType.Confused:
                other.gameObject.GetComponentInChildren<PlayerManager>().confuseDamage(DMG, duration);
                break;
        }
        GameManager.Instance.playerEffect(type.ToString());

        Destroy(gameObject);
    }
}
