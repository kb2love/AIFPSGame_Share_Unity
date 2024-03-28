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
    [SerializeField] private List<GameObject> coliderContain = new List<GameObject>();
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
            collidersSet.Add(other.gameObject);
            if(collidersSet.Count > 1)
            {
                for (int i = 1; i < collidersSet.Count; i++)
                {
                    coliderContain.Add(collidersSet[i]);
                    collidersSet.Remove(collidersSet[i]);
                }
                isContact = true;
            }
            else if(collidersSet.Count == 1)
            {
                isContact = true;
            }
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
                    GetItemF();
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.HEAL);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF();
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLE);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF();
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUN);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLEBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF();
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.RIFLEBULLET);
                }
            }
            else if (collidersSet.Count > 0 && collidersSet[0].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUNBULLET)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetItemF();
                    GameManager.Instance.AddItem(ItemDataBase.ItemType.SHOTGUNBULLET);
                }
            }
        }
    }

    private void GetItemF()
    {
        collidersSet[0].gameObject.SetActive(false);
        collidersSet.RemoveAt(0);
        if(collidersSet.Count == 0 && coliderContain.Count == 0)
        {
            getItemPanel.SetActive(false);
            isContact = false;
        }
        else if(coliderContain.Count > 0)
        {
            for(int i = 0;  i < coliderContain.Count; i++)
            {
                collidersSet.Add(coliderContain[i]);
                coliderContain.Remove(coliderContain[i]);
            }
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
