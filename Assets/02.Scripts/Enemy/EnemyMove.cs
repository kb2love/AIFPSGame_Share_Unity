using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;                       //에너미 데이타 스크립터블오브젝트
    private Transform[] stairsPoint;                            //계단에 도착했을때 계단을 순차적으로 이동하게 하기위한 트랜스폼
    private Transform playerTr;                                 //플레이어 포지션
    private List<Transform> stairList = new List<Transform>();  //계단 트랜스폼을 가져와서 사용할 리스트
    private EnemyAI enemyAI;                                    //Enemy의 상태를 가져오기 위한 변수
    private int nextIdx = 0;                                    //계단의 순서를 나타낼 Idx
    private float starDist;                                     //계단과 Enemy사이의 거리를 나타낼 변수
    private Vector3 disOne;                                     //계단으로 갈 방향
    private bool isStair;                                       //계단을 걷는걸 확인할 불변수
    private float RayDistance;                                  //Enemy가 벽/오브젝트에 부딪히기 전의 거리
    private float rayPer;                                       //Enemy의 회전정도를 나눌 변수
    private RaycastHit frontHit;                                //Enemy의 앞으로 쏠 레이케스트
    private RaycastHit rightHit;                                //Enemy의 오른쪽으로 쏠 레이케스트
    private RaycastHit leftHit;                                 //Enemy의 왼쪽으로 쏠 레이케스트
    void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        stairsPoint = GameObject.Find("StairPoints").GetComponentsInChildren<Transform>();
        enemyAI = GetComponent<EnemyAI>();
        for (int i = 1; i < stairsPoint.Length; i++)                 //스테어포인트에 스테어포인트를 담는 그룹부모 오브젝트까지 가져오기 때문에 부모오브젝트를 빼준다   
        {
            stairList.Add(stairsPoint[i]);
        }
        rayPer = 2;
        RayDistance = 1;
    }
    private void OnEnable()
    {
        isStair = false;                                            //태어날때는 계단을 오르는 상태가 아님
    }
    void Update()
    {
        if (enemyAI.isDie) return;                                  //죽으면 리턴시켜서 움직임을 멈추게한다
        if (enemyAI.isMove)                                         //주변에 Player가 없으면 기본적인 움직임을 한다
        {
            EnemyVaseMove();
        }
        else if (enemyAI.isTrace)                                   //주변에 플레이어가 있으면 플레이어를 따라간다
        {
            OnPlayerTrace();
        }
        else if (enemyAI.isAttack)                                  //공격반경에 플레이어가 있으면 총을쏜다
        {
            Quaternion cRot = Quaternion.LookRotation(playerTr.position - transform.position);      //플레이어와 나사이의 거리를 구해서 
            cRot.z = cRot.x = 0;    
            transform.rotation = Quaternion.Slerp(transform.rotation, cRot, 10f * Time.deltaTime);  //플레이어의 방향으로 Enemy를 회전시킨다

        }
    }
    public void EnemyVaseMove()    //계단이 주변에 없다면 RayCastMove를 하고 계단이 주변에있다면 계단을 오르는 메서드
    {
        disOne = (stairList[nextIdx].position - transform.position).normalized;                     //계단의 위치와 나의 위치를 확인하여 방향을 구한다
        starDist = Vector3.Distance(stairList[nextIdx].position, transform.position);               //계단의 위치와 나의 위치를 확인하여 거리를 구한다

        float distTwo = Vector3.Distance(stairList[3].position, transform.position);                //두번째 계단위치와 나의 위치를 계산한 거리
        float distTree = Vector3.Distance(stairList[nextIdx].position, transform.position);         //세번째 계단위치와 나의 위치를 계산한 거리
        if (distTree <= 3f)                                                         //계단과 Enemy의 거리가 3이라면
        {
            isStair = true;                                                         //계단을 건넌다
        }
        if (!isStair)                                                               //그게 아니라면 레이로 움직이는 메소드를 활성화 시킨다
        {
            RayMove();

        }
        else if (isStair)                                                           //계단과 Enemy의 사이가 3이라면 계단을 건넌다
        {
            transform.Translate(disOne * enemyData.e_MoveSpeed * Time.deltaTime);
        }
        if (starDist <= 0.5f)                                                       //계단위치에 도착했다면
        {

            ++nextIdx;                                                              //Idx를 더해줘 계단을 다올랐는지 확인한다
            if (nextIdx >= 4)
            {
                nextIdx = 3;                                                        
            }
        }
        if (distTwo <= 0.5f || distTree > 7)                                        //계단을 다 올랐다면 계단오르기를 비활성화시켜주고 Idx를 0으로 초기화시켜준다
        {
            isStair = false;
            nextIdx = 0;
        }
    }
    private void RayMove()                              //네비게이션 매쉬를 사용안하고 벽/오브젝트에 부딪히기 전에 방향을 틀어 자동적으로 움직이게 하는 메서드
    {
        Vector3 caracRot = transform.localEulerAngles;  //회전을 할떄 y값만 변경될 수있도록 하게 하는 변수
        caracRot.x = caracRot.z = 0f;
        transform.localEulerAngles = caracRot;
        //frontDir = transform.TransformDirection(frontDir);
        transform.Translate(transform.forward * Time.deltaTime * enemyData.e_MoveSpeed);    //기본적으로 앞으로만 이동한다

        //레이를 쏠 위치(레이를 바닥쪽에서 쏘게되면 작은 턱에도 걸리기 때문에 레이를 바닥보다 조금 위쪽에서 쏴준다) 
        Vector3 rayHeight = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if (Physics.Raycast(rayHeight, -transform.right, out leftHit, RayDistance))     
        {
            RayCastMove(rayHeight, -transform.right, leftHit, transform.right);
        }
        else if (Physics.Raycast(rayHeight, transform.right, out rightHit, RayDistance))
        {
            RayCastMove(rayHeight, transform.right, rightHit, -transform.right);
        }
        else if (Physics.Raycast(rayHeight, transform.forward, out frontHit, RayDistance))
        {
            RayCastMove(rayHeight, transform.forward, frontHit, frontHit.normal);
        }
    }

    private void RayCastMove(Vector3 rayHeight, Vector3 dis, RaycastHit hit, Vector3 rDis)
    {
        //레이를 쏴서 쏜 방향에 오브젝트/건물이 없는지 확인하고 있다면 그반대방향으로 회전한다

        if (!Physics.Raycast(rayHeight, dis, out hit, RayDistance)) return;
        Quaternion rot = Quaternion.LookRotation(rDis / rayPer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
    }

    public void OnPlayerTrace()
    {
        //플레이어를 쫓아가는 메서드
        Vector3 plDis = (playerTr.position - transform.position).normalized;
        transform.Translate(plDis * enemyData.e_MoveSpeed * Time.deltaTime);
        Quaternion plRot = Quaternion.LookRotation(playerTr.position - transform.position);
        plRot.z = plRot.x = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, plRot, 10f * Time.deltaTime);
    }
}

