using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private Animator animator;
    private string bulletTag = "Bullet";
    private int hp;
    private int maxHp;
    private readonly int aniE_Hit = Animator.StringToHash("EnemyHit");
    GameObject _effect;
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHp = 100;
        hp = maxHp;
        _effect = ObjectPoolingManager.objPooling.GetHitEffect();
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            hp -= col.gameObject.GetComponent<BulletCtlr>().damage;
            _effect.transform.position = col.transform.position;
            _effect.transform.rotation = col.transform.rotation;
            _effect.SetActive(true);
            Invoke("EffectOff", 0.3f);
            animator.SetTrigger(aniE_Hit);
        }
    }
    private void EffectOff()
    {
        _effect.SetActive(false);
    }
}
