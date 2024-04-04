
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    [SerializeField] MadicinData madicinData;
    [SerializeField] GunData gunData;
    [SerializeField] PlayerData playerData;
    [SerializeField] GranadeData granadeData;
    [SerializeField] ItemInfo info;
    public List<Text> itemEmptyText = new List<Text>();
    
    private RectTransform[] itemEmptyRect;
    public List<RectTransform> itemEmptyRectList = new List<RectTransform>();

    private RectTransform[] imageDrop;
    private List<RectTransform> imageDropList = new List<RectTransform>();

    private Text scoreText;

    private PlayerDamage playerDamage;
    private FireCtrl fireCtrl;

    private int killCount = 0;
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
    public bool getGranade;
    public bool getShotGun;
    public bool getRifle;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }/*
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);*/
    }
    void OnEnable()
    {
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
        scoreText = GameObject.Find("Text (Legacy)-KillScore").GetComponent<Text>();  
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
        getGranade = false;
        getShotGun = false;
        getRifle = false;

    }
    public void AddItem(ItemData.ItemType itemType)
    {
        switch(itemType)
        {
            case ItemData.ItemType.Meadicine:
                CaseHeal();
                break;
            case ItemData.ItemType.RIFLE:
                AddGun(gunData.rf_Sprite, ItemData.ItemType.RIFLE, rifleIdx);
                if (!getShotGun && !getGranade)
                    fireCtrl.weaponImage.sprite = gunData.rf_UISprite;
                getRifle = true;
                break;
            case ItemData.ItemType.SHOTGUN:
                AddGun(gunData.sg_Sprite, ItemData.ItemType.SHOTGUN, shotgunIdx);
                if (!getRifle && !getGranade)
                    fireCtrl.weaponImage.sprite = gunData.sg_UISprite;
                getShotGun = true;
                break;
            case ItemData.ItemType.RIFLEBULLET:
                CaseRifleBullet();
                break;
            case ItemData.ItemType.SHOTGUNBULLET:
                CaseShotGunBullet();
                break;
            case ItemData.ItemType.GRENADE:
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
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = ItemData.ItemType.RIFLE;
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
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = ItemData.ItemType.SHOTGUNBULLET;
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
    private void AddGun(Sprite sprite,ItemData.ItemType type, int idx)
    {
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (imageDropList[i].childCount > 0) continue;
            itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
            itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().sprite = sprite;
            itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().enabled = true;
            itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = type;
            itemEmptyIdx++;
            idx = itemEmptyIdx;
            fireCtrl.weaponImage.enabled = true;
            if (itemEmptyIdx >= 16) itemEmptyIdx = 0;
            break;
        }
    }
    private void CaseGranade()
    {
        granadeData.Count++;
        getGranade = true;
        if(!getRifle && !getGranade)
        fireCtrl.weaponImage.sprite = granadeData.ui_Sprite;
        fireCtrl.weaponImage.enabled = true;
        if (fireCtrl.isGranade)
            fireCtrl.bulletText.text = granadeData.Count.ToString();
        if (!isGranadeGet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = ItemData.ItemType.GRENADE;
                granadeIdx = itemEmptyIdx;
                AddItem(i, granadeData.itme_Sprite, granadeData.Count);
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
        playerDamage.hp += (int)madicinData.m_Value;
        playerDamage.hpBarImage.fillAmount = (float)playerDamage.hp / (float)playerData.maxHp;
        if(playerDamage.hp >= 100)
            playerDamage.hp = 100;
        if(madicinData.m_Count == 0)
        {
            itemEmptyRectList[healIdx].SetParent(itemEmptyRect[0]);
            itemEmptyRectList[healIdx].GetComponent<Image>().enabled = false;
            itemEmptyRectList[healIdx].GetComponent<Button>().enabled = false;
            itemEmptyText[healIdx].gameObject.SetActive(false);
            isHeal = false;
        }
    }
    public void ScoreUp(int score)
    {
        killCount += score;
        scoreText.text = killCount.ToString();
    }
    public void ScoureSave()
    {
        PlayerPrefs.SetInt("KillCount", killCount);
    }
    public void ScoreDelete()
    {
        PlayerPrefs.DeleteKey("KillCount");
    }
}
