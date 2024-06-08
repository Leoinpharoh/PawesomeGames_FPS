using UnityEngine;

public abstract class Potion : ScriptableObject
{
    public string potionName;

    public abstract void Use(GameObject user);
}
