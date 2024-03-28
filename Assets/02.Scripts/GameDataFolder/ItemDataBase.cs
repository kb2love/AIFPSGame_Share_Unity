using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemDataBase;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase itemDataBase;
    public ItemType itemType;
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
    }
    private void Awake()
    {
        itemDataBase = this;
    }
}

