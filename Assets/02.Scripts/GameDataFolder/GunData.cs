using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "GunData", menuName ="GunData", order = 3)]
public class GunData : ScriptableObject
{
    public int Sg_Count;
    public int Rf_Count;
    public float g_damage;
    public Sprite rfB_Sprite;
    public Sprite rf_Sprite;
    public Sprite rf_UISprite;
    public Sprite sgB_Sprite;
    public Sprite sg_Sprite;
    public Sprite sg_UISprite;
    public GameObject rifle;
    public GameObject rifleBullet;
    public AudioClip rifleClip;
    public AudioClip rifleReloadClip;
    public GameObject shotgun;
    public GameObject shotgunBullet;
    public AudioClip shotgunClip;
    public AudioClip shotgunReloadClip;
}
