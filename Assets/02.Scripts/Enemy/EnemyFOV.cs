using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange;
    [Range(0f, 360f)] public float viewAngle;
    private Transform playerTr;
    private Transform enemyTr;
    private int playerLayer;
    private string playerTag = "Player";
    void Start()
    {
        viewAngle = 15f;
        viewAngle = 120f;
        playerTr = GameObject.FindWithTag(playerTag).transform;
        enemyTr = transform;
    }
    private void OnEnable()
    {
        playerLayer = LayerMask.NameToLayer("PLAYER");
    }
    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sign(angle * Mathf.Rad2Deg), 0f, Mathf.Cos(angle *  Mathf.Deg2Rad)); 
    }
    public bool IsTracePlayer()
    {
        bool isTracePlayer = false;
        Collider[] cols = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
        if(cols.Length == 1)
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            if(Vector3.Angle(enemyTr.forward,dir) < viewAngle * 0.5f)
            {
                return isTracePlayer = true;
            }
        }
        return isTracePlayer;
    }
    public bool IsViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - enemyTr.position).normalized;
        if(Physics.Raycast(enemyTr.position, dir, out hit, viewRange, 1 << playerLayer))
        {
            return isView = hit.collider.CompareTag(playerTag);
        }
        return isView;
    }
}
