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
    [SerializeField] private List<GameObject> collidersSet = new List<GameObject>();
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
            collidersSet.Add(other.gameObject);
            StartCoroutine(GetItemPlayer());
        }
    }
    
    IEnumerator GetItemPlayer()
    {
        while (isContact)
        {
            yield return new WaitForSeconds(0.002f);
            if (collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.HEAL)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.HEAL);
                }
            }
            else if (collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLE);
                }
            }
            else if (collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUN);
                }
            }
            else if (collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLEBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLEBULLET);
                }
            }
            else if (collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUNBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUNBULLET);
                }
            }
        }
    }

    private void GetItemF(GameObject other)
    {
        other.SetActive(false);
        getItemPanel.SetActive(false);
        collidersSet.RemoveAt(0);
        isContact = false;
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
