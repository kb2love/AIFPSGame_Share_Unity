using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase itemDataBase;
    public ItemType itemType;
    public int itemCount;
    public string itemName;
    public Sprite itemImage;
    public float itemBulletCount;
    public float itemBulletDamage;
    public float itemHealValue;
    public enum ItemType
    {
        HEAL = 1,
        GUN,
        BULLET,
        PARTS,
        ARMOR,
        GRENADE
    }
    private void Awake()
    {
        itemDataBase = this;
        itemBulletCount = 30;
        itemBulletDamage = 25;
        itemHealValue = 20;
    }
}
