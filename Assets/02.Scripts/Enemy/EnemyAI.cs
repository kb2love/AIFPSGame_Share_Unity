using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { IDLE = 1, PATROL, TRACE, ATTACK, DIE }
    public State state;
    private Transform playerTr;
    private Animator animator;
    public float attackDist;
    public float traceDist;
    private readonly int aniDieTrigger = Animator.StringToHash("DieTrigger");
    private readonly int aniDieIdx = Animator.StringToHash("DieIdx");
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
        state = State.PATROL;
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
                    StartCoroutine(EnemyDie());
                    break;
            }
        }
    }

    private void AttackState()
    {
        animator.SetBool("IsMove", false);
        _isAttack = true;
        _isTrace = false;
        _isMove = false;
    }

    private void TraceState()
    {
        animator.SetBool("IsMove", true);
        _isAttack = false;
        _isTrace = true;
        _isMove = false;
    }

    private void PatrolState()
    {
        _isTrace = false;
        _isMove = true;
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

    private IEnumerator EnemyDie()
    {
        _isTrace = false;
        _isAttack = false;
        _isMove = false;
        isDie = true;
        animator.SetBool("IsMove", false);
        animator.SetInteger(aniDieIdx, Random.Range(0, 2));
        animator.SetTrigger(aniDieTrigger);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        yield return new WaitForSeconds(3);
        GameManager.Instance.GetComponent<LoopSpawn>().e_Count--;
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        isDie = false;
        attackDist = 5f;
        traceDist = 10f;
    }
}
