using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    private CharacterController ch;
    private Animator animator;
    private PlayerDamage playerDamage;
    private AudioSource audioSource;
    private float gravity;
    private float gravityValue;
    private float hor, ver;
    private float hei;
    private bool isSprint;
    private Vector3 plDir;
    private Vector3 moveVelocity;
    void OnEnable()
    {
        ch = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerDamage = GetComponent<PlayerDamage>();
        gravityValue = -0.05f;
        gravity = gravityValue;
        isSprint = false;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        RunCheck();
        Move();
    }

    private void Move()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        {
            animator.SetFloat("speedX", hor, 0.1f, Time.deltaTime);
            animator.SetFloat("speedY", ver, 0.1f, Time.deltaTime);
        }
        plDir = new Vector3(hor, 0, ver);
        moveVelocity = plDir * (isSprint ? playerData.runSpeed : playerData.moveSpeed) * Time.deltaTime;
        
        GravityAndJump();
        moveVelocity.y = hei;
        moveVelocity = transform.TransformDirection(moveVelocity);
        if(ch.velocity.sqrMagnitude > 0.1f)
        {
            if (!audioSource.isPlaying || audioSource.clip != (playerData.walkClip || playerData.runClip))
            {
                SoundManager.soundInst.PlaySound(isSprint ? playerData.runClip : playerData.walkClip, audioSource);
            }
        }
        ch.Move(moveVelocity);
    }
    private void RunCheck()
    {
        if(Input.GetKey(KeyCode.LeftShift) && ch.velocity.sqrMagnitude > 0.1f)
        {
            isSprint = true;
            animator.SetBool("IsSprint", true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) | ch.velocity.sqrMagnitude <= 0.1f)
        {
            isSprint = false;
            animator.SetBool("IsSprint", false);
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
                if (isSprint)
                {
                    animator.SetTrigger("SprintJump");
                }
                else
                {
                    animator.SetTrigger("StandingJump");
                }
                SoundManager.soundInst.PlaySound(playerData.jumpClip, audioSource);
            }
        }
    }
}
