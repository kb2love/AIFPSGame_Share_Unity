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
    public delegate void EnemyMoveHandler();
    public static event EnemyMoveHandler moveHandler;
    public delegate void PlayerTraceHandler();
    public static event PlayerTraceHandler playerTraceHandler;
    
    private EnemyFOV enemyFOV;
    void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        enemyFOV = GetComponent<EnemyFOV>();
        enemyFire = GetComponent<EnemyFire>();
        attackDist = 5f;
        traceDist = 10f;
        isDie = false;
        enemyMove = GetComponent<EnemyMove>();
    }
    private void OnEnable()
    {
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
            else if(dist < traceDist)
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
                    Debug.Log("Idle");
                    enemyMove.isTrace = false;
                    enemyFire.isAttack = false;
                    animator.SetBool("IsMove", false);
                    break;
                case State.PATROL:
                    enemyMove.isTrace = false;
                    enemyFire.isAttack = false;
                    animator.SetBool("IsMove", true);
                    moveHandler();
                    break;
                case State.TRACE:
                    animator.SetBool("IsMove", true);
                    playerTraceHandler();
                    enemyFire.isAttack = false;
                    break;
                case State.ATTACK:
                    animator.SetBool("IsMove", false);
                    enemyMove.isTrace = false;
                    enemyFire.isAttack = true;
                    Vector3 rot = playerTr.position - transform.position;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rot), 10f * Time.deltaTime);
                    break;
                case State.DIE:
                    EnemyDie();
                    break;
            }
        }
    }
    public void EnemyDie()
    {
        state = State.DIE;
        Debug.Log("Die");
        enemyMove.isTrace = false;
        enemyFire.isAttack = false;
        GameManager.Instance.GetComponent<LoopSpawn>().enemySpawnCount--;
    }
}
