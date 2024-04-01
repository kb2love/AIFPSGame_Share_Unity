using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { IDLE = 1, PATROL, TRACE, ATTACK, DIE }
    public State state;
    private Transform playerTr;
    private Animator animator;
    private float attackDist;
    private float traceDist;
    public bool isDie;
    public bool isAttack
    {
        get { return _isAttack; }
        set
        {
            _isAttack = value;
            if(_isAttack)
            isAttack = value;
        }
    }
    private bool _isAttack;
    public bool isTrace
    {
        get { return _isTrace; }
        set
        {
            _isTrace = value;
            if(_isTrace)
                _isTrace = value;
        }
    }
    private bool _isTrace;
    public bool isMove
    {
        get { return _isMove; }
        set
        {
            _isMove = value;
            if(_isMove)
                _isMove = value;
        }
    }
    private bool _isMove;
    
    void Start()
    {

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = transform.GetChild(0).GetComponent<Animator>();
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
        _isMove = true;
        _isAttack = false;
        _isTrace = false;
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
            yield return new WaitForSeconds(0.1f);
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
        _isAttack = true;
        isTrace = false;
        isMove = false;
    }

    private void TraceState()
    {
        animator.SetBool("IsMove", true);
        _isAttack = false;
        isTrace = true;
        isMove = false;
    }

    private void PatrolState()
    {
        isTrace = false;
        isMove = true;
        _isAttack = false;
        animator.SetBool("IsMove", true);
    }

    private void IdleState()
    {
        Debug.Log("Idle");
        _isTrace = false;
        _isMove= false;
        _isAttack= false;
        animator.SetBool("IsMove", false);
    }

    public void EnemyDie()
    {
        animator.SetBool("IsMove", false);
        isTrace = false;
        _isAttack = false;
        isMove = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        isDie = true;
        Invoke("OffObject", 2.0f);
    }
    private void OffObject()
    {
        gameObject.SetActive(false);
    }
    void OnDisable()
    {

    }
}
