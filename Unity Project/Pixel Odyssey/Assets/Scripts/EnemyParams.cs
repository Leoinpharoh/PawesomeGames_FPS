using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu]
public class EnemyParams : ScriptableObject
{

    [Header("General Parameters")]
    public GameObject bloodSplash;
    [Range(1, 1000)] public int HP;
    [Range(0.5f, 10.0f)] public float movementSpeed;
    [Range(1, 20)] public int Acceleration;
    [Range(2, 10)] public int animSpeedTrans;
    public enum EnemyType { Melee, Ranged, Combination, Stationary, Exploding };
    public bool flying = false;
    public EnemyType enemyType;



    [Header("Damage")]
    public GameObject bullet;
    [Range(0.1f, 5)] public float attackSpeed;
    [Range(0.1f, 3)] public float rangedAttackLead;
    [Range(0, 100)] public int meleeRange;
    [Range(1, 20)] public int meleeDamage;
    [Range(0, 100)] public int rangedRange;
    [Range(1, 20)] public int rangedDamage;
    [Range(0.1f, 20)] public float bulletSpeed;
    [Range(2, 10)] public int destroyTime;
    [Range(0, 20)] public int effectDamage;
    [Range(0.1f, 20)] public float effectDuration;
    public enum DamageType { Regular, Poisoned, Burning, Freezing, Slowed, Confused };
    public DamageType damageType;



    [Header("Roaming Parameters")]
    public bool roaming = false;
    [Range(0, 100)] public int roamDist;
    [Range(0, 100)] public int roamTimer;


    [Header("Loot Parameters")]
    public bool lootPinata = false;
    [Range(0, 100)] public int lootChance;
    public GameObject[] loot;

    [Header("Detection Parameters")]
    [Range(0, 100)] public int lineOfSightRange;
    [Range(0, 180)] public float viewAngle;
    public enum DetectionType { LineOfSight, Wave };
    public DetectionType detectionType;

    [Header("Audio Files")]
    public AudioClip[] attackSound;
    public AudioClip[] deathSound;
    public AudioClip[] damagedSound;
    public AudioClip[] idleSound;
    public AudioClip[] walkingSound;

}
