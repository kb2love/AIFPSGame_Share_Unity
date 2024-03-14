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
    void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        attackDist = 20f;
        traceDist = 60f;
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
            yield return new WaitForSeconds(0.02f);
            switch(state)
            {
                case State.IDLE:

                    break;
                case State.PATROL:

                    break;
                case State.TRACE:

                    break;
                case State.ATTACK:

                    break;
                case State.DIE:

                    break;
            }
        }
    }
    void Update()
    {
        
    }
}
