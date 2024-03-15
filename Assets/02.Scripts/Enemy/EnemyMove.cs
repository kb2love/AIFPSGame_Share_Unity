using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Transform playerTr;
    private EnemyAI enemyAI;
    private float speed;
    public bool isEnemyMove;

    private readonly int aniE_FowardSpeed = Animator.StringToHash("FowardSpeed");
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindWithTag("Player").transform;
        enemyAI = GetComponent<EnemyAI>();
        isEnemyMove = false;
        StartCoroutine(EnemyMovement());
        speed = agent.speed;
    }

    void Update()
    {
        
    }
    IEnumerator EnemyMovement()
    {
        while(!enemyAI.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            if(isEnemyMove)
            {
                agent.destination = playerTr.position;
                animator.SetFloat(aniE_FowardSpeed,agent.speed);
            }
        }
    }
}
