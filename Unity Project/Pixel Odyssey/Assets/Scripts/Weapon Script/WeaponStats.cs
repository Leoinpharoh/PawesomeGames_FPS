using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    [Header("Gun Stats")]
    [Range(0, 100)] public int shootDamage;
    [Range(10, 500)] public int shootDist;
    public float shootSpeed;
    [Range(1, 50)] public int projectileSpeed;
    [Range(0, 10)] public int numberOfRays;
    public float reloadTime;
    public float LaserWaitTime;

    [HideInInspector] public enum WeaponType { RayCast, Laser, Projectile, SpreadRay, Automatic }
    public WeaponType weaponType;

    [Header("Ammo Stats")]
    public int Ammo;
    public int clip;
    public int TilReload;
    [HideInInspector] public enum AmmoType { Light, Medium, Heavy }
    public AmmoType ammoType;

    [Header("Audio Files")]
    public AudioClip audioSFXShoot;
    public AudioClip audioSFXReload;
}