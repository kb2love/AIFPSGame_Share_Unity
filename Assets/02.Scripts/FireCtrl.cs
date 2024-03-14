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
    private readonly int aniIsReload = Animator.StringToHash("IsReload");
    private int bulletCount;
    private int bulletMaxCount;
    private bool isReload;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    IEnumerator OnFIre()
    {
        while(!playerDamage.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            if (Time.time - curTime > fireTIme && Input.GetMouseButton(0))
            {
                if(!isReload)
                {
                    Fire();
                    if (bulletCount == 0)
                        StartCoroutine(Reload());
                }
                curTime = Time.time;
            }
            if(Input.GetKeyDown(KeyCode.R) && bulletCount != bulletMaxCount &&!isReload)
                StartCoroutine(Reload());
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
    }
    IEnumerator Reload()
    {
        isReload = true;
        animator.SetTrigger(aniReload);
        animator.SetBool(aniIsReload, true);
        yield return new WaitForSeconds(1.55f);
        animator.SetBool(aniIsReload, false);
        isReload = false;
        bulletCount = bulletMaxCount;

    }
}
