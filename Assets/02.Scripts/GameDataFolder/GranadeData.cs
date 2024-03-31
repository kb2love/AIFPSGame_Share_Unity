using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "GranadeData", menuName = "GranadeData", order = 5)]
public class GranadeData : ScriptableObject
{
    public int Count;
    public float Damage;
    public float throwSpeed;
    public Sprite sprite;
    public GameObject spawnGranade;
    public GameObject throwGranade;
}
