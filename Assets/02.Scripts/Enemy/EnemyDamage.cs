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
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHp = 100;
        hp = maxHp;
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            hp -= col.gameObject.GetComponent<BulletCtlr>().damage;
            animator.SetTrigger(aniE_Hit);
        }
    }
}
