using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    public enum State { PATROL = 1, TRACE, ATTACK, DIE }  // 적 상태를 나타내는 열거형
    public State state;  // 현재 적 상태

    private Transform playerTr;  // 플레이어의 Transform 참조
    private Animator animator;  // 적의 Animator 컴포넌트 참조

    public float attackDist;  // 플레이어를 공격할 거리
    public float traceDist;  // 플레이어를 추적할 거리

    private readonly int aniDieTrigger = Animator.StringToHash("DieTrigger");  // 사망 애니메이션 트리거 해시
    private readonly int aniDieIdx = Animator.StringToHash("DieIdx");  // 사망 애니메이션 인덱스 해시

    public bool isDie;  // 적이 죽었는지 여부

    private bool _isAttack;  // isAttack 속성의 백킹 필드
    public bool isAttack
    {
        get { return _isAttack; }  // 공격 여부 반환
        set
        {
            _isAttack = value;
            if (_isAttack)
                isAttack = value;  // 공격 여부 설정
        }
    }

    private bool _isTrace;  // isTrace 속성의 백킹 필드
    public bool isTrace
    {
        get { return _isTrace; }  // 추적 여부 반환
        set
        {
            _isTrace = value;
            if (_isTrace)
                _isTrace = value;  // 추적 여부 설정
        }
    }

    private bool _isMove;  // isMove 속성의 백킹 필드
    public bool isMove
    {
        get { return _isMove; }  // 이동 여부 반환
        set
        {
            _isMove = value;
            if (_isMove)
                _isMove = value;  // 이동 여부 설정
        }
    }

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();  // 플레이어의 Transform 가져오기
        animator = transform.GetChild(0).GetComponent<Animator>();  // 자식 객체에서 Animator 컴포넌트 가져오기
        attackDist = 5f;  // 공격 거리 초기화
        traceDist = 10f;  // 추적 거리 초기화
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody>().useGravity = true;  // 리지드바디 중력 사용 설정
        GetComponent<Rigidbody>().isKinematic = false;  // 리지드바디 키네마틱 해제
        GetComponent<CapsuleCollider>().isTrigger = false;  // 캡슐 콜라이더 트리거 해제
        state = State.PATROL;  // 초기 상태를 PATROL로 설정
        isDie = false;  // 사망 상태 초기화
        StartCoroutine(EnemyScope());  // EnemyScope 코루틴 시작
        StartCoroutine(EnemyMotion());  // EnemyMotion 코루틴 시작
        _isMove = true;  // 이동 여부 초기화
        _isAttack = false;  // 공격 여부 초기화
        _isTrace = false;  // 추적 여부 초기화
    }

    IEnumerator EnemyScope()
    {
        yield return new WaitForSeconds(1f);  // 1초 대기
        while (!isDie)  // 사망하지 않았을 때
        {
            float dist = Vector3.Distance(playerTr.position, transform.position);  // 플레이어와의 거리 계산
            float distY = playerTr.position.y - transform.position.y;  // 플레이어와의 Y축 거리 계산

            if (dist < attackDist && distY < 0.5f)
            {
                state = State.ATTACK;  // 공격 상태로 전환
                if (distY > 0.5f)
                    state = State.PATROL;  // 플레이어가 위에 있으면 순찰 상태로 전환
            }
            else if (dist < traceDist && distY < 0.5f)
            {
                state = State.TRACE;  // 추적 상태로 전환
                if (distY > 0.5f)
                    state = State.PATROL;  // 플레이어가 위에 있으면 순찰 상태로 전환
            }
            else
            {
                state = State.PATROL;  // 기본적으로 순찰 상태
            }
            yield return new WaitForSeconds(0.2f);  // 0.2초 대기
        }
    }

    IEnumerator EnemyMotion()
    {
        while (!isDie)  // 사망하지 않았을 때
        {
            yield return new WaitForSeconds(0.1f);  // 0.1초 대기
            switch (state)
            {
                case State.PATROL:
                    PatrolState();  // 순찰 상태 함수 호출
                    break;
                case State.TRACE:
                    TraceState();  // 추적 상태 함수 호출
                    break;
                case State.ATTACK:
                    AttackState();  // 공격 상태 함수 호출
                    break;
                case State.DIE:
                    StartCoroutine(EnemyDie());  // 사망 상태 함수 호출
                    break;
            }
        }
    }

    private void AttackState()
    {
        animator.SetBool("IsMove", false);  // 이동 애니메이션 비활성화
        _isAttack = true;  // 공격 여부 설정
        _isTrace = false;  // 추적 여부 해제
        _isMove = false;  // 이동 여부 해제
    }

    private void TraceState()
    {
        animator.SetBool("IsMove", true);  // 이동 애니메이션 활성화
        _isAttack = false;  // 공격 여부 해제
        _isTrace = true;  // 추적 여부 설정
        _isMove = false;  // 이동 여부 해제
    }

    private void PatrolState()
    {
        _isTrace = false;  // 추적 여부 해제
        _isMove = true;  // 이동 여부 설정
        _isAttack = false;  // 공격 여부 해제
        animator.SetBool("IsMove", true);  // 이동 애니메이션 활성화
    }

    private IEnumerator EnemyDie()
    {
        _isTrace = false;  // 추적 여부 해제
        _isAttack = false;  // 공격 여부 해제
        _isMove = false;  // 이동 여부 해제
        isDie = true;  // 사망 상태 설정
        animator.SetBool("IsMove", false);  // 이동 애니메이션 비활성화
        animator.SetInteger(aniDieIdx, Random.Range(0, 2));  // 랜덤한 사망 애니메이션 설정
        animator.SetTrigger(aniDieTrigger);  // 사망 애니메이션 트리거
        GetComponent<Rigidbody>().useGravity = false;  // 리지드바디 중력 해제
        GetComponent<Rigidbody>().isKinematic = true;  // 리지드바디 키네마틱 설정
        GetComponent<CapsuleCollider>().isTrigger = true;  // 캡슐 콜라이더 트리거 설정
        yield return new WaitForSeconds(3);  // 3초 대기
        ObjectPoolingManager.objPooling.GetComponent<LoopSpawn>().e_Count--;  // 오브젝트 풀링 카운트 감소
        GameManager.Instance.ScoreUp(1);  // 점수 증가
        gameObject.SetActive(false);  // 게임 오브젝트 비활성화
    }

    void OnDisable()
    {
        isDie = false;  // 사망 상태 초기화
        attackDist = 5f;  // 공격 거리 초기화
        traceDist = 10f;  // 추적 거리 초기화
    }
}
