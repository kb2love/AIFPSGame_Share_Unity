using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform inventoryTr;
    private RectTransform itemTr;
    [SerializeField] private RectTransform rightHand;
    [SerializeField] public Vector2 origPos;
    private CanvasGroup canvasGroup;
    public static GameObject DraggingItem = null;
    private FireCtrl fireCtrl;
   
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemTr = GetComponent<RectTransform>();
        inventoryTr = GameObject.Find("Image-Inventory").GetComponent<RectTransform>();
        fireCtrl = FindObjectOfType<FireCtrl>();
        rightHand = GameObject.Find("Inventory_equipment").transform.GetChild(0).GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        DraggingItem = this.transform.gameObject;
        itemTr.SetParent(inventoryTr);
        canvasGroup.blocksRaycasts = false;
        origPos = itemTr.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggingItem = null;
        canvasGroup.blocksRaycasts = true;

        
        if(itemTr.parent == rightHand && this.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
        {
            fireCtrl.isRifle = true;
            fireCtrl.isShotGun = false;
            fireCtrl.ChangeRifle();
        }
        else if(itemTr.parent == rightHand && this.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
        {
            fireCtrl.isShotGun = true;
            fireCtrl.isRifle = false;
            fireCtrl.ChangeShotGun();
        }
        else if(itemTr.parent != rightHand && this.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE )
        {
            fireCtrl.isRifle = false;
        }
        else if(itemTr.parent != rightHand && this.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
        {
            fireCtrl.isShotGun = false;
        }
        else if (itemTr.parent == inventoryTr)
        {
            itemTr.position = origPos;
        }
    }
}
