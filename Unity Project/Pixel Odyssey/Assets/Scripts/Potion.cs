using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : ScriptableObject
{
    public string potionName;

    public abstract void UsePotion(GameObject player);
}

