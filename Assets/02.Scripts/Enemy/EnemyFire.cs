using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private Transform firePos;
    private ParticleSystem shotFlash;
    private Animator animator;
    readonly int Anireload = Animator.StringToHash("ReloadTrigger");
    readonly int Anifire = Animator.StringToHash("FireTrigger");
    private int bulletCount;
    private int maxBulletCount;
    public bool isAttack;
    private bool isReload;
    private EnemyAI enemyAI;
    void Start()
    {
        maxBulletCount = 20;
        bulletCount = maxBulletCount;
    }
    private void OnEnable()
    {
        isAttack = false;
        isReload = false;
        animator = transform.GetChild(0).GetComponent<Animator>();
        firePos = transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<Transform>();
        shotFlash = firePos.transform.GetChild(0).GetComponent<ParticleSystem>();
        enemyAI = GetComponent<EnemyAI>();
        shotFlash.Stop();
        StartCoroutine(OnFIre());
    }
    IEnumerator OnFIre()
    {
        while(!enemyAI.isDie)
        {
            yield return new WaitForSeconds(0.5f);
            while (isAttack)
            {
                yield return new WaitForSeconds(0.2f);
                if(!isReload)
                {
                    Fire();
                }
            }
        }
    }
    void Fire()
    {
        --bulletCount;
        GameObject e_bullet = ObjectPoolingManager.objPooling.GetEnemyBullet();
        e_bullet.transform.position = firePos.position;
        e_bullet.transform.rotation = firePos.rotation;
        e_bullet.SetActive(true);
        animator.SetTrigger(Anifire);
        shotFlash.Play();
        Invoke("ShootFlashStop", 0.1f);
        if(bulletCount <= 0)
        {
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        isReload = true;
        animator.SetTrigger(Anireload);
        yield return new WaitForSeconds(2.0f);
        isReload = false;
        bulletCount = maxBulletCount;
    }
    private void ShootFlashStop()
    {
        shotFlash.Stop();
    }
}
