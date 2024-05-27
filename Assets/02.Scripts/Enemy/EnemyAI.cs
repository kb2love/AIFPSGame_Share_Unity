using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public enum State { PATROL = 1, TRACE, ATTACK, DIE }  // �� ���¸� ��Ÿ���� ������
    public State state;  // ���� �� ����

    private Transform playerTr;  // �÷��̾��� Transform ����
    private Animator animator;  // ���� Animator ������Ʈ ����

    public float attackDist;  // �÷��̾ ������ �Ÿ�
    public float traceDist;  // �÷��̾ ������ �Ÿ�

    private readonly int aniDieTrigger = Animator.StringToHash("DieTrigger");  // ��� �ִϸ��̼� Ʈ���� �ؽ�
    private readonly int aniDieIdx = Animator.StringToHash("DieIdx");  // ��� �ִϸ��̼� �ε��� �ؽ�

    public bool isDie;  // ���� �׾����� ����

    private bool _isAttack;  // isAttack �Ӽ��� ��ŷ �ʵ�
    public bool isAttack
    {
        get { return _isAttack; }  // ���� ���� ��ȯ
        set
        {
            _isAttack = value;
            if (_isAttack)
                isAttack = value;  // ���� ���� ����
        }
    }

    private bool _isTrace;  // isTrace �Ӽ��� ��ŷ �ʵ�
    public bool isTrace
    {
        get { return _isTrace; }  // ���� ���� ��ȯ
        set
        {
            _isTrace = value;
            if (_isTrace)
                _isTrace = value;  // ���� ���� ����
        }
    }

    private bool _isMove;  // isMove �Ӽ��� ��ŷ �ʵ�
    public bool isMove
    {
        get { return _isMove; }  // �̵� ���� ��ȯ
        set
        {
            _isMove = value;
            if (_isMove)
                _isMove = value;  // �̵� ���� ����
        }
    }

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();  // �÷��̾��� Transform ��������
        animator = transform.GetChild(0).GetComponent<Animator>();  // �ڽ� ��ü���� Animator ������Ʈ ��������
        attackDist = 5f;  // ���� �Ÿ� �ʱ�ȭ
        traceDist = 10f;  // ���� �Ÿ� �ʱ�ȭ
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody>().useGravity = true;  // ������ٵ� �߷� ��� ����
        GetComponent<Rigidbody>().isKinematic = false;  // ������ٵ� Ű�׸�ƽ ����
        GetComponent<CapsuleCollider>().isTrigger = false;  // ĸ�� �ݶ��̴� Ʈ���� ����
        state = State.PATROL;  // �ʱ� ���¸� PATROL�� ����
        isDie = false;  // ��� ���� �ʱ�ȭ
        StartCoroutine(EnemyScope());  // EnemyScope �ڷ�ƾ ����
        StartCoroutine(EnemyMotion());  // EnemyMotion �ڷ�ƾ ����
        _isMove = true;  // �̵� ���� �ʱ�ȭ
        _isAttack = false;  // ���� ���� �ʱ�ȭ
        _isTrace = false;  // ���� ���� �ʱ�ȭ
    }

    IEnumerator EnemyScope()
    {
        yield return new WaitForSeconds(1f);  // 1�� ���
        while (!isDie)  // ������� �ʾ��� ��
        {
            float dist = Vector3.Distance(playerTr.position, transform.position);  // �÷��̾���� �Ÿ� ���
            float distY = playerTr.position.y - transform.position.y;  // �÷��̾���� Y�� �Ÿ� ���

            if (dist < attackDist && distY < 0.5f)
            {
                state = State.ATTACK;  // ���� ���·� ��ȯ
                if (distY > 0.5f)
                    state = State.PATROL;  // �÷��̾ ���� ������ ���� ���·� ��ȯ
            }
            else if (dist < traceDist && distY < 0.5f)
            {
                state = State.TRACE;  // ���� ���·� ��ȯ
                if (distY > 0.5f)
                    state = State.PATROL;  // �÷��̾ ���� ������ ���� ���·� ��ȯ
            }
            else
            {
                state = State.PATROL;  // �⺻������ ���� ����
            }
            yield return new WaitForSeconds(0.2f);  // 0.2�� ���
        }
    }

    IEnumerator EnemyMotion()
    {
        while (!isDie)  // ������� �ʾ��� ��
        {
            yield return new WaitForSeconds(0.1f);  // 0.1�� ���
            switch (state)
            {
                case State.PATROL:
                    PatrolState();  // ���� ���� �Լ� ȣ��
                    break;
                case State.TRACE:
                    TraceState();  // ���� ���� �Լ� ȣ��
                    break;
                case State.ATTACK:
                    AttackState();  // ���� ���� �Լ� ȣ��
                    break;
                case State.DIE:
                    StartCoroutine(EnemyDie());  // ��� ���� �Լ� ȣ��
                    break;
            }
        }
    }

    private void AttackState()
    {
        animator.SetBool("IsMove", false);  // �̵� �ִϸ��̼� ��Ȱ��ȭ
        _isAttack = true;  // ���� ���� ����
        _isTrace = false;  // ���� ���� ����
        _isMove = false;  // �̵� ���� ����
    }

    private void TraceState()
    {
        animator.SetBool("IsMove", true);  // �̵� �ִϸ��̼� Ȱ��ȭ
        _isAttack = false;  // ���� ���� ����
        _isTrace = true;  // ���� ���� ����
        _isMove = false;  // �̵� ���� ����
    }

    private void PatrolState()
    {
        _isTrace = false;  // ���� ���� ����
        _isMove = true;  // �̵� ���� ����
        _isAttack = false;  // ���� ���� ����
        animator.SetBool("IsMove", true);  // �̵� �ִϸ��̼� Ȱ��ȭ
    }

    private IEnumerator EnemyDie()
    {
        _isTrace = false;  // ���� ���� ����
        _isAttack = false;  // ���� ���� ����
        _isMove = false;  // �̵� ���� ����
        isDie = true;  // ��� ���� ����
        animator.SetBool("IsMove", false);  // �̵� �ִϸ��̼� ��Ȱ��ȭ
        animator.SetInteger(aniDieIdx, Random.Range(0, 2));  // ������ ��� �ִϸ��̼� ����
        animator.SetTrigger(aniDieTrigger);  // ��� �ִϸ��̼� Ʈ����
        GetComponent<Rigidbody>().useGravity = false;  // ������ٵ� �߷� ����
        GetComponent<Rigidbody>().isKinematic = true;  // ������ٵ� Ű�׸�ƽ ����
        GetComponent<CapsuleCollider>().isTrigger = true;  // ĸ�� �ݶ��̴� Ʈ���� ����
        yield return new WaitForSeconds(3);  // 3�� ���
        ObjectPoolingManager.objPooling.GetComponent<LoopSpawn>().e_Count--;  // ������Ʈ Ǯ�� ī��Ʈ ����
        GameManager.Instance.ScoreUp(1);  // ���� ����
        gameObject.SetActive(false);  // ���� ������Ʈ ��Ȱ��ȭ
    }

    void OnDisable()
    {
        isDie = false;  // ��� ���� �ʱ�ȭ
        attackDist = 5f;  // ���� �Ÿ� �ʱ�ȭ
        traceDist = 10f;  // ���� �Ÿ� �ʱ�ȭ
    }
}
