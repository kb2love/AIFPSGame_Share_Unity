using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController ch;
    private Animator animator;
    private float walkSpeed;
    private float runSpeed;
    private float gravity;
    private float gravityValue;
    private float hor, ver;
    private float hei;
    private float jumpForce;
    private bool isSprint;
    public bool isDie;
    private Vector3 plDir;
    private Vector3 moveVelocity;
    private readonly int speedX = Animator.StringToHash("speedX");
    private readonly int speedY = Animator.StringToHash("speedY");
    private readonly int sprint = Animator.StringToHash("IsSprint");
    void Start()
    {
        ch = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        walkSpeed = 3f;
        runSpeed = 5f;
        gravityValue = -0.05f;
        gravity = gravityValue;
        jumpForce = 0.03f;
        isSprint = false;
        isDie = false;
        StartCoroutine(PlayerMovement());
    }

    void Update()
    {
        
    }
    IEnumerator PlayerMovement()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.002f);
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");
            {
                animator.SetFloat(speedX, hor);
                animator.SetFloat(speedY, ver);
            }
            plDir = new Vector3(hor, 0,ver).normalized;
            moveVelocity = plDir * (isSprint ? runSpeed : walkSpeed) * Time.deltaTime;
            {
                if(!ch.isGrounded)
                {
                    gravity += gravity * Time.deltaTime;
                    hei += gravity * Time.deltaTime;
                }
                else
                {
                    gravity = gravityValue;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        hei = jumpForce;
                    }
                }
            }
            moveVelocity.y = hei;
            moveVelocity = transform.TransformDirection(moveVelocity);
            ch.Move(moveVelocity);
        }
    }
}
