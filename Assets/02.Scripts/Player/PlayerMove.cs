using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private CharacterController ch;
    private Animator animator;
    private PlayerDamage playerDamage;
    private float gravity;
    private float gravityValue;
    private float hor, ver;
    private float hei;
    private bool isSprint;
    private Vector3 plDir;
    private Vector3 moveVelocity;
    private readonly int speedX = Animator.StringToHash("speedX");
    private readonly int speedY = Animator.StringToHash("speedY");
    private readonly int sprint = Animator.StringToHash("IsSprint");
    void Start()
    {
        ch = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerDamage = GetComponent<PlayerDamage>();
        gravityValue = -0.05f;
        gravity = gravityValue;
        isSprint = false;
        StartCoroutine(PlayerMovement());
    }

    IEnumerator PlayerMovement()
    {
        while(!playerDamage.isDie)
        {
            yield return new WaitForSeconds(0.002f);
            RunCheck();
            Move();
            
        }
    }

    private void Move()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        {
            animator.SetFloat(speedX, hor, 0.1f, Time.deltaTime);
            animator.SetFloat(speedY, ver, 0.1f, Time.deltaTime);
        }
        plDir = new Vector3(hor, 0, ver);
        moveVelocity = plDir * (isSprint ? playerData.runSpeed : playerData.moveSpeed) * Time.deltaTime;
        GravityAndJump();
        moveVelocity.y = hei;
        moveVelocity = transform.TransformDirection(moveVelocity);
        ch.Move(moveVelocity);
    }
    private void RunCheck()
    {
        if(Input.GetKey(KeyCode.LeftShift) && ver > 0)
        {
            isSprint = true;
            animator.SetBool(sprint, true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) | ver <= 0)
        {
            isSprint = false;
            animator.SetBool(sprint, false);
        }
    }
    private void GravityAndJump()
    {
        if (!ch.isGrounded)
        {
            gravity += gravity * Time.deltaTime;
            hei += gravity * Time.deltaTime;
        }
        else
        {
            gravity = gravityValue;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hei = playerData.jumpForce;
            }
        }
    }
}
