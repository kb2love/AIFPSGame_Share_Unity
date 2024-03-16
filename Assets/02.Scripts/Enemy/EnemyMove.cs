using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    CharacterController controller;
    Transform[] stairsPoint;
    List<Transform> stairList = new List<Transform>();
    EnemyMovement enemyMovement;
    float walSpeed;
    public int nextIdx = 0;
    public float dist;
    public Vector3 disOne;
    bool isStair;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        walSpeed = 2f;
        isStair = false;
        stairsPoint = GameObject.Find("StairPoints").GetComponentsInChildren<Transform>();
        enemyMovement = GetComponent<EnemyMovement>();
        for(int i = 0; i < stairsPoint.Length; i++)
        {
            stairList.Add(stairsPoint[i]);
        }
        stairList.RemoveAt(0);
    }
    private void Update()
    {

        Vector3 plDir = new Vector3(0, 0, 1);
        plDir = transform.TransformDirection(plDir);

            disOne = (stairList[nextIdx].position - transform.position).normalized;
            dist = Vector3.Distance(stairList[nextIdx].position, transform.position);
        Vector3 height = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        float distTwo = Vector3.Distance(stairList[3].position, transform.position);
        float distTree = Vector3.Distance(stairList[nextIdx].position, transform.position);
        Debug.DrawRay(height, transform.forward * 5f, Color.yellow);
        if(distTree <= 3f)
        {
            Debug.Log("stair");
            isStair=true;
        }
        if(!isStair)
        {
            enemyMovement.RayMove();
            
        }
        if( isStair )
        {
            controller.Move(disOne * walSpeed * Time.deltaTime);
        }
        if(dist <= 0.5f)
        {
            
            Debug.Log("next++");
            ++nextIdx;
            if(nextIdx >= 4)
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
}

