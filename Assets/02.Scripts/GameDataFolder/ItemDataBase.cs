using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase itemDataBase;
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public float itemValue = 0;
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
    }
    
    public bool ItemUse()
    {
        return false;
    }
}
