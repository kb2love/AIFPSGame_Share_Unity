using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    private string itemTag = "Item";
    private GameObject getItemPanel;
    
    void Awake()
    {
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
                if(other.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.BULLET)
                {
                    Debug.Log("get");
                    GameManager.Instance.AddItem(ItemType.GUN);
                }
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
