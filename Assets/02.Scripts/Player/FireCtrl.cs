using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] private GunData gunData;           //총 스크립터블 데이터
    [SerializeField] private GranadeData granadeData;   //수류탄 스크립터블 데이터
    private Transform rifleFirePos;                     //라이플 총이 발사될 트랜스폼
    private Transform shotgunFirePos;                   //샷건이 발사될 트랜스폼
    private Transform granadePos;                       //그래네이드가 던져질 트랜스폼
    private AudioSource source;                         //오디오 소스
    private Animator animator;                          //애니메이터
    private PlayerDamage playerDamage;                  //플레이어가 받을 데미지
    private ParticleSystem rifleFlash;                  //라이플이 쏴질때 플레이 될 파티클
    private ParticleSystem shotgunFlahs;                //샷건이 쏴질때 플레이 될 파티클
    private CanvasGroup canvasGroup;                    //인벤토리를 보이거나 안보이게 함
    public Image weaponImage;                           //총을 바꿀때 Ui상에 보이게 할 이미지
    public Text bulletText;                             //총알의 개수를 텍스트로 보이게
    private RectTransform itemEmptyGroup;               //총알을 다 썻을때 인벤토리의 
    private MeshRenderer rifleMesh;                     //총을 바꿀때 나타날 라이플 매쉬
    private MeshRenderer shotgunMesh;                   //총을 바꿀때 나타날 샷건 매쉬
    public MeshRenderer granadeMesh;                    //수류탄으로 바꿀때 나타날 수류탄 매쉬
    private bool tabOn;                                 //인벤토리를 켰는지 안켰는지 확인하기 위한 불변수
    private float curTime;                              //총이 발사 되는 시간을 구하기 위한 현재 시간
    private float fireTIme;                             //총이 발사 되는 시간
    private readonly int aniFire = Animator.StringToHash("FireTrigger");
    private readonly int aniReload = Animator.StringToHash("ReloadTrigger");
    private readonly int aniIsReload = Animator.StringToHash("IsReload");
    private readonly int aniIsGun = Animator.StringToHash("IsGun");
    private readonly int aniGranade = Animator.StringToHash("GranadeTrigger");
    public int rifleBulletCount;
    private int rifleBulletMaxCount;
    public int shotgunBulletCount;
    private int shotgunBulletMaxCount;
    private bool isReload;
    private bool isThrow;
    public bool isGranade;
    public bool isRifle;
    public bool isShotGun;
    
    void OnEnable()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        rifleFirePos = GameObject.Find("FirePos_Rifle").transform;
        shotgunFirePos = GameObject.Find("FirePos_ShotGun").transform;
        granadePos = GameObject.Find("GrandePos").transform;

        rifleFlash = rifleFirePos.GetChild(0).GetComponent<ParticleSystem>();
        shotgunFlahs = shotgunFirePos.GetChild(0).GetComponent <ParticleSystem>();

        rifleMesh = rifleFirePos.parent.GetComponent<MeshRenderer>();
        shotgunMesh = shotgunFirePos.parent.GetComponent<MeshRenderer>();
        granadeMesh = GameObject.Find("SCIGrenade").GetComponent<MeshRenderer>();

        itemEmptyGroup = GameObject.Find("Item_EmptyGroup").GetComponent<RectTransform>();
        source = GetComponent<AudioSource>();
        canvasGroup = GameObject.Find("Canvas_ui").transform.GetChild(0).GetComponent<CanvasGroup>();
        weaponImage = GameObject.Find("Panel-Weapon").transform.GetChild(0).GetComponent<Image>();
        bulletText = weaponImage.transform.parent.GetChild(1).GetComponent<Text>();
        playerDamage = GetComponent<PlayerDamage>();
        curTime = Time.time;
        fireTIme = 0.1f;
        rifleBulletMaxCount = 30;
        rifleBulletCount = gunData.Rf_Count;

        shotgunBulletMaxCount = 10;
        shotgunBulletCount = gunData.Sg_Count;

        Cursor.lockState = CursorLockMode.Locked;
        isThrow = false;
        Cursor.visible = false;
        isGranade = false;
        isShotGun = false;
        isRifle = false;
        tabOn = false;
        isReload = false;
        rifleFlash.Stop();
        shotgunFlahs.Stop();
        StartCoroutine(OnFIre());
        granadeData.Count = 0;
        gunData.Rf_Count = 0;
        gunData.Sg_Count = 0;
    }
    IEnumerator OnFIre()
    {
        while(!playerDamage.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            ChangeWeapon();
            if (isRifle)
                RifleFireAndReload();
            else
                rifleMesh.enabled = false;
            if (isShotGun)
                ShotGunFireAndReload();
            else
                shotgunMesh.enabled = false;
            if (isGranade)
                GranadeThrow();
            else
                granadeMesh.enabled = false;
            TabInventory();
        }
    }
    private void TabInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabOn = !tabOn;
        }
        if (tabOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canvasGroup.alpha = 1;
        }
        else if (!tabOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canvasGroup.alpha = 0;
        }
    }
    private void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && GameManager.Instance.getRifle)
        {
            ChangeRifle();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && GameManager.Instance.getShotGun)
        {
            ChangeShotGun();
        }
        else if( Input.GetKeyDown(KeyCode.Alpha3) && GameManager.Instance.getGranade)
        {
            ChangeGranade();
        }
    }
    private void RifleFireAndReload()
    {
        if (Time.time - curTime > fireTIme && Input.GetMouseButton(0) && isRifle)
        {
            if (!isReload && !tabOn && rifleBulletCount > 0)
            {
                ShootFire(rifleFlash, "RifleFlashStop", rifleBulletCount, gunData.rifleClip, gunData.Rf_Count);
                rifleBulletCount--;
                if (rifleBulletCount == 0 & gunData.Rf_Count > 0)
                    StartCoroutine(RifleReload());
            }
            curTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.R) && rifleBulletCount != gunData.Rf_Count && !isReload & gunData.Rf_Count > 0 && rifleBulletCount < 30 && isRifle)
            StartCoroutine(RifleReload());
    }

    private void ShotGunFireAndReload()
    {
        if (Input.GetMouseButtonDown(0) && isShotGun)
        {
            if (!isReload && !tabOn && shotgunBulletCount > 0)
            {
                ShootFire(shotgunFlahs, "ShotGunFlashStop", shotgunBulletCount,gunData.shotgunClip,gunData.Sg_Count);
                shotgunBulletCount--;
                if (shotgunBulletCount == 0 && gunData.Sg_Count > 0)
                    StartCoroutine(ShotGunReload());
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && shotgunBulletCount != gunData.Sg_Count && !isReload & gunData.Sg_Count> 0 && shotgunBulletCount < 10 && isShotGun)
            StartCoroutine(ShotGunReload());
    }
    private void GranadeThrow()
    {
        if (Input.GetMouseButtonDown(0) && isGranade && !isThrow)
        {
            if(granadeData.Count > 0 && !tabOn)
            {
                animator.SetTrigger(aniGranade);
                StartCoroutine(ThrowGranade());
                
            }
        }
        else if (granadeData.Count == 0)
        {
            granadeMesh.enabled = false;
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.granadeIdx].SetParent(itemEmptyGroup);
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.granadeIdx].GetComponent<Image>().enabled = false;
            GameManager.Instance.itemEmptyText[GameManager.Instance.granadeIdx].enabled = false;
            GameManager.Instance.getGranade = false;
        }
    }
    IEnumerator ThrowGranade()
    {
        isThrow = true;
        granadeData.Count--;
        yield return new WaitForSeconds(1.8f);
        SoundManager.soundInst.PlayeOneShot(granadeData.throwClip, source);
        bulletText.text = granadeData.Count.ToString();
        GameObject _granade = ObjectPoolingManager.objPooling.GetWeaponGranade();
        _granade.transform.position = granadePos.position;
        _granade.transform.rotation = granadePos.rotation;
        _granade.SetActive(true);
        GameManager.Instance.itemEmptyText[GameManager.Instance.granadeIdx].text = gunData.Rf_Count.ToString();
        yield return new WaitForSeconds(2f);
        isThrow = false;
    }
    public void ChangeGranade()
    {
        isGranade = true;
        isRifle = false;
        isShotGun = false;
        granadeMesh.enabled = true;
        animator.SetBool(aniIsGun, false);
        bulletText.text = granadeData.Count.ToString();
        weaponImage.sprite = granadeData.ui_Sprite;
    }
    public void ChangeShotGun()
    {
        animator.SetBool(aniIsGun, true);
        isRifle = false;
        isGranade = false;
        isShotGun = true;
        rifleMesh.enabled = false;
        shotgunMesh.enabled = true;
        gunData.g_damage = 50;
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();
        weaponImage.sprite = gunData.sg_UISprite;
    }
    public void ChangeRifle()
    {
        animator.SetBool(aniIsGun, true);
        isShotGun = false;
        isRifle = true;
        isGranade = false;
        shotgunMesh.enabled = false;
        rifleMesh.enabled = true;
        gunData.g_damage = 15;
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();
        weaponImage.sprite = gunData.rf_UISprite;
    }
    void ShootFire(ParticleSystem particle, string st, int bulletCount, AudioClip clip, int itemDatabaseCount)
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();
        _bullet.transform.position = shotgunFirePos.position;
        _bullet.transform.rotation = shotgunFirePos.rotation;
        _bullet.SetActive(true);
        particle.Play();
        Invoke(st, 0.1f);
        animator.SetTrigger(aniFire);
        bulletText.text = bulletCount.ToString() + " / " + itemDatabaseCount.ToString();
        SoundManager.soundInst.PlayeOneShot(clip, source);
    }

    IEnumerator RifleReload()
    {
        
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
        SoundManager.soundInst.PlayeOneShot(gunData.rifleReloadClip, source);
        animator.SetBool(aniIsReload, false);
        isReload = false;
        if(gunData.Rf_Count >= rifleBulletMaxCount)
        {
            if(rifleBulletCount == 0)
            {
                rifleBulletCount += rifleBulletMaxCount;
                gunData.Rf_Count -= rifleBulletCount;
            }
            else if(rifleBulletCount > 0)
            {
                int cot = rifleBulletMaxCount;
                cot -= rifleBulletCount;
                rifleBulletCount += cot;
                gunData.Rf_Count -= cot;
            }
        }
        else if(gunData.Rf_Count < rifleBulletMaxCount)
        {
            if(rifleBulletCount == 0)
            {
                rifleBulletCount += gunData.Rf_Count;
                gunData.Rf_Count -= rifleBulletCount; 
            }
            else if(rifleBulletCount > 0)
            { 
                int let = rifleBulletMaxCount;
                let -= rifleBulletCount;
                if(gunData.Rf_Count >= let)
                {
                    rifleBulletCount += let;
                    gunData.Rf_Count -= let;
                }
                else if(gunData.Rf_Count < let)
                {
                    rifleBulletCount += gunData.Rf_Count;
                    gunData.Rf_Count -= gunData.Rf_Count;
                }
            }
            
        }
        /*GameManager.Instance.itemEmptyObject.text = bulletValue.ToString();*/
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();
        GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].text = gunData.Rf_Count.ToString();
        if(gunData.Rf_Count == 0)
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.rifleBulletIdx].SetParent(itemEmptyGroup);
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.rifleBulletIdx].GetComponent<Image>().enabled = false;
            GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].enabled = false;
        }
    }
    IEnumerator ShotGunReload()
    {
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
        SoundManager.soundInst.PlayeOneShot(gunData.shotgunReloadClip, source); 
        animator.SetBool(aniIsReload, false);
        isReload = false;
        if (gunData.Sg_Count >= shotgunBulletMaxCount)
        {
            if (shotgunBulletCount == 0)
            {
                shotgunBulletCount += shotgunBulletMaxCount;
                gunData.Sg_Count -= shotgunBulletCount;
            }
            else if (shotgunBulletCount > 0)
            {
                int shot = shotgunBulletMaxCount;
                shot -= shotgunBulletCount;
                shotgunBulletCount += shot;
                gunData.Sg_Count -= shot;
            }
        }
        else if (gunData.Sg_Count < shotgunBulletMaxCount)
        {
            if (shotgunBulletCount == 0)
            {
                shotgunBulletCount += gunData.Sg_Count;
                gunData.Sg_Count -= shotgunBulletCount;
            }
            else if (shotgunBulletCount > 0)
            {
                int gun = shotgunBulletMaxCount;
                gun -= shotgunBulletCount;
                if (gunData.Sg_Count >= gun)
                {
                    shotgunBulletCount += gun;
                    gunData.Sg_Count -= gun;
                }
                else if (gunData.Sg_Count < gun)
                {
                    shotgunBulletCount += gunData.Sg_Count;
                    gunData.Sg_Count -= gunData.Sg_Count;
                }
            }

        }
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();
        GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].text = gunData.Sg_Count.ToString();
        if (gunData.Sg_Count == 0)
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.shotgunBulletIdx].SetParent(itemEmptyGroup);
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.shotgunBulletIdx].GetComponent<Image>().enabled = false;
            GameManager.Instance.itemEmptyText[GameManager.Instance.shotgunBulletIdx].enabled = false;
        }
    }
    void RifleFlashStop()
    {
        rifleFlash.Stop();
    }
    void ShotGunFlashStop()
    {
        shotgunFlahs.Stop();
    }
}
