using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(int damageAmount, Vector3 hitPosition); // Interface method for taking damage 
}
