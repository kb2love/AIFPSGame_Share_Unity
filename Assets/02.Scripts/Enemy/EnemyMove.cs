using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    // private CharacterController ch;
    [SerializeField] EnemyData enemyData;
    private Transform[] stairsPoint;
    private Transform playerTr;
    private List<Transform> stairList = new List<Transform>();
    public float walkSpeed;
    private int nextIdx = 0;
    private float dist;
    private Vector3 disOne;
    private  bool isStair;
    private float RayDistance;
    private float rayPer;
    public bool isTrace;
    private RaycastHit frontHit;
    private RaycastHit righHit;
    private RaycastHit leftHit;
    void Start()
    {
       // ch = GetComponent<CharacterController>();
        playerTr = GameObject.FindWithTag("Player").transform;
        stairsPoint = GameObject.Find("StairPoints").GetComponentsInChildren<Transform>();
        for (int i = 0; i < stairsPoint.Length; i++)
        {
            stairList.Add(stairsPoint[i]);
        }
        stairList.RemoveAt(0);
        rayPer = 2;
        RayDistance = 1;
    }
    private void OnEnable()
    {
        isStair = false;
        isTrace = false;
        walkSpeed = enemyData.e_MoveSpeed;
    }

    public void RacastStairs()
    {
        if (isTrace) return;
        disOne = (stairList[nextIdx].position - transform.position).normalized;
        dist = Vector3.Distance(stairList[nextIdx].position, transform.position);
        Vector3 height = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        float distTwo = Vector3.Distance(stairList[3].position, transform.position);
        float distTree = Vector3.Distance(stairList[nextIdx].position, transform.position);
        Debug.DrawRay(height, transform.forward * 5f, Color.yellow);
        if (distTree <= 3f)
        {
            isStair = true;
        }
        if (!isStair)
        {
            RayMove();

        }
        else if (isStair)
        {
            transform.Translate(disOne * walkSpeed * Time.deltaTime);
        }
        if (dist <= 0.5f)
        {

            ++nextIdx;
            if (nextIdx >= 4)
            {
                nextIdx = 3;
            }
        }
        if (distTwo <= 0.5f || distTree > 7)
        {
            isStair = false;
            nextIdx = 0;
        }
    }
   private void RayMove()
    {
        Vector3 plDir = new Vector3(0f, 0f, 1);
        Vector3 caracRot = transform.localEulerAngles;
        caracRot.x = caracRot.z = 0f;
        transform.localEulerAngles = caracRot;
        //plDir = transform.TransformDirection(plDir);
        transform.Translate(plDir * Time.deltaTime * walkSpeed);
        Vector3 rayHeight = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        Debug.DrawRay(rayHeight, transform.forward * 5f, Color.yellow);
        Debug.DrawRay(rayHeight, transform.right * 5f, Color.yellow);
        Debug.DrawRay(rayHeight, -transform.right * 5f, Color.yellow);
        if (Physics.Raycast(rayHeight, -transform.right, out leftHit, RayDistance))
        {
            if (!Physics.Raycast(rayHeight, -transform.right, out leftHit, RayDistance)) return;
            Quaternion rot = Quaternion.LookRotation(transform.right / rayPer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
        else if (Physics.Raycast(rayHeight, transform.right, out righHit, RayDistance))
        {
            if (!Physics.Raycast(rayHeight, transform.right, out righHit, RayDistance)) return;
            Quaternion rot = Quaternion.LookRotation(-transform.right / rayPer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
        else if (Physics.Raycast(rayHeight, transform.forward, out frontHit, RayDistance))
        {
            if (!Physics.Raycast(rayHeight, transform.forward, out frontHit, RayDistance)) return;
            Quaternion rot = Quaternion.LookRotation(frontHit.normal / rayPer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);         
        }
    }
    public void OnPlayerTrace()
    {
        /*if (!isTrace) return;
        float distY = playerTr.position.y - transform.position.y;
        if (distY > 1)
        {
            isTrace = false;
        }
        else if(distY < 1)
        {
            isTrace = true;
        }
        isTrace = true;*/
        Vector3 plDis = (playerTr.position - transform.position).normalized;
        transform.Translate(plDis * walkSpeed * Time.deltaTime);
        Vector3 plRot = playerTr.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(plRot), 10f * Time.deltaTime);
    }
}

