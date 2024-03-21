using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    private string itemTag = "Item";
    private GameObject getItemPanel;
    private bool isContact;
    
    void Awake()
    {
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
        isContact = false;
    }
    void OnEnable()
    {
        
    }
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
                other.gameObject.SetActive(false);
                getItemPanel.SetActive(false);
                Debug.Log("get");
                GameManager.Instance.AddItem(ItemType.HEAL);
            }
        }
        else if (other.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.BULLET)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.SetActive(false);
                getItemPanel.SetActive(false);
                Debug.Log("get");
                GameManager.Instance.AddItem(ItemType.BULLET);
            }
        }
        else if (other.gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.GUN)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.gameObject.SetActive(false);
                getItemPanel.SetActive(false);
                Debug.Log("get");
                GameManager.Instance.AddItem(ItemType.GUN);
            }
        }
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
