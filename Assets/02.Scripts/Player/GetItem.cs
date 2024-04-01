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
    private Animator animator;
    [SerializeField] private int setCount;
    private readonly int aniGetItem = Animator.StringToHash("GetItemTrigger");
    [SerializeField] private bool isInputF;
    [SerializeField] private List<GameObject> collidersSet = new List<GameObject>();
    void Awake()
    {
        getItemPanel = GameObject.Find("Canvas_ui").transform.GetChild(1).gameObject;
        animator = transform.GetChild(0).GetComponent<Animator>();
        isContact = false;
        isInputF = false;
        setCount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(true);
            collidersSet.Add(other.gameObject);
            isContact = true;
            StartCoroutine(GetItemPlayer());
            setCount = 0;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isInputF = true;
        }
    }
    /*IEnumerator GetItemPlayer()
    {
        while (isContact)
        {
            yield return new WaitForSeconds(0.2f);
            
            if (isInputF && collidersSet.Count > 0 && collidersSet[setCount].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.HEAL)
            {
                GetItemF(ItemDataBase.ItemType.HEAL);
            }
            
            else if (isInputF && collidersSet.Count > 0 && collidersSet[setCount].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLE)
            {
                GetItemF(ItemDataBase.ItemType.RIFLE);
            }
            else if (isInputF && collidersSet.Count > 0 && collidersSet[setCount].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUN)
            {
                GetItemF(ItemDataBase.ItemType.SHOTGUN);
            }
            else if (isInputF && collidersSet.Count > 0 && collidersSet[setCount].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.RIFLEBULLET)
            {
                GetItemF(ItemDataBase.ItemType.RIFLEBULLET);
            }
            else if (isInputF && collidersSet.Count > 0 && collidersSet[setCount].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.SHOTGUNBULLET)
            {
                GetItemF(ItemDataBase.ItemType.SHOTGUNBULLET);
            }
            else if (isInputF && collidersSet.Count > 0 && collidersSet[setCount].GetComponent<ItemDataBase>().itemType == ItemDataBase.ItemType.GRENADE)
            {
                GetItemF(ItemDataBase.ItemType.GRENADE);
            }
        }
    }*/
    IEnumerator GetItemPlayer()
    {
        while (isContact)
        {
            yield return new WaitForSeconds(0.2f);

            if (isInputF && collidersSet.Count > 0)
            {
                var itemType = collidersSet[setCount].GetComponent<ItemDataBase>().itemType;
                GetItemF(itemType);
            }
        }
    }
    /*private void GetItemF(ItemDataBase.ItemType itemType)
    {
        if (isInputF)
        {
            collidersSet[setCount].gameObject.SetActive(false);
            setCount++;
            colliderContain.RemoveAt(0);
            isInputF = false;
            Debug.Log("�ι��Ŵµ�?");
            if (colliderContain.Count == 0)
            {
                getItemPanel.SetActive(false);
                collidersSet.Clear();
                setCount = 0;
                isContact = false;
            }
            animator.SetTrigger(aniGetItem);
            ItemManager.Instance.AddItem(itemType);
        }
    }*/
    private void GetItemF(ItemDataBase.ItemType itemType)
    {
        if (isInputF)
        {
            collidersSet[setCount].gameObject.SetActive(false);
            setCount++;
            isInputF = false;

            if (setCount >= collidersSet.Count)
            {
                getItemPanel.SetActive(false);
                collidersSet.Clear();
                setCount = 0;
                isContact = false;
            }

            animator.SetTrigger(aniGetItem);
            ItemManager.Instance.AddItem(itemType);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(itemTag))
        {
            getItemPanel.SetActive(false);
            isContact = false;
            collidersSet.Clear();
        }
    }
}
