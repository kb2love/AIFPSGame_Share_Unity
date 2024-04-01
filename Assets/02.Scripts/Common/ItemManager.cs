
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance = null;

    [SerializeField] MadicinData madicinData;
    [SerializeField] GunData gunData;
    [SerializeField] PlayerData playerData;
    [SerializeField] GranadeData granadeData;
    public List<Text> itemEmptyText = new List<Text>();

    private RectTransform[] itemEmptyRect;
    public List<RectTransform> itemEmptyRectList = new List<RectTransform>();

    private RectTransform[] imageDrop;
    private List<RectTransform> imageDropList = new List<RectTransform>();

    private PlayerDamage playerDamage;

    private FireCtrl fireCtrl;
    public int itemEmptyIdx;
    public int rifleIdx;
    public int shotgunIdx;
    public int rifleBulletIdx;
    public int shotgunBulletIdx;
    public int granadeIdx;
    private int healIdx;
    private bool isHeal;
    public bool isRifleBullet;
    public bool isShotGunBullet;
    public bool isGranadeGet;
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
        fireCtrl = FindObjectOfType<FireCtrl>();
        itemEmptyRect = GameObject.Find("Item_EmptyGroup").transform.GetComponentsInChildren<RectTransform>(includeInactive: false);
        imageDrop = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        playerDamage = FindObjectOfType<PlayerDamage>();   
        for(int i = 1; i < imageDrop.Length; i++)
        {
            imageDropList.Add(imageDrop[i]);
        }
        for(int i = 1;i < itemEmptyRect.Length; i++)
        {
            itemEmptyRectList.Add(itemEmptyRect[i]);
        }
        
        for(int i = 0; i < itemEmptyRectList.Count; i++)
        {
            Transform childTransform = itemEmptyRectList[i];
            Text textCompenet = childTransform.transform.GetChild(0).GetComponent<Text>();
            itemEmptyText.Add(textCompenet);
        }
        itemEmptyIdx = 0;
        rifleBulletIdx = 0;
        healIdx = 0;
        rifleIdx = 0;
        shotgunIdx = 0;
        isRifleBullet = false;
        isShotGunBullet = false;
        isHeal = false;
        gunData.Rf_Count = 0;
        gunData.Sg_Count = 0;
        madicinData.m_Count = 0;
        granadeData.Count = 0;
    }
    public void AddItem(ItemDataBase.ItemType itemType)
    {
        switch(itemType)
        {
            case ItemDataBase.ItemType.HEAL:
                CaseHeal();
                break;
            case ItemDataBase.ItemType.RIFLE:
                AddGun(gunData.rf_Sprite, ItemDataBase.ItemType.RIFLE, rifleIdx);
                fireCtrl.getRifle = true;
                break;
            case ItemDataBase.ItemType.SHOTGUN:
                AddGun(gunData.sg_Sprite, ItemDataBase.ItemType.SHOTGUN, shotgunIdx);
                fireCtrl.getShotGun = true;
                break;
            case ItemDataBase.ItemType.RIFLEBULLET:
                CaseRifleBullet();
                break;
            case ItemDataBase.ItemType.SHOTGUNBULLET:
                CaseShotGunBullet();
                break;
            case ItemDataBase.ItemType.GRENADE:
                CaseGranade();
                break;

        }
    }
    private void CaseHeal()
    {

        madicinData.m_Count++;
        itemEmptyText[itemEmptyIdx].text = madicinData.m_Count.ToString();
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (!isHeal)
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].GetComponent<Button>().onClick.AddListener(HealItem);
                healIdx = itemEmptyIdx;
                AddItem(i, madicinData.m_Sprite,madicinData.m_Count);
                isHeal = true;
                break;
            }
            else
            {
                itemEmptyText[healIdx].text = madicinData.m_Count.ToString();
            }
        }
    }
    private void CaseRifleBullet()
    {
        gunData.Rf_Count += 30;
        if(fireCtrl.isRifle)
            fireCtrl.bulletText.text = fireCtrl.rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();
        
        if(!isRifleBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = ItemDataBase.ItemType.RIFLE;
                rifleBulletIdx = itemEmptyIdx;
                AddItem(i, gunData.rfB_Sprite,gunData.Rf_Count);
                isRifleBullet = true;
                break;
            }
        }
        else 
        {
            itemEmptyText[rifleBulletIdx].text = gunData.Rf_Count.ToString();
        }
    }
    private void CaseShotGunBullet()
    {
        gunData.Sg_Count += 10;
        if(fireCtrl.isShotGun)
            fireCtrl.bulletText.text = fireCtrl.shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();
        if (!isShotGunBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = ItemDataBase.ItemType.SHOTGUNBULLET;
                shotgunBulletIdx = itemEmptyIdx;
                AddItem(i, gunData.sgB_Sprite, gunData.Sg_Count);
                
                isShotGunBullet = true;
                break;
            }
        }
        else 
        {
            itemEmptyText[shotgunBulletIdx].text = gunData.Sg_Count.ToString();
        }
    }
    private void AddGun(Sprite sprite, ItemDataBase.ItemType type, int idx)
    {
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (imageDropList[i].childCount > 0) continue;
            itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
            itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().sprite = sprite;
            itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().enabled = true;
            itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = type;
            itemEmptyIdx++;
            idx = itemEmptyIdx;
            if (itemEmptyIdx >= 16) itemEmptyIdx = 0;
            break;
        }
    }
    private void CaseGranade()
    {
        granadeData.Count++;
        fireCtrl.getGranade = true;
        if(fireCtrl.isGranade)
            fireCtrl.bulletText.text = granadeData.Count.ToString();
        if (!isGranadeGet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = ItemDataBase.ItemType.GRENADE;
                granadeIdx = itemEmptyIdx;
                AddItem(i, granadeData.sprite, granadeData.Count);
                if(fireCtrl.isGranade)
                    fireCtrl.granadeMesh.enabled = true;
                isGranadeGet = true;
                break;
            }
        }
        else
        {
            itemEmptyText[granadeIdx].text = granadeData.Count.ToString();
        }

    }
    private void AddItem(int array, Sprite sprite, int itemCount)
    {
        itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[array]);
        itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().sprite = sprite;
        itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().enabled = true;
        itemEmptyText[itemEmptyIdx].gameObject.SetActive(true);
        itemEmptyText[itemEmptyIdx].text = itemCount.ToString();
        itemEmptyIdx++;
        if (itemEmptyIdx >= 16) itemEmptyIdx = 0;
    }
    private void HealItem()
    {
        madicinData.m_Count--;
        itemEmptyText[healIdx].text = madicinData.m_Count.ToString();
        playerDamage.HP += (int)madicinData.m_Value;
        playerDamage.hpBarImage.fillAmount = (float)playerDamage.HP / (float)playerData.maxHp;
        if(playerDamage.HP >= 100)
            playerDamage.HP = 100;
        if(madicinData.m_Count == 0)
        {
            itemEmptyRectList[healIdx].SetParent(itemEmptyRect[0]);
            itemEmptyRectList[healIdx].GetComponent<Image>().enabled = false;
            itemEmptyRectList[healIdx].GetComponent<Button>().enabled = false;
            itemEmptyText[healIdx].gameObject.SetActive(false);
            isHeal = false;
        }
    }
}
