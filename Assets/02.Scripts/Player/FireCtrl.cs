using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] GunData gunData;
    private Transform rifleFirePos;
    private Transform shotgunFirePos;
    private AudioSource source;
    private AudioClip rifleClip;
    private AudioClip shotgunClip;
    private Animator animator;
    private PlayerDamage playerDamage;
    private ParticleSystem rifleFlash;
     private ParticleSystem shotgunFlahs;
    private CanvasGroup canvasGroup;
    private Image weaponImage;
    public Text bulletText;
    private RectTransform itemEmptyGroup;
    private MeshRenderer rifleMesh;
    private MeshRenderer shotgunMesh;
    private bool tabOn;
    private float curTime;
    private float fireTIme;
    private readonly int aniFire = Animator.StringToHash("FireTrigger");
    private readonly int aniReload = Animator.StringToHash("ReloadTrigger");
    private readonly int aniIsReload = Animator.StringToHash("IsReload");
    public int rifleBulletCount;
    private int rifleBulletMaxCount;
    public int shotgunBulletCount;
    private int shotgunBulletMaxCount;
    private bool isReload;
    public bool isRifle;
    public bool isShotGun;
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        rifleFirePos = animator.transform.GetChild(0).GetChild(0).GetChild(0).transform;
        shotgunFirePos = animator.transform.GetChild(0).GetChild(1).GetChild(0).transform;

        rifleFlash = rifleFirePos.GetChild(0).GetComponent<ParticleSystem>();
        shotgunFlahs = shotgunFirePos.GetChild(0).GetComponent <ParticleSystem>();

        rifleMesh = rifleFirePos.parent.GetComponent<MeshRenderer>();
        shotgunMesh = shotgunFirePos.parent.GetComponent<MeshRenderer>();

        itemEmptyGroup = GameObject.Find("Item_EmptyGroup").GetComponent<RectTransform>();
        source = GetComponent<AudioSource>();
        rifleClip = Resources.Load<AudioClip>("Sounds/Fires/p_ak_1");
        shotgunClip = Resources.Load<AudioClip>("Sounds/Fires/p_sg_01");
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
        isShotGun = false;
        isRifle = false;
        tabOn = false;
        isReload = false;
        rifleFlash.Stop();
        shotgunFlahs.Stop();
        StartCoroutine(OnFIre());
    }
    IEnumerator OnFIre()
    {
        while(!playerDamage.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            if (isRifle)
                RifleFireAndReload();
            else
                rifleMesh.enabled = false;
            if (isShotGun)
                ShotGunFireAndReload();
            else
                shotgunMesh.enabled = false;

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
    
    private void RifleFireAndReload()
    {
        if (Time.time - curTime > fireTIme && Input.GetMouseButton(0) && isRifle)
        {
            if (!isReload && !tabOn && rifleBulletCount > 0)
            {
                ShootFire(rifleFlash, "RifleFlashStop", rifleBulletCount, rifleClip, gunData.Rf_Count);
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
                ShootFire(shotgunFlahs, "ShotGunFlashStop", shotgunBulletCount,shotgunClip,gunData.Sg_Count);
                shotgunBulletCount--;
                if (shotgunBulletCount == 0 && gunData.Sg_Count > 0)
                    StartCoroutine(ShotGunReload());
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && shotgunBulletCount != gunData.Sg_Count && !isReload & gunData.Sg_SpawnCount > 0 && shotgunBulletCount < 10 && isShotGun)
            StartCoroutine(ShotGunReload());
    }

    public void ChangeShotGun()
    {
        isRifle = false;
        rifleMesh.enabled = false;
        shotgunMesh.enabled = true;
        gunData.g_damage = 50;
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();
    }
    public void ChangeRifle()
    {
        isShotGun = false;
        shotgunMesh.enabled = false;
        rifleMesh.enabled = true;
        gunData.g_damage = 15;
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();
    }
    /*void RifleFire()
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();
        _bullet.transform.position = rifleFirePos.position;
        _bullet.transform.rotation = rifleFirePos.rotation;
        _bullet.SetActive(true);
        rifleFlash.Play();
        Invoke("RifleFlashStop", 0.1f);
        animator.SetTrigger(aniFire);
        --rifleBulletCount;
        source.PlayOneShot(rifleClip, 1.0f);
        bulletText.text = rifleBulletCount.ToString() + " / " + ItemDataBase.itemDataBase.rifleBulletCount.ToString();
    }*/
    /*void ShotGun()
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();
        _bullet.transform.position = shotgunFirePos.position;
        _bullet.transform.rotation = shotgunFirePos.rotation;
        _bullet.SetActive(true);
        shotgunFlahs.Play();
        Invoke("ShotGunFlashStop", 0.1f);
        animator.SetTrigger(aniFire);
        --shotgunBulletCount;
        source.PlayOneShot(shotgunClip, 1.0f);
        bulletText.text = shotgunBulletCount.ToString() + " / " + ItemDataBase.itemDataBase.shotgunBulletCount.ToString();

    }*/
    void ShootFire(ParticleSystem particle, string st, int bulletCount, AudioClip clip, int itemDatabaseCount)
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();
        _bullet.transform.position = shotgunFirePos.position;
        _bullet.transform.rotation = shotgunFirePos.rotation;
        _bullet.SetActive(true);
        particle.Play();
        Invoke(st, 0.1f);
        animator.SetTrigger(aniFire);
        source.PlayOneShot(clip, 1.0f);
        bulletText.text = bulletCount.ToString() + " / " + itemDatabaseCount.ToString();

    }
    IEnumerator RifleReload()
    {
        
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
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
            GameManager.Instance.isRifleBullet = false;
            GameManager.Instance.GetComponent<LoopSpawn>().spawnRfBulletCount--;
        }
    }
    IEnumerator ShotGunReload()
    {
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
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
            GameManager.Instance.isShotGunBullet = false;
            GameManager.Instance.GetComponent<LoopSpawn>().spawnSgBulletCount--;
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
