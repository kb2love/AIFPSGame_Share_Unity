using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private RectTransform[] equipmentTr;
    [SerializeField] private List<RectTransform> equipmentTrList = new List<RectTransform>();
    private void Start()
    {
        equipmentTr = GameObject.Find("Inventory_equipment").GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < equipmentTr.Length; i++)
        {
            equipmentTrList.Add(equipmentTr[i]);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        for(int i = 0; i < equipmentTrList.Count; i++)
        {
            if ((Drag.DraggingItem.gameObject.GetComponent<ItemDataBase>().itemType != ItemDataBase.ItemType.SHOTGUN |
            Drag.DraggingItem.gameObject.GetComponent<ItemDataBase>().itemType != ItemDataBase.ItemType.RIFLE) && Drag.DraggingItem.transform.parent == equipmentTrList[i])
            {
                Debug.Log("¤·");
                Drag.DraggingItem.transform.position = Drag.DraggingItem.GetComponent<Drag>().origPos;
            }
            else if (transform.childCount == 0)
            {
                Debug.Log("¤±");
                Drag.DraggingItem.transform.SetParent(transform, false);
            }
        }
    }

}
