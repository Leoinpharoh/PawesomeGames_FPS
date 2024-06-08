using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObjects : MonoBehaviour, SDamage
{

    [SerializeField] int ObjectHP;
    
    public void ObjectDamage(int damageAmount)
    {
        ObjectHP -= damageAmount;
        if (ObjectHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
