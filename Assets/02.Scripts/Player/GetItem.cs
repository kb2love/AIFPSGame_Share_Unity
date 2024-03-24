using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    private string itemTag = "Item";
    private GameObject getItemPanel;
    public bool isContact;
    
    void Awake()
    {
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
        isContact = false;
    }
    void Start()
    {
       
    }
    private 
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(true);
            isContact = true;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.HEAL)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetItemF(other);
                GameManager.Instance.AddItem(ItemDataBase.ItemType.HEAL);
            }
        }
        else if (other.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.BULLET)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetItemF(other);
                GameManager.Instance.AddItem(ItemDataBase.ItemType.BULLET);
            }
        }
        else if (other.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetItemF(other);
                GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUN);
            }
        }
    }

    private void GetItemF(Collider other)
    {
        other.gameObject.SetActive(false);
        getItemPanel.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(false);
            isContact = false;
        }
    }
}
