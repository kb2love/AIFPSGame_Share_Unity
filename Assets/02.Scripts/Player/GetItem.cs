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
    [SerializeField] private List<GameObject> collidersSet = new List<GameObject>();
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
            collidersSet.Clear();
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
            if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.HEAL)
            { 
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.HEAL);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    Debug.Log("왜두번될까");
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLE);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUN);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLEBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF(collidersSet[0]);
                    Debug.Log("왜두번될까");
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLEBULLET);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUNBULLET)
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
