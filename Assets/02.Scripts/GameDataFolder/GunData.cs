using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GunData", menuName ="GunData", order = 3)]
public class GunData : ScriptableObject
{
    public int Sg_Count;
    public int Rf_Count;
    public float g_damage;
    public Sprite sgB_Sprite;
    public Sprite rfB_Sprite;
    public Sprite sg_Sprite;
    public Sprite rf_Sprite;
}
