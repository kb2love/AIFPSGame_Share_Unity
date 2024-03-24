
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    private Transform[] itemEmptyObject;
    private List<Transform> itemEmptyObjectList = new List<Transform>();
    public List<Text> itemEmptyText = new List<Text>();

    private RectTransform[] itemEmptyRect;
    public List<RectTransform> itemEmptyRectList = new List<RectTransform>();

    private RectTransform[] imageDrop;
    private List<RectTransform> imageDropList = new List<RectTransform>();

    private Image[] itemEmptyImage;
    public List<Image> itemEmptyImageList = new List<Image>();

    private Sprite bulletBoxSprite;
    private Sprite shotGunSprite;

    private FireCtrl fireCtrl;
    public int itemEmptyIdx;
    public int bulletIdx;
    [SerializeField] public bool isBullet;
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
        shotGunSprite = Resources.Load<Sprite>("Image/Inventory/ShotGunImage");
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
        bulletIdx = 0;
        isBullet = false;
    }
    public void AddItem(ItemDataBase.ItemType itemType)
    {
        switch(itemType)
        {
            case ItemDataBase.ItemType.HEAL:
                CaseHeal();
                Debug.Log("Heal");
                break;
            case ItemDataBase.ItemType.RIFLE:
                break;
            case ItemDataBase.ItemType.SHOTGUN:
                CaseShotGun();
                break;
            case ItemDataBase.ItemType.BULLET:
                Debug.Log("Bullet");
                CaseBullet();
                break;
            case ItemDataBase.ItemType.ARMOR:

                break;
            case ItemDataBase.ItemType.GRENADE:

                break;

        }
    }

    private void CaseHeal()
    {
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (imageDropList[i].childCount > 0) continue;
            /*for (int j = 0; j < itemEmptyRectList.Count; j++)
            {*/
                /*if (imageDropList[i].childCount == 0)
                {*/
                    itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
                    itemEmptyIdx++;
            break;
        }
    }
    private void CaseBullet()
    {
        Debug.Log("Bullet");
        fireCtrl.rilfeBulletValue += (int)ItemDataBase.itemDataBase.rifleBulletCount;
        fireCtrl.rifleBulletText.text = fireCtrl.rifleBulletCount.ToString() + " / " + fireCtrl.rilfeBulletValue.ToString();
        
        if(!isBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                        itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
                        itemEmptyImageList[itemEmptyIdx].sprite = bulletBoxSprite;
                        itemEmptyImageList[itemEmptyIdx].enabled = true;
                        itemEmptyText[itemEmptyIdx].text = fireCtrl.rilfeBulletValue.ToString();
                        itemEmptyText[itemEmptyIdx].gameObject.SetActive(true);
                        bulletIdx = itemEmptyIdx;
                        itemEmptyIdx++;
                        isBullet = true;
                break;
            }
        }
        else if(isBullet)
        {
            itemEmptyText[bulletIdx].text = fireCtrl.rilfeBulletValue.ToString();
        }
    }
    private void CaseShotGun()
    {
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (imageDropList[i].childCount > 0) continue;
            itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
            itemEmptyImageList[itemEmptyIdx].sprite = shotGunSprite;
            itemEmptyImageList[itemEmptyIdx].enabled=true;
            itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = ItemDataBase.ItemType.SHOTGUN;
            itemEmptyIdx++;
            
            break;
        }
    }
}
