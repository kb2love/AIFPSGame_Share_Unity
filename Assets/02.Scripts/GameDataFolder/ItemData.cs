using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemType", menuName ="ItemType", order = 0)]
public class ItemData : ScriptableObject
{
    public static ItemData itemData;
    public ItemType itemType;
    public enum ItemType
    {
        Meadicine = 1,
        RIFLE,
        SHOTGUN,
        RIFLEBULLET,
        SHOTGUNBULLET,
        GRENADE
    }
}

