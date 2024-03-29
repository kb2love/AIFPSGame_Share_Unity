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
    [SerializeField] private bool isInputF;
    [SerializeField] private List<GameObject> collidersSet = new List<GameObject>();
    [SerializeField] private List<GameObject> coliderContain = new List<GameObject>();
    void Awake()
    {
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
        isContact = false;
        isInputF = false;
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
            collidersSet.Add(other.gameObject);
            isContact = true;
            StartCoroutine(GetItemPlayer());
        }
    }
    IEnumerator GetItemPlayer()
    {
        while (isContact)
        {
            yield return new WaitForSeconds(0.002f);
            if(Input.GetKeyDown(KeyCode.F))
            {
                isInputF = true;
            }
            if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.HEAL)
            {
                GetItemF(ItemDataBase.ItemType.HEAL);
            }
            
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
            {
                GetItemF(ItemDataBase.ItemType.RIFLE);
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
            {
                GetItemF(ItemDataBase.ItemType.SHOTGUN);
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLEBULLET)
            {
                GetItemF(ItemDataBase.ItemType.RIFLEBULLET);
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUNBULLET)
            {
                GetItemF(ItemDataBase.ItemType.SHOTGUNBULLET);
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.GRENADE)
            {
                GetItemF(ItemDataBase.ItemType.GRENADE);
            }
        }
    }

    private void GetItemF(ItemDataBase.ItemType itemType)
    {
        if (isInputF)
        {
            collidersSet[0].gameObject.SetActive(false);
            collidersSet.RemoveAt(0);
            isInputF = false;
            Debug.Log("µÎ¹øµÅ´Âµí?");
            if (collidersSet.Count == 0)
            {
                getItemPanel.SetActive(false);
                isContact = false;
            }
            GameManager.Instance.AddItem(itemType);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(false);
            isContact = false;
            collidersSet.Clear();
            coliderContain.Clear();
        }
    }
}
