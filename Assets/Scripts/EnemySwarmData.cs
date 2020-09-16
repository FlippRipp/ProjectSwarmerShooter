using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FG/EnemySwarmData", fileName = "EnemySwarmData")]
public class EnemySwarmData : ScriptableObject
{
    public float maxSpeed = 8f;
    public float minSpeed = 3f;
    public float accel = 1f;
    public float lockAngle = 10;
    public float preferedDistToTarget = 2f;
    public float minRotationSpeed = 90f;
    public float maxRotationSpeed = 140f;
    public float minReflex = 0.1f;
    public float maxReflex = 0.3f;
    public float minTimeBetweenJumps = 1;
    public float maxTimeBetweenJumps = 2;
    public float jumpVelocity = 5;
    public float maxWaver = 5f;
    public float minWaver = -5f;
    [Header("Damage")]
    public float damage = 1;
    public float damageTime = 0.5f;
    [Header("DeSpawn Time")] 
    public float bodyDeSpawnTime = 10f;
    public float unseenDeSpawnTime = 30f;
}
