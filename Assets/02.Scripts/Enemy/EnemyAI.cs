using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { IDLE =1, PATROL, TRACE, ATTACK, DIE }
    public State state;
    private Transform playerTr;
    private Animator animator;
    private float attackDist;
    private float traceDist;
    public bool isDie;
    private EnemyFire enemyFire;
    private EnemyMove enemyMove;
    
    void Start()
    {

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>();
        enemyMove = GetComponent<EnemyMove>();
        attackDist = 5f;
        traceDist = 10f;
    }
    private void OnEnable()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().isTrigger = false;
        isDie = false;
        StartCoroutine(EnemyScope());
        StartCoroutine(EnemyMotion());  
    }
    IEnumerator EnemyScope()
    {
        yield return new WaitForSeconds(1f);
        while(!isDie)
        {
            float dist = Vector3.Distance(playerTr.position, transform.position);
            if(dist < attackDist)
            {
                    state = State.ATTACK;
            }
            else if(dist < traceDist && enemyMove.isTrace)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator EnemyMotion()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.002f);
            switch(state)
            {
                case State.IDLE:
                    IdleState();
                    break;
                case State.PATROL:
                    PatrolState();
                    break;
                case State.TRACE:
                    TraceState();
                    break;
                case State.ATTACK:
                    AttackState();
                    break;
                case State.DIE:
                    EnemyDie();
                    break;
            }
        }
    }

    private void AttackState()
    {
        animator.SetBool("IsMove", false);
        enemyMove.isTrace = false;
        enemyFire.isAttack = true;

        Vector3 rot = playerTr.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rot), 10f * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    private void TraceState()
    {
        animator.SetBool("IsMove", true);
        enemyMove.OnPlayerTrace();
        enemyFire.isAttack = false;
    }

    private void PatrolState()
    {
        enemyMove.isTrace = false;
        enemyFire.isAttack = false;
        animator.SetBool("IsMove", true);
        enemyMove.RacastStairs();
    }

    private void IdleState()
    {
        Debug.Log("Idle");
        enemyMove.isTrace = false;
        enemyFire.isAttack = false;
        animator.SetBool("IsMove", false);
    }

    public void EnemyDie()
    {
        animator.SetBool("IsMove", false);
        enemyMove.isTrace = false;
        enemyFire.isAttack = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        isDie = true;
        Invoke("OffObject", 2.0f);
    }
    private void OffObject()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GetComponent<LoopSpawn>().enemySpawnCount--;
    }
    void OnDisable()
    {

    }
}
