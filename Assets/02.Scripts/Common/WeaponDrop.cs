using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponDrop : MonoBehaviour, IDropHandler
{
    private FireCtrl fireCtrl;
    void Start()
    {
        fireCtrl = FindObjectOfType<FireCtrl>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 && (Drag.DraggingItem.GetComponent<ItemInfo>().itemType == ItemData.ItemType.RIFLE))
        {
            Drag.DraggingItem.transform.SetParent(transform, false);
            fireCtrl.isRifle = true;
            fireCtrl.isShotGun = false;
            fireCtrl.isGranade = false;
            fireCtrl.ChangeRifle();
        }
        else if (transform.childCount == 0 && Drag.DraggingItem.GetComponent<ItemInfo>().itemType == ItemData.ItemType.SHOTGUN)
        {
            fireCtrl.isShotGun = true;
            fireCtrl.isRifle = false;
            fireCtrl.isGranade = false;
            fireCtrl.ChangeShotGun();
        }
    }
}
