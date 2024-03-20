using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;
public class GetItem : MonoBehaviour
{
    private string itemTag = "Item";
    private GameObject getItemPanel;
    private RectTransform itemEmpty;
    private Image itemEmptyImage;
    private RectTransform[] imageDrop;
    [SerializeField] private Sprite pistolBulletBox;
    void Awake()
    {
        itemEmpty = GameObject.Find("Item_Empty").GetComponent<RectTransform>();
        imageDrop = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        itemEmptyImage = itemEmpty.GetComponent<Image>();
        pistolBulletBox = Resources.Load<Sprite>("Image/Inventory/PistolBulletBox");
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
    }
    void OnEnable()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(true);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag(itemTag))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.SetActive(false);
                getItemPanel.SetActive(false);
                itemEmptyImage.sprite = pistolBulletBox;
                itemEmpty.transform.SetParent(imageDrop[1]);
                itemEmptyImage.enabled = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(false);
        }
    }
}
