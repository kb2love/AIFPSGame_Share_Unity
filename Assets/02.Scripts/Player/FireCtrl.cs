using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    private Transform firePos;
    private AudioSource source;
    private AudioClip fireClip;
    private Animator animator;
    private PlayerDamage playerDamage;
    private ParticleSystem fireFlash;
    private CanvasGroup canvasGroup;
    private Image weaponImage;
    public Text bulletText;
    private GetItem getItem;
    private bool tabOn;
    private float curTime;
    private float fireTIme;
    private readonly int aniFire = Animator.StringToHash("FireTrigger");
    private readonly int aniReload = Animator.StringToHash("ReloadTrigger");
    private readonly int aniIsReload = Animator.StringToHash("IsReload");
    public int bulletCount;
    public int bulletValue;
    private int bulletMaxCount;
    private bool isReload;

    void Start()
    {
        
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        fireFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerDamage = GetComponent<PlayerDamage>();
        canvasGroup = GameObject.Find("Canvas_ui").transform.GetChild(0).GetComponent<CanvasGroup>();
        weaponImage = GameObject.Find("Panel-Weapon").transform.GetChild(0).GetComponent<Image>();
        bulletText = weaponImage.transform.parent.GetChild(1).GetComponent<Text>();
        getItem = GetComponent<GetItem>();
        curTime = Time.time;
        fireTIme = 0.1f;
        isReload = false;
        bulletValue = 0;
        bulletMaxCount = 30;
        bulletCount = bulletValue;
        fireClip = Resources.Load<AudioClip>("Sounds/Fires/p_ak_1");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        tabOn = false;
        fireFlash.Stop();
        StartCoroutine(OnFIre());
    }
    IEnumerator OnFIre()
    {
        while(!playerDamage.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            FireAndReload();
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

    private void FireAndReload()
    {
        if (Time.time - curTime > fireTIme && Input.GetMouseButton(0))
        {
            if (!isReload && !tabOn & bulletCount > 0)
            {
                Fire();
                if (bulletCount == 0 && bulletValue > 0)
                    StartCoroutine(Reload());
            }
            curTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletCount != bulletValue && !isReload && bulletValue > 0 && bulletCount < 30)
            StartCoroutine(Reload());
    }

    void Fire()
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();
        _bullet.transform.position = firePos.position;
        _bullet.transform.rotation = firePos.rotation;
        _bullet.SetActive(true);
        fireFlash.Play();
        Invoke("FlashStop", 0.1f);
        animator.SetTrigger(aniFire);
        --bulletCount;
        source.PlayOneShot(fireClip, 1.0f);
        bulletText.text = bulletCount.ToString() + " / " + bulletValue.ToString();
    }
    IEnumerator Reload()
    {
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
        animator.SetBool(aniIsReload, false);
        isReload = false;
        if(bulletValue >= bulletMaxCount)
        {
            if(bulletCount == 0)
            {
                bulletCount += bulletMaxCount;
                bulletValue -= bulletCount;
            }
            else if(bulletCount > 0)
            {
                int cot = bulletMaxCount;
                cot -= bulletCount;
                bulletCount += cot;
                bulletValue -= cot;
            }
        }
        else if(bulletValue < bulletMaxCount)
        {
            if(bulletCount == 0)
            {
                bulletCount += bulletValue;
                bulletValue -= bulletCount; 
            }
            else if(bulletCount > 0)
            { 
                int let = bulletMaxCount;
                let -= bulletCount;
                if(bulletValue >= let)
                {
                    bulletCount += let;
                    bulletValue -= let;
                }
                else if( bulletValue < let)
                {
                    bulletCount += bulletValue;
                    bulletValue -= bulletValue;
                }
            }
            
        }
        GameManager.Instance.itemEmptText.text = bulletValue.ToString();
        bulletText.text = bulletCount.ToString() + " / " + bulletValue.ToString();
    }
    void FlashStop()
    {
        fireFlash.Stop();
    }
}
