using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] GunData gunData;
    [SerializeField] GranadeData granadeData;
    private Transform rifleFirePos;
    private Transform shotgunFirePos;
    private Transform granadePos;
    private AudioSource source;
    private Animator animator;
    private PlayerDamage playerDamage;
    private ParticleSystem rifleFlash;
    private ParticleSystem shotgunFlahs;
    private CanvasGroup canvasGroup;
    public Image weaponImage;
    public Text bulletText;
    private RectTransform itemEmptyGroup;
    private MeshRenderer rifleMesh;
    private MeshRenderer shotgunMesh;
    public MeshRenderer granadeMesh;
    private bool tabOn;
    private float curTime;
    private float fireTIme;
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
    public bool isGranade;
    public bool isRifle;
    public bool isShotGun;
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        rifleFirePos = animator.transform.GetChild(2).GetChild(0).GetChild(0).transform;
        shotgunFirePos = animator.transform.GetChild(2).GetChild(1).GetChild(0).transform;
        granadePos = animator.transform.GetChild(6).transform;

        rifleFlash = rifleFirePos.GetChild(0).GetComponent<ParticleSystem>();
        shotgunFlahs = shotgunFirePos.GetChild(0).GetComponent <ParticleSystem>();

        rifleMesh = rifleFirePos.parent.GetComponent<MeshRenderer>();
        shotgunMesh = shotgunFirePos.parent.GetComponent<MeshRenderer>();
        granadeMesh = animator.transform.GetChild(2).GetChild(2).GetComponent<MeshRenderer>();

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
        if(Input.GetKeyDown(KeyCode.Alpha1) && ItemManager.Instance.getRifle)
        {
            ChangeRifle();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && ItemManager.Instance.getShotGun)
        {
            ChangeShotGun();
        }
        else if( Input.GetKeyDown(KeyCode.Alpha3) && ItemManager.Instance.getGranade)
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
        if (Input.GetMouseButtonDown(0) && isGranade)
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
            ItemManager.Instance.itemEmptyRectList[ItemManager.Instance.granadeIdx].SetParent(itemEmptyGroup);
            ItemManager.Instance.itemEmptyRectList[ItemManager.Instance.granadeIdx].GetComponent<Image>().enabled = false;
            ItemManager.Instance.itemEmptyText[ItemManager.Instance.granadeIdx].enabled = false;
            ItemManager.Instance.isGranadeGet = false;
            ItemManager.Instance.getGranade = false;
        }
    }
    IEnumerator ThrowGranade()
    {
        granadeData.Count--;
        yield return new WaitForSeconds(1.8f);
        SoundManager.soundInst.PlayeOneShot(granadeData.throwClip, source);
        bulletText.text = granadeData.Count.ToString();
        GameObject _granade = ObjectPoolingManager.objPooling.GetWeaponGranade();
        _granade.transform.position = granadePos.position;
        _granade.transform.rotation = granadePos.rotation;
        _granade.SetActive(true);
        
        ItemManager.Instance.itemEmptyText[ItemManager.Instance.granadeIdx].text = gunData.Rf_Count.ToString();
        yield return new WaitForSeconds(4f);
        SoundManager.soundInst.PlayeOneShot(granadeData.expClip, source);
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
        /*ItemManager.Instance.itemEmptyObject.text = bulletValue.ToString();*/
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();
        ItemManager.Instance.itemEmptyText[ItemManager.Instance.rifleBulletIdx].text = gunData.Rf_Count.ToString();
        if(gunData.Rf_Count == 0)
        {
            ItemManager.Instance.itemEmptyRectList[ItemManager.Instance.rifleBulletIdx].SetParent(itemEmptyGroup);
            ItemManager.Instance.itemEmptyRectList[ItemManager.Instance.rifleBulletIdx].GetComponent<Image>().enabled = false;
            ItemManager.Instance.itemEmptyText[ItemManager.Instance.rifleBulletIdx].enabled = false;
            ItemManager.Instance.isRifleBullet = false;
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
        ItemManager.Instance.itemEmptyText[ItemManager.Instance.rifleBulletIdx].text = gunData.Sg_Count.ToString();
        if (gunData.Sg_Count == 0)
        {
            ItemManager.Instance.itemEmptyRectList[ItemManager.Instance.shotgunBulletIdx].SetParent(itemEmptyGroup);
            ItemManager.Instance.itemEmptyRectList[ItemManager.Instance.shotgunBulletIdx].GetComponent<Image>().enabled = false;
            ItemManager.Instance.itemEmptyText[ItemManager.Instance.shotgunBulletIdx].enabled = false;
            ItemManager.Instance.isShotGunBullet = false;
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
