using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    CharacterController ch;
    Transform[] stairsPoint;
    List<Transform> stairList = new List<Transform>();
    float walkSpeed;
    public int nextIdx = 0;
    public float dist;
    public Vector3 disOne;
    bool isStair;
    public float RayDistance;
    public float rayPer;
    RaycastHit frontHit;
    RaycastHit righHit;
    RaycastHit leftHit;
    private void Awake()
    {
        ch = GetComponent<CharacterController>();
        walkSpeed = 2f;
        rayPer = 2;
        RayDistance = 1;
        isStair = false;
        stairsPoint = GameObject.Find("StairPoints").GetComponentsInChildren<Transform>();
        for(int i = 0; i < stairsPoint.Length; i++)
        {
            stairList.Add(stairsPoint[i]);
        }
        stairList.RemoveAt(0);

    }
    private void OnEnable()
    {
        EnemyAI.moveHandler += RacastStairs;
    }
    private void Update()
    {
        
    }

    private void RacastStairs()
    {
        Vector3 plDir = new Vector3(0, 0, 1);
        plDir = transform.TransformDirection(plDir);

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
        if (isStair)
        {
            ch.Move(disOne * walkSpeed * Time.deltaTime);
        }
        if (dist <= 0.5f)
        {

            Debug.Log("next++");
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

        Vector3 plDir = new Vector3(0f, 0f, 1).normalized;
        Vector3 caracRot = transform.localEulerAngles;
        caracRot.x = caracRot.z = 0f;
        transform.localEulerAngles = caracRot;
        plDir = transform.TransformDirection(plDir);
        ch.Move(plDir * Time.deltaTime * walkSpeed);
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
    private void OnDisable()
    {
        EnemyAI.moveHandler -= RacastStairs;
    }
}

