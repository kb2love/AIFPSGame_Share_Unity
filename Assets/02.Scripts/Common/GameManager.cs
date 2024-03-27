
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public List<Text> itemEmptyText = new List<Text>();

    [SerializeField] private RectTransform[] itemEmptyRect;
    public List<RectTransform> itemEmptyRectList = new List<RectTransform>();

    private RectTransform[] imageDrop;
    private List<RectTransform> imageDropList = new List<RectTransform>();

    private PlayerDamage playerDamage;
    private LoopSpawn loopSpawn;

    private Sprite rifleBulletBoxSprite;
    private Sprite shotgunBulletBoxSprite; 
    private Sprite shotGunSprite;
    private Sprite rifleSprite;
    private Sprite healSprite;

    private FireCtrl fireCtrl;
    public int itemEmptyIdx;
    public int rifleBulletIdx;
    public int shotgunBulletIdx;
    private int healIdx;
    private bool isHeal;
    public bool isRifleBullet;
    public bool isShotGunBullet;
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

        fireCtrl = FindObjectOfType<FireCtrl>();
        rifleBulletBoxSprite = Resources.Load<Sprite>("Image/Inventory/PistolBulletBox");
        shotgunBulletBoxSprite = Resources.Load<Sprite>("Image/Inventory/ShotGunBulletBox");
        shotGunSprite = Resources.Load<Sprite>("Image/Inventory/ShotGunImage");
        rifleSprite = Resources.Load<Sprite>("Image/Inventory/RifleImage");
        healSprite = Resources.Load<Sprite>("Image/Inventory/Madecian");
        itemEmptyRect = GameObject.Find("Item_EmptyGroup").transform.GetComponentsInChildren<RectTransform>(includeInactive: false);
        imageDrop = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        playerDamage = FindObjectOfType<PlayerDamage>();   
        loopSpawn = FindObjectOfType<LoopSpawn>();
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
        isRifleBullet = false;
        isShotGunBullet = false;
        isHeal = false;
    }
    public void AddItem(ItemDataBase.ItemType itemType)
    {
        switch(itemType)
        {
            case ItemDataBase.ItemType.HEAL:
                CaseHeal();
                break;
            case ItemDataBase.ItemType.RIFLE:
                AddGun(rifleSprite, ItemDataBase.ItemType.RIFLE);
                break;
            case ItemDataBase.ItemType.SHOTGUN:
                AddGun(shotGunSprite, ItemDataBase.ItemType.SHOTGUN);
                break;
            case ItemDataBase.ItemType.RIFLEBULLET:
                CaseRifleBullet();
                break;
            case ItemDataBase.ItemType.SHOTGUNBULLET:
                CaseShotGunBullet();
                break;
            case ItemDataBase.ItemType.GRENADE:

                break;

        }
    }
    private void CaseHeal()
    {

        ItemDataBase.itemDataBase.itemHealCount++;
        Debug.Log(ItemDataBase.itemDataBase.itemHealCount);
        itemEmptyText[itemEmptyIdx].text = ItemDataBase.itemDataBase.itemHealCount.ToString();
        loopSpawn.spawnMadicineCount--;
        loopSpawn.spawnMaxMadicineCount--;
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (!isHeal)
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].GetComponent<Button>().onClick.AddListener(HealItem);
                healIdx = itemEmptyIdx;
                AddItem(i, healSprite,ItemDataBase.itemDataBase.itemHealCount);
                isHeal = true;
                break;
            }
            else
            {
                itemEmptyText[healIdx].text = ItemDataBase.itemDataBase.itemHealCount.ToString();
            }
        }
    }
    private void AddGun(Sprite sprite, ItemDataBase.ItemType type)
    {
        for (int i = 0; i < imageDropList.Count; i++)
        {
            if (imageDropList[i].childCount > 0) continue;
            itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[i]);
            itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().sprite = sprite;
            itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().enabled = true;
            itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = type;
            itemEmptyIdx++;
            if (itemEmptyIdx >= 16) itemEmptyIdx = 0;
            break;
        }
    }
    private void CaseRifleBullet()
    {
        ItemDataBase.itemDataBase.rifleBulletCount += 30;
        if(fireCtrl.isRifle)
            fireCtrl.bulletText.text = fireCtrl.rifleBulletCount.ToString() + " / " + ItemDataBase.itemDataBase.rifleBulletCount.ToString();
        
        if(!isRifleBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                rifleBulletIdx = itemEmptyIdx;
                AddItem(i, rifleBulletBoxSprite,ItemDataBase.itemDataBase.rifleBulletCount);
                isRifleBullet = true;
                break;
            }
        }
        else 
        {
            itemEmptyText[rifleBulletIdx].text = ItemDataBase.itemDataBase.rifleBulletCount.ToString();
        }
    }
    private void CaseShotGunBullet()
    {
        Debug.Log("Bullet");
        ItemDataBase.itemDataBase.shotgunBulletCount += 10;
        if(fireCtrl.isShotGun)
            fireCtrl.bulletText.text = fireCtrl.shotgunBulletCount.ToString() + " / " + ItemDataBase.itemDataBase.shotgunBulletCount.ToString();
        if (!isShotGunBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)  //인벤토리에 불렛이 추가된다
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemDataBase>().itemType = ItemDataBase.ItemType.SHOTGUNBULLET;
                shotgunBulletIdx = itemEmptyIdx;
                AddItem(i, shotgunBulletBoxSprite, ItemDataBase.itemDataBase.shotgunBulletCount);
                
                isShotGunBullet = true;
                break;
            }
        }
        else 
        {
            itemEmptyText[shotgunBulletIdx].text = ItemDataBase.itemDataBase.shotgunBulletCount.ToString();
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
        ItemDataBase.itemDataBase.itemHealCount--;
        itemEmptyText[healIdx].text = ItemDataBase.itemDataBase.itemHealCount.ToString();
        playerDamage.HP += (int)ItemDataBase.itemDataBase.itemHealValue;
        playerDamage.hpBarImage.fillAmount = (float)playerDamage.HP / (float)playerDamage.maxHp;
        if(playerDamage.HP >= 100)
            playerDamage.HP = 100;
        if(ItemDataBase.itemDataBase.itemHealCount == 0)
        {
            itemEmptyRectList[healIdx].SetParent(itemEmptyRect[0]);
            itemEmptyRectList[healIdx].GetComponent<Image>().enabled = false;
            itemEmptyRectList[healIdx].GetComponent<Button>().enabled = false;
            itemEmptyText[healIdx].gameObject.SetActive(false);
            isHeal = false;
        }
        GetComponent<LoopSpawn>().spawnMadicineCount--;
    }
}
