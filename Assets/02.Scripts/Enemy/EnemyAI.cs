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
    public delegate void EnemyMoveHandler();
    public static event EnemyMoveHandler moveHandler;
    void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        attackDist = 0.1f;
        traceDist = 0.5f;
        isDie = false;
    }
    private void OnEnable()
    {
        StartCoroutine(EnemyScope());
        StartCoroutine(EnemyMotion());  
    }
    IEnumerator EnemyScope()
    {
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
            yield return new WaitForSeconds(0.02f);
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
                    break;
                case State.PATROL:
                    animator.SetBool("IsMove", true);
                    moveHandler();
                    break;
                case State.TRACE:
                    Debug.Log("Trace");
                    break;
                case State.ATTACK:
                    Debug.Log("Attack");
                    break;
                case State.DIE:
                    Debug.Log("Die");
                    break;
            }
        }
    }
    void Update()
    {
        
    }
}
