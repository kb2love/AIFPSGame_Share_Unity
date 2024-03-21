
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    private RectTransform[] itemListRect;
    public Text itemEmptText;
    private RectTransform[] imageDrop;
    private List<RectTransform> itemList = new List<RectTransform>();
    [SerializeField] private Sprite pistolBulletBox;
    private Image itemEmptyImage;
    private RectTransform itemEmpty;
    private FireCtrl fireCtrl;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        fireCtrl = GameObject.FindWithTag("Player").GetComponent<FireCtrl>();
        itemEmpty = GameObject.Find("Item_Empty").GetComponent<RectTransform>();
        itemEmptText = itemEmpty.transform.GetChild(0).GetComponent<Text>();
        imageDrop = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        pistolBulletBox = Resources.Load<Sprite>("Image/Inventory/PistolBulletBox");
        itemEmptyImage = itemEmpty.GetComponent<Image>();
        itemListRect = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        for(int i = 0; i < itemListRect.Length; i++)
        {
            itemList.Add(itemListRect[i]);
        }
        itemList.RemoveAt(0);
        
    }
    public void AddItem(ItemType itemType)
    {
        switch(itemType)
        {
            case ItemType.HEAL:

                break;
            case ItemType.GUN:

                break;
            case ItemType.BULLET:
                Debug.Log("Bullet");
                CaseBullet();
                break;
            case ItemType.PARTS:

                break;
            case ItemType.ARMOR:

                break;
            case ItemType.GRENADE:

                break;

        }
    }

    private void CaseBullet()
    {
        fireCtrl.bulletValue += (int)ItemDataBase.itemDataBase.itemValue;
        itemEmptText.text = fireCtrl.bulletValue.ToString();
        itemEmptText.enabled = true;
        fireCtrl.bulletText.text = fireCtrl.bulletCount.ToString() + " / " + fireCtrl.bulletValue.ToString();
        itemEmptyImage.sprite = pistolBulletBox;
        itemEmpty.transform.SetParent(imageDrop[1]);
        itemEmptyImage.enabled = true;
    }
    private void CaseGun()
    {

    }
}
