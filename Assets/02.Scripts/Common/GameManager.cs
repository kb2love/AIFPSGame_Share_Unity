
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

    [SerializeField] private Sprite pistolBulletBox;
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
        itemEmptyObject = GameObject.Find("Item_EmptyGroup").GetComponentsInChildren<Transform>(includeInactive: false);
        pistolBulletBox = Resources.Load<Sprite>("Image/Inventory/PistolBulletBox");
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
    }
    public void AddItem(ItemType itemType)
    {
        switch(itemType)
        {
            case ItemType.HEAL:

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

    }
    private void CaseBullet()
    {
        Debug.Log("Bullet");
        fireCtrl.bulletValue += (int)ItemDataBase.itemDataBase.itemValue;
        fireCtrl.bulletText.text = fireCtrl.bulletCount.ToString() + " / " + fireCtrl.bulletValue.ToString();
        #region 배열을 쓰지않고 하나만 하는방법
        /*itemEmptyObject.text = fireCtrl.bulletValue.ToString();
        itemEmptyObject.enabled = true;
        itemEmptyImage.sprite = pistolBulletBox;
         itemEmptyRect.transform.SetParent(imageDrop[1]);
         itemEmptyImage.enabled = true;*/
        #endregion
        for(int i = 0;i < imageDropList.Count;i++)
        {
            if (imageDropList[i].childCount > 0) continue;
            for(int j = 0; j < itemEmptyRectList.Count;j++)
            {
                if (imageDropList[i].childCount == 0)
                {
                    itemEmptyRectList[j].SetParent(imageDropList[i]);
                    break;
                }

            }
            if (imageDropList[i].childCount > 0) return;
        }
    }
    private void CaseGun()
    {

    }
}
