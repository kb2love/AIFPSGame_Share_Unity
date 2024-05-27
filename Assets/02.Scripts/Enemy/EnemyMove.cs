using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;                       //���ʹ� ����Ÿ ��ũ���ͺ������Ʈ
    private Transform[] stairsPoint;                            //��ܿ� ���������� ����� ���������� �̵��ϰ� �ϱ����� Ʈ������
    private Transform playerTr;                                 //�÷��̾� ������
    private List<Transform> stairList = new List<Transform>();  //��� Ʈ�������� �����ͼ� ����� ����Ʈ
    private EnemyAI enemyAI;                                    //Enemy�� ���¸� �������� ���� ����
    private int nextIdx = 0;                                    //����� ������ ��Ÿ�� Idx
    private float starDist;                                     //��ܰ� Enemy������ �Ÿ��� ��Ÿ�� ����
    private Vector3 disOne;                                     //������� �� ����
    private bool isStair;                                       //����� �ȴ°� Ȯ���� �Һ���
    private float RayDistance;                                  //Enemy�� ��/������Ʈ�� �ε����� ���� �Ÿ�
    private float rayPer;                                       //Enemy�� ȸ�������� ���� ����
    private RaycastHit frontHit;                                //Enemy�� ������ �� �����ɽ�Ʈ
    private RaycastHit rightHit;                                //Enemy�� ���������� �� �����ɽ�Ʈ
    private RaycastHit leftHit;                                 //Enemy�� �������� �� �����ɽ�Ʈ
    void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        stairsPoint = GameObject.Find("StairPoints").GetComponentsInChildren<Transform>();
        enemyAI = GetComponent<EnemyAI>();
        for (int i = 1; i < stairsPoint.Length; i++)                 //���׾�����Ʈ�� ���׾�����Ʈ�� ��� �׷�θ� ������Ʈ���� �������� ������ �θ������Ʈ�� ���ش�   
        {
            stairList.Add(stairsPoint[i]);
        }
        rayPer = 2;
        RayDistance = 1;
    }
    private void OnEnable()
    {
        isStair = false;                                            //�¾���� ����� ������ ���°� �ƴ�
    }
    void Update()
    {
        if (enemyAI.isDie) return;                                  //������ ���Ͻ��Ѽ� �������� ���߰��Ѵ�
        if (enemyAI.isMove)                                         //�ֺ��� Player�� ������ �⺻���� �������� �Ѵ�
        {
            EnemyVaseMove();
        }
        else if (enemyAI.isTrace)                                   //�ֺ��� �÷��̾ ������ �÷��̾ ���󰣴�
        {
            OnPlayerTrace();
        }
        else if (enemyAI.isAttack)                                  //���ݹݰ濡 �÷��̾ ������ �������
        {
            Quaternion cRot = Quaternion.LookRotation(playerTr.position - transform.position);      //�÷��̾�� �������� �Ÿ��� ���ؼ� 
            cRot.z = cRot.x = 0;    
            transform.rotation = Quaternion.Slerp(transform.rotation, cRot, 10f * Time.deltaTime);  //�÷��̾��� �������� Enemy�� ȸ����Ų��

        }
    }
    public void EnemyVaseMove()    //����� �ֺ��� ���ٸ� RayCastMove�� �ϰ� ����� �ֺ����ִٸ� ����� ������ �޼���
    {
        disOne = (stairList[nextIdx].position - transform.position).normalized;                     //����� ��ġ�� ���� ��ġ�� Ȯ���Ͽ� ������ ���Ѵ�
        starDist = Vector3.Distance(stairList[nextIdx].position, transform.position);               //����� ��ġ�� ���� ��ġ�� Ȯ���Ͽ� �Ÿ��� ���Ѵ�

        float distTwo = Vector3.Distance(stairList[3].position, transform.position);                //�ι�° �����ġ�� ���� ��ġ�� ����� �Ÿ�
        float distTree = Vector3.Distance(stairList[nextIdx].position, transform.position);         //����° �����ġ�� ���� ��ġ�� ����� �Ÿ�
        if (distTree <= 3f)                                                         //��ܰ� Enemy�� �Ÿ��� 3�̶��
        {
            isStair = true;                                                         //����� �ǳʹ�
        }
        if (!isStair)                                                               //�װ� �ƴ϶�� ���̷� �����̴� �޼ҵ带 Ȱ��ȭ ��Ų��
        {
            RayMove();

        }
        else if (isStair)                                                           //��ܰ� Enemy�� ���̰� 3�̶�� ����� �ǳʹ�
        {
            transform.Translate(disOne * enemyData.e_MoveSpeed * Time.deltaTime);
        }
        if (starDist <= 0.5f)                                                       //�����ġ�� �����ߴٸ�
        {

            ++nextIdx;                                                              //Idx�� ������ ����� �ٿö����� Ȯ���Ѵ�
            if (nextIdx >= 4)
            {
                nextIdx = 3;                                                        
            }
        }
        if (distTwo <= 0.5f || distTree > 7)                                        //����� �� �ö��ٸ� ��ܿ����⸦ ��Ȱ��ȭ�����ְ� Idx�� 0���� �ʱ�ȭ�����ش�
        {
            isStair = false;
            nextIdx = 0;
        }
    }
    private void RayMove()                              //�׺���̼� �Ž��� �����ϰ� ��/������Ʈ�� �ε����� ���� ������ Ʋ�� �ڵ������� �����̰� �ϴ� �޼���
    {
        Vector3 caracRot = transform.localEulerAngles;  //ȸ���� �ҋ� y���� ����� ���ֵ��� �ϰ� �ϴ� ����
        caracRot.x = caracRot.z = 0f;
        transform.localEulerAngles = caracRot;
        //frontDir = transform.TransformDirection(frontDir);
        transform.Translate(transform.forward * Time.deltaTime * enemyData.e_MoveSpeed);    //�⺻������ �����θ� �̵��Ѵ�

        //���̸� �� ��ġ(���̸� �ٴ��ʿ��� ��ԵǸ� ���� �ο��� �ɸ��� ������ ���̸� �ٴں��� ���� ���ʿ��� ���ش�) 
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
        //���̸� ���� �� ���⿡ ������Ʈ/�ǹ��� ������ Ȯ���ϰ� �ִٸ� �׹ݴ�������� ȸ���Ѵ�

        if (!Physics.Raycast(rayHeight, dis, out hit, RayDistance)) return;
        Quaternion rot = Quaternion.LookRotation(rDis / rayPer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
    }

    public void OnPlayerTrace()
    {
        //�÷��̾ �Ѿư��� �޼���
        Vector3 plDis = (playerTr.position - transform.position).normalized;
        transform.Translate(plDis * enemyData.e_MoveSpeed * Time.deltaTime);
        Quaternion plRot = Quaternion.LookRotation(playerTr.position - transform.position);
        plRot.z = plRot.x = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, plRot, 10f * Time.deltaTime);
    }
}

