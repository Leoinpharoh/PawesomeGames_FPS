using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EDamage
{
    void poisonDamage(int damage, float duration);
    void burnDamage(int damage, float duration);
    void freezeDamage(int damage, float duration);
    //void slowDamage(int damage, float duration);
    //void confuseDamage(int damage, float duration);

}
