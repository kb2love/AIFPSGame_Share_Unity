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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ch = GetComponent<CharacterController>();
        walkSpeed = 2f;
    }

    void Update()
    {
       Vector3 plDir = new Vector3(0f, 0f, 1).normalized;
        Vector3 caracRot = transform.localEulerAngles;
        caracRot.x = caracRot.z = 0f;
        transform.localEulerAngles = caracRot;
        plDir = transform.TransformDirection(plDir);
        ch.Move(plDir * Time.deltaTime * walkSpeed);
        Debug.DrawRay(transform.position, transform.forward * 20f, Color.yellow);
        Debug.DrawRay(transform.position, transform.right * 20f, Color.yellow);
        Debug.DrawRay(transform.position, -transform.right * 20f, Color.yellow);
        RaycastHit hit;
        bool rayFront = Physics.Raycast(transform.position, transform.forward, out hit, RayDistance);
        bool rayRight = Physics.Raycast(transform.position, transform.right, out hit, RayDistance);
        bool rayLeft = Physics.Raycast(transform.position,-transform.right, out hit, RayDistance);
        if (rayFront && rayRight && rayLeft)
        {
            
            Quaternion quaternion = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 10f);
        }
        else if(rayFront && rayRight)
        {
            Quaternion quaternion = Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 10f);
        }
        else if(rayFront && rayLeft)
        {
            Quaternion quaternion = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 10f);
        }
        else 
        if(rayFront)
        {
            Quaternion quaternion = Quaternion.LookRotation(hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 10f);
        }

    }
}
