using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    private Transform firePos;
    private GameObject bullet;
    private AudioSource source;
    private AudioClip fireClip;
    private Animator animator;
    private PlayerDamage playerDamage;
    private float curTime;
    private float fireTIme;
    private readonly int aniFire = Animator.StringToHash("FireTrigger");
    private readonly int aniReload = Animator.StringToHash("ReloadTrigger");
    private int bulletCount;
    private int bulletMaxCount;
    private bool isReload;

    void Start()
    {
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        bullet = Resources.Load<GameObject>("Weapon/Bullet");
        source = GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerDamage = GetComponent<PlayerDamage>();
        curTime = Time.time;
        fireTIme = 0.1f;
        isReload = false;
        bulletMaxCount = 20;
        bulletCount = bulletMaxCount;
        fireClip = Resources.Load<AudioClip>("Sounds/Fires/p_ak_1");
        StartCoroutine(OnFIre());
    }

    void Update()
    {
        
    }
    IEnumerator OnFIre()
    {
        while(playerDamage.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            if (Input.GetMouseButton(0) && !isReload && Time.time - curTime > fireTIme)
            {
                Fire();
                curTime = Time.time;
            }
        }
    }
    void Fire()
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();
        _bullet.transform.position = firePos.position;
        _bullet.transform.rotation = firePos.rotation;
        _bullet.SetActive(true);
        animator.SetTrigger(aniFire);
        --bulletCount;
        source.PlayOneShot(fireClip, 1.0f);
        if(bulletCount == 0)
            StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        isReload = true;
        animator.SetTrigger(aniReload);
        yield return new WaitForSeconds(2.0f);
        isReload = false;
        bulletCount = bulletMaxCount;
    }
}
