using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "GranadeData", menuName = "GranadeData", order = 5)]
public class GranadeData : ScriptableObject
{
    public int Count;
    public float Damage;
    public Sprite sprite;
    public float throwSpeed;
}
