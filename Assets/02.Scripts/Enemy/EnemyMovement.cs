using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private CharacterController ch;
    private float walkSpeed;
    public float RayDistance;
    public float rayPer;
    RaycastHit frontHit;
    RaycastHit righHit;
    RaycastHit leftHit;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ch = GetComponent<CharacterController>();
        walkSpeed = 2f;
        rayPer = 2;
        RayDistance = 1;
    }

    void Update()
    {
        
    }

    public void RayMove()
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
}
