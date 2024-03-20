using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
