
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    [SerializeField] private Transform[] itemEmptyObject;
    [SerializeField] private List<Transform> itemEmptyObjectList = new List<Transform>();
    [SerializeField] private List<Text> itemEmptyText = new List<Text>();

    [SerializeField] private RectTransform[] itemEmptyRect;
    [SerializeField] private List<RectTransform> itemEmptyRectList = new List<RectTransform>();

    private RectTransform[] imageDrop;
    [SerializeField] private List<RectTransform> imageDropList = new List<RectTransform>();

    [SerializeField] private Image[] itemEmptyImage;
    [SerializeField] private List<Image> itemEmptyImageList = new List<Image>();

    [SerializeField] private Sprite bulletBoxSprite;

    private FireCtrl fireCtrl;
    private int itemEmptyIdx;
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
        itemEmptyObject = GameObject.Find("Item_EmptyGroup").GetComponentsInChildren<Transform>(includeInactive: false);
        bulletBoxSprite = Resources.Load<Sprite>("Image/Inventory/PistolBulletBox");
        itemEmptyImage = GameObject.Find("Item_EmptyGroup").transform.GetComponentsInChildren<Image>(includeInactive: false);
        itemEmptyRect = GameObject.Find("Item_EmptyGroup").transform.GetComponentsInChildren<RectTransform>(includeInactive: false);
        imageDrop = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        for(int i = 0; i < imageDrop.Length; i++)
        {
            imageDropList.Add(imageDrop[i]);
        }
        for(int i = 0; i <itemEmptyImage.Length; i++)
        {
            itemEmptyImageList.Add(itemEmptyImage[i]);
        }
        for(int i = 0;i < itemEmptyRect.Length; i++)
        {
            itemEmptyRectList.Add(itemEmptyRect[i]);
        }
        for(int i = 0; i <itemEmptyObject.Length; i++)
        {
            itemEmptyObjectList.Add(itemEmptyObject[i]);
        }
        /*for (int i = 0; i < itemEmptyRect.Length; i++)
        {
            itemEmptyObject[i] = itemEmptyRect[i].transform.GetChild(0).gameObject;
        }*/
        itemEmptyObjectList.RemoveAt(0);
        imageDropList.RemoveAt(0);
        itemEmptyRectList.RemoveAt(0);
        for(int i = 0; i < itemEmptyObjectList.Count; i++)
        {
            Transform childTransform = itemEmptyObjectList[i];
            Text textCompenet = childTransform.transform.GetChild(0).GetComponent<Text>();
            itemEmptyText.Add(textCompenet);
        }
        itemEmptyIdx = 0;
    }
    public void AddItem(ItemType itemType)
    {
        switch(itemType)
        {
            case ItemType.HEAL:
                CaseHeal();
                Debug.Log("Heal");
                break;
            case ItemType.GUN:
                CaseGun();
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

    private void CaseHeal()
    {
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (imageDropList[i].childCount > 0)
            {
                continue;
            }
            for (int j = 0; j < itemEmptyRectList.Count; j++)
            {
                if (imageDropList[i].childCount == 0)
                {
                    itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
                    itemEmptyIdx++;
                }

            }
            break;
        }
    }
    private void CaseBullet()
    {
        Debug.Log("Bullet");
        fireCtrl.bulletValue += (int)ItemDataBase.itemDataBase.itemBulletCount;
        fireCtrl.bulletText.text = fireCtrl.bulletCount.ToString() + " / " + fireCtrl.bulletValue.ToString();
        
        for(int i = 0;i < imageDropList.Count;i++)  //인벤토리에 불렛이 추가된다
        {
            if (imageDropList[i].childCount > 0) continue;
            for (int j = 0; j < itemEmptyRectList.Count;j++)
            {
                if (imageDropList[i].childCount == 0)
                {
                    itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
                    itemEmptyImageList[itemEmptyIdx].sprite = bulletBoxSprite;
                    itemEmptyImageList[itemEmptyIdx].enabled = true;
                    itemEmptyText[itemEmptyIdx].text = fireCtrl.bulletValue.ToString();
                    itemEmptyText[itemEmptyIdx].gameObject.SetActive(true);
                    itemEmptyIdx++;
                }

            }
            break;
        }
    }
    private void CaseGun()
    {

    }
}
