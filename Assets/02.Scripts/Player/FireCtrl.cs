using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    private Transform rifleFirePos;
    private Transform shotgunFirePos;
    private AudioSource source;
    private AudioClip rifleClip;
    private Animator animator;
    private PlayerDamage playerDamage;
    private ParticleSystem rifleFlash;
     private ParticleSystem shotgunFlahs;
    private CanvasGroup canvasGroup;
    private Image weaponImage;
    public Text rifleBulletText;
    private GetItem getItem;
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
    public int rilfeBulletValue;
    private int rifleBulletMaxCount;
    private bool isReload;
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
        canvasGroup = GameObject.Find("Canvas_ui").transform.GetChild(0).GetComponent<CanvasGroup>();
        weaponImage = GameObject.Find("Panel-Weapon").transform.GetChild(0).GetComponent<Image>();
        rifleBulletText = weaponImage.transform.parent.GetChild(1).GetComponent<Text>();
        playerDamage = GetComponent<PlayerDamage>();
        getItem = GetComponent<GetItem>();

        curTime = Time.time;
        fireTIme = 0.1f;

        rilfeBulletValue = 0;
        rifleBulletMaxCount = 30;
        rifleBulletCount = rilfeBulletValue;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isShotGun = false;
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
            if(!isShotGun)
                RifleFireAndReload();
            else 
                ShotGunFireAndReload();

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
    private void ShotGunFireAndReload()
    {
        rifleMesh.enabled = false;
        shotgunMesh.enabled = true;
        if(Input.GetMouseButton(0))
        {
            
        }
    }
    private void RifleFireAndReload()
    {
        shotgunMesh.enabled = false;
        rifleMesh.enabled = true;
        if (Time.time - curTime > fireTIme && Input.GetMouseButton(0))
        {
            if (!isReload && !tabOn & rifleBulletCount > 0)
            {
                RifleFire();
                if (rifleBulletCount == 0 && rilfeBulletValue > 0)
                    StartCoroutine(RifleReload());
            }
            curTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.R) && rifleBulletCount != rilfeBulletValue && !isReload && rilfeBulletValue > 0 && rifleBulletCount < 30)
            StartCoroutine(RifleReload());
    }

    void RifleFire()
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
        rifleBulletText.text = rifleBulletCount.ToString() + " / " + rilfeBulletValue.ToString();
    }
    IEnumerator RifleReload()
    {
        
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
        animator.SetBool(aniIsReload, false);
        isReload = false;
        if(rilfeBulletValue >= rifleBulletMaxCount)
        {
            if(rifleBulletCount == 0)
            {
                rifleBulletCount += rifleBulletMaxCount;
                rilfeBulletValue -= rifleBulletCount;
            }
            else if(rifleBulletCount > 0)
            {
                int cot = rifleBulletMaxCount;
                cot -= rifleBulletCount;
                rifleBulletCount += cot;
                rilfeBulletValue -= cot;
            }
        }
        else if(rilfeBulletValue < rifleBulletMaxCount)
        {
            if(rifleBulletCount == 0)
            {
                rifleBulletCount += rilfeBulletValue;
                rilfeBulletValue -= rifleBulletCount; 
            }
            else if(rifleBulletCount > 0)
            { 
                int let = rifleBulletMaxCount;
                let -= rifleBulletCount;
                if(rilfeBulletValue >= let)
                {
                    rifleBulletCount += let;
                    rilfeBulletValue -= let;
                }
                else if( rilfeBulletValue < let)
                {
                    rifleBulletCount += rilfeBulletValue;
                    rilfeBulletValue -= rilfeBulletValue;
                }
            }
            
        }
        /*GameManager.Instance.itemEmptyObject.text = bulletValue.ToString();*/
        rifleBulletText.text = rifleBulletCount.ToString() + " / " + rilfeBulletValue.ToString();
        GameManager.Instance.itemEmptyText[GameManager.Instance.bulletIdx].text = rilfeBulletValue.ToString();
        if(rilfeBulletValue == 0)
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.bulletIdx].SetParent(itemEmptyGroup);
            GameManager.Instance.itemEmptyImageList[GameManager.Instance.bulletIdx].enabled = false;
            GameManager.Instance.itemEmptyText[GameManager.Instance.bulletIdx].enabled = false;
            GameManager.Instance.isBullet = false;
        }
    }
    void RifleFlashStop()
    {
        rifleFlash.Stop();
    }
}
