using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
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
            fireCtrl.isRifle = false;
        }
        else if(transform.childCount == 0 && (Drag.DraggingItem.GetComponent<ItemInfo>().itemType == ItemData.ItemType.SHOTGUN))
        {
            fireCtrl.isShotGun = false;
        }
    }

}
