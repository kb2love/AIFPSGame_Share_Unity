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
    public float rifleBulletCount;
    public float rifleBulletDamage;
    public float shotgunBulletCount;
    public float shotgunBulletDamage;
    public float itemHealValue;
    public enum ItemType
    {
        HEAL = 1,
        RIFLE,
        SHOTGUN,
        BULLET,
        ARMOR,
        GRENADE
    }
    [System.Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public string itemName;
        public Sprite itemImage;
        public float itemValue = 30;
        public List<ItemData> itemDataList = new List<ItemData>();
    }

    private void Awake()
    {
        itemDataBase = this;
        rifleBulletCount = 30;
        rifleBulletDamage = 25;
        shotgunBulletCount = 10;
        shotgunBulletDamage = 50;
        itemHealValue = 20;
    }
}
