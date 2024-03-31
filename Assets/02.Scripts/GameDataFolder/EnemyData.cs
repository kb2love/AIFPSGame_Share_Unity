using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="EnemyData", menuName ="EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    public int e_Hp;
    public int e_Damage;
    public float e_MoveSpeed;
    public GameObject e_bullet;
    public GameObject hitParticle;
    public GameObject spawnEnemy;
    public AudioClip shotClip;
    public AudioClip walkClip;
}
