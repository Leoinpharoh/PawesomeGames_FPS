using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class EnemyParams : ScriptableObject
{
    public GameObject bloodSplash;
    public GameObject bullet;
    [Range(1, 1000)] public int HP;
    [Range(0.1f , 5)] public float attackSpeed;
    [Range(0.5f, 10.0f)] public float movementSpeed;
    [Range(1, 20)] public int Acceleration;
    [Range(1, 20)] public int rangedDamage;
    [Range(1, 20)] public int meleeDamage;
    [Range(1, 20)] public int effectDamage;
    [Range(0, 100)] public int lineOfSightRange;
    [Range(0, 100)] public int attackRange;
    [Range(0.1f, 20)] public float effectDuration;
    [Range(0.1f, 20)] public float bulletSpeed;
    [Range(2, 10)] public int destroyTime;

    public enum EnemyType { Melee, Ranged, Combination, Stationary, Exploding };
    public enum DamageType { Regular, Poisoned, Burning, Freezing, Slowed, Confused };
    public enum DetectionType { LineOfSight, Wave };

    public DamageType damageType;
    public EnemyType enemyType;
    public DetectionType detectionType;

    public bool flying = false;

}
