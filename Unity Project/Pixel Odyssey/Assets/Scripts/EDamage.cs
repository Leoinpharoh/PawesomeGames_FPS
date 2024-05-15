using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EDamage
{
    void poisonDamage(int damage, float duration);
    void burnDamage(string effect);
    //void freezeDamage(string effect);
    //void slowDamage(string effect);
    //void confuseDamage(string effect);

}
