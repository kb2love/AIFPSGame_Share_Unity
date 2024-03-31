using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 4)]
public class PlayerData : ScriptableObject
{
    public int maxHp;
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public GameObject bullet;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip rfFireClip;
    public AudioClip rfReloadClip;
    public AudioClip sgFireClip;
    public AudioClip sgReloadClip;
}
