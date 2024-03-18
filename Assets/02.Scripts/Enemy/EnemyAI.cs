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
    EnemyMove enemyMove;
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
                if (enemyFOV.IsViewPlayer())
                    state = State.ATTACK;
                else
                    state = State.TRACE;
            }
            else if(enemyFOV.IsTracePlayer())
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
                    animator.SetBool("IsMove", false);
                    break;
                case State.PATROL:
                    enemyMove.isTrace = false;
                    animator.SetBool("IsMove", true);
                    moveHandler();
                    break;
                case State.TRACE:
                    Debug.Log("Trace");
                    animator.SetBool("IsMove", true);
                    playerTraceHandler();
                    break;
                case State.ATTACK:
                    Debug.Log("Attack");
                    enemyMove.isTrace = false;
                    break;
                case State.DIE:
                    Debug.Log("Die");
                    break;
            }
        }
    }

}
