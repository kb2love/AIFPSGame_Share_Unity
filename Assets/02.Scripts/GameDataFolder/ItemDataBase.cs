using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemDataBase;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase itemDataBase;
    public ItemType itemType;
    public int itemCount;
    public string itemName;
    public Sprite itemImage;
    public int rifleBulletCount;
    public float BulletDamage;
    public int shotgunBulletCount;
    public int itemHealCount;
    public float itemHealValue;
    public enum ItemType
    {
        HEAL = 1,
        RIFLE,
        SHOTGUN,
        RIFLEBULLET,
        SHOTGUNBULLET,
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
        rifleBulletCount = 0;
        BulletDamage = 0;
        shotgunBulletCount = 0;
        itemHealCount = 0;
        itemHealValue = 20;
    }
}

