using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HEAL = 1,
    GUN,
    BULLET,
    PARTS,
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
    public bool ItemUse()
    {
        return false;
    }
}

