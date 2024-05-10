using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;

    [SerializeField] int HP;
    public void takeDamage(int damageAmount)
    {
        HP -= damageAmount;
        StartCoroutine(flash());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}