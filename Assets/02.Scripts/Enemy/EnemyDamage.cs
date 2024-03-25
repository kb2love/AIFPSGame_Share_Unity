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
    private EnemyAI enemyAI;
    private GameObject _effect;
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHp = 100;
        hp = maxHp;
        _effect = ObjectPoolingManager.objPooling.GetHitEffect();
        enemyAI = GetComponent<EnemyAI>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            hp -= (int)col.gameObject.GetComponent<BulletCtlr>().damage;
            Vector3 normal = col.contacts[0].normal;
            _effect.transform.position = col.contacts[0].point;
            _effect.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, normal);
            _effect.SetActive(true);
            Invoke("EffectOff", 1f);
            animator.SetTrigger(aniE_Hit);
            if(hp <= 0)
            {
                enemyAI.EnemyDie();
            }
        }
    }
    private void EffectOff()
    {
        _effect.SetActive(false);
    }
}
