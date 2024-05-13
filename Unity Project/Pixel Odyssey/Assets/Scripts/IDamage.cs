using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void takeDamage(int damageAmount, Vector3 hitPositio); // Interface method for taking damage 
}
