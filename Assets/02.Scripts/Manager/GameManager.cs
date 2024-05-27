
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null; // 싱글톤 인스턴스
    [SerializeField] private MadicinData madicinData; // 약 데이터
    [SerializeField] private GunData gunData; // 총 데이터
    [SerializeField] private PlayerData playerData; // 플레이어 데이터
    [SerializeField] private GranadeData granadeData; // 수류탄 데이터
    public List<Text> itemEmptyText = new List<Text>(); // 아이템 텍스트 리스트

    private RectTransform[] itemEmptyRect; // 아이템 빈 슬롯 배열
    public List<RectTransform> itemEmptyRectList = new List<RectTransform>(); // 아이템 빈 슬롯 리스트
    private RectTransform[] imageDrop; // 드롭된 이미지 배열
    private List<RectTransform> imageDropList = new List<RectTransform>(); // 드롭된 이미지 리스트

    private Text scoreText; // 점수 텍스트

    private PlayerDamage playerDamage; // 플레이어 데미지
    private FireCtrl fireCtrl; // 발사 컨트롤러

    public int killCount = 0; // 킬 카운트
    public int itemEmptyIdx; // 아이템 빈 슬롯 인덱스
    public int rifleIdx; // 라이플 인덱스
    public int shotgunIdx; // 샷건 인덱스
    public int rifleBulletIdx; // 라이플 총알 인덱스
    public int shotgunBulletIdx; // 샷건 총알 인덱스
    public int granadeIdx; // 수류탄 인덱스
    private int healIdx; // 힐 인덱스
    private bool isHeal; // 힐 여부
    public bool isRifleBullet; // 라이플 총알 여부
    public bool isShotGunBullet; // 샷건 총알 여부
    public bool isGranadeGet; // 수류탄 획득 여부
    public bool getGranade; // 수류탄 획득 상태
    public bool getShotGun; // 샷건 획득 상태
    public bool getRifle; // 라이플 획득 상태

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        // 각종 컴포넌트 초기화
        fireCtrl = FindObjectOfType<FireCtrl>();
        itemEmptyRect = GameObject.Find("Item_EmptyGroup").transform.GetComponentsInChildren<RectTransform>(includeInactive: false);
        imageDrop = GameObject.Find("ItemList").GetComponentsInChildren<RectTransform>();
        playerDamage = FindObjectOfType<PlayerDamage>();

        // 이미지 드롭 리스트 초기화
        for (int i = 1; i < imageDrop.Length; i++)
        {
            imageDropList.Add(imageDrop[i]);
        }

        // 아이템 빈 슬롯 리스트 초기화
        for (int i = 1; i < itemEmptyRect.Length; i++)
        {
            itemEmptyRectList.Add(itemEmptyRect[i]);
        }

        // 아이템 빈 슬롯 텍스트 초기화
        for (int i = 0; i < itemEmptyRectList.Count; i++)
        {
            Text textComponent = itemEmptyRectList[i].transform.GetChild(0).GetComponent<Text>();
            itemEmptyText.Add(textComponent);
        }

        scoreText = GameObject.Find("Text (Legacy)-KillScore").GetComponent<Text>(); // 점수 텍스트 초기화
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

    // 아이템 추가 메서드
    public void AddItem(ItemData.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemData.ItemType.Meadicine:
                CaseHeal();
                break;
            case ItemData.ItemType.RIFLE:
                AddGun(gunData.rf_Sprite, ItemData.ItemType.RIFLE, ref rifleIdx);
                if (!getShotGun && !getGranade)
                    fireCtrl.weaponImage.sprite = gunData.rf_UISprite;
                getRifle = true;
                break;
            case ItemData.ItemType.SHOTGUN:
                AddGun(gunData.sg_Sprite, ItemData.ItemType.SHOTGUN, ref shotgunIdx);
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

    // 힐 아이템 처리 메서드
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
                AddItemToInventory(i, madicinData.m_Sprite, madicinData.m_Count);
                isHeal = true;
                break;
            }
            else
            {
                itemEmptyText[healIdx].text = madicinData.m_Count.ToString();
            }
        }
    }

    // 라이플 총알 처리 메서드
    private void CaseRifleBullet()
    {
        gunData.Rf_Count += 30;
        if (fireCtrl.isRifle)
            fireCtrl.bulletText.text = fireCtrl.rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();

        if (!isRifleBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = ItemData.ItemType.RIFLEBULLET;
                rifleBulletIdx = itemEmptyIdx;
                AddItemToInventory(i, gunData.rfB_Sprite, gunData.Rf_Count);
                isRifleBullet = true;
                break;
            }
        }
        else
        {
            itemEmptyText[rifleBulletIdx].text = gunData.Rf_Count.ToString();
        }
    }

    // 샷건 총알 처리 메서드
    private void CaseShotGunBullet()
    {
        gunData.Sg_Count += 10;
        if (fireCtrl.isShotGun)
            fireCtrl.bulletText.text = fireCtrl.shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();

        if (!isShotGunBullet)
        {
            for (int i = 0; i < imageDropList.Count; i++)
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = ItemData.ItemType.SHOTGUNBULLET;
                shotgunBulletIdx = itemEmptyIdx;
                AddItemToInventory(i, gunData.sgB_Sprite, gunData.Sg_Count);
                isShotGunBullet = true;
                break;
            }
        }
        else
        {
            itemEmptyText[shotgunBulletIdx].text = gunData.Sg_Count.ToString();
        }
    }

    // 총기 추가 메서드
    private void AddGun(Sprite sprite, ItemData.ItemType type, ref int idx)
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

    // 수류탄 처리 메서드
    private void CaseGranade()
    {
        granadeData.Count++;
        getGranade = true;
        if (!getRifle && !getGranade)
            fireCtrl.weaponImage.sprite = granadeData.ui_Sprite;
        fireCtrl.weaponImage.enabled = true;
        if (fireCtrl.isGranade)
            fireCtrl.bulletText.text = granadeData.Count.ToString();

        if (!isGranadeGet)
        {
            for (int i = 0; i < imageDropList.Count; i++)
            {
                if (imageDropList[i].childCount > 0) continue;
                itemEmptyRectList[itemEmptyIdx].gameObject.GetComponent<ItemInfo>().itemType = ItemData.ItemType.GRENADE;
                granadeIdx = itemEmptyIdx;
                AddItemToInventory(i, granadeData.itme_Sprite, granadeData.Count);
                if (fireCtrl.isGranade)
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

    // 인벤토리에 아이템 추가 메서드
    private void AddItemToInventory(int array, Sprite sprite, int itemCount)
    {
        itemEmptyRectList[itemEmptyIdx].SetParent(imageDropList[array]);
        itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().sprite = sprite;
        itemEmptyRectList[itemEmptyIdx].GetComponent<Image>().enabled = true;
        itemEmptyText[itemEmptyIdx].gameObject.SetActive(true);
        itemEmptyText[itemEmptyIdx].text = itemCount.ToString();
        itemEmptyIdx++;
        if (itemEmptyIdx >= 16) itemEmptyIdx = 0;
    }

    // 힐 아이템 사용 메서드
    private void HealItem()
    {
        madicinData.m_Count--;
        itemEmptyText[healIdx].text = madicinData.m_Count.ToString();
        playerDamage.hp += (int)madicinData.m_Value;
        playerDamage.hpBarImage.fillAmount = (float)playerDamage.hp / playerData.maxHp;
        if (playerDamage.hp >= 100)
            playerDamage.hp = 100;
        if (madicinData.m_Count == 0)
        {
            itemEmptyRectList[healIdx].SetParent(itemEmptyRect[0]);
            itemEmptyRectList[healIdx].GetComponent<Image>().enabled = false;
            itemEmptyRectList[healIdx].GetComponent<Button>().enabled = false;
            itemEmptyText[healIdx].gameObject.SetActive(false);
            isHeal = false;
        }
    }

    // 점수 증가 메서드
    public void ScoreUp(int score)
    {
        killCount += score;
        scoreText.text = killCount.ToString();
    }
}

