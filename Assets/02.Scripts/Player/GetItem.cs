using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    private string itemTag = "Item";
    private GameObject getItemPanel;
    private PlayerDamage playerDamage;
    public bool isContact;
    private List<Collider> colliders = new List<Collider>();
    void Awake()
    {
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
        playerDamage = GetComponent<PlayerDamage>();
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
            colliders.Add(other);
            StartCoroutine(GetItemPlayer());
        }
    }
    
    IEnumerator GetItemPlayer()
    {
        while (isContact)
        {
            yield return new WaitForSeconds(0.002f);
            if (colliders[0].gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.HEAL)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(colliders[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.HEAL);
                }
            }
            else if (colliders[0].gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(colliders[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLE);
                }
            }
            else if (colliders[0].gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(colliders[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUN);
                }
            }
            else if (colliders[0].gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLEBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(colliders[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLEBULLET);
                }
            }
            else if (colliders[0].gameObject.GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUNBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(colliders[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUNBULLET);
                }
            }
        }
    }

    private void GetItemF(Collider other)
    {
        other.gameObject.SetActive(false);
        getItemPanel.SetActive(false);
        isContact = false;
        colliders.RemoveAt(0);
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
