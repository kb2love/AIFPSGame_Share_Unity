using System.Collections;
using System.Collections.Generic;
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
    private readonly int aniSpeedX = Animator.StringToHash("speedX");
    private readonly int aniSpeedY = Animator.StringToHash("speedY");
    private readonly int aniSprint = Animator.StringToHash("IsSprint");
    private readonly int aniStandingJump = Animator.StringToHash("StandingJump");
    private readonly int aniSprintJump = Animator.StringToHash("SprintJump");
    void Start()
    {
        ch = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerDamage = GetComponent<PlayerDamage>();
        gravityValue = -0.05f;
        gravity = gravityValue;
        isSprint = false;
        audioSource = GetComponent<AudioSource>();
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
            animator.SetFloat(aniSpeedX, hor, 0.1f, Time.deltaTime);
            animator.SetFloat(aniSpeedY, ver, 0.1f, Time.deltaTime);
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
                audioSource.clip = isSprint ? playerData.runClip : playerData.walkClip ;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if(ch.velocity.sqrMagnitude <= 0.1f)
        {
            audioSource.Stop();
        }
        ch.Move(moveVelocity);
    }
    private void RunCheck()
    {
        if(Input.GetKey(KeyCode.LeftShift) && ch.velocity.sqrMagnitude > 0.1f)
        {
            isSprint = true;
            animator.SetBool(aniSprint, true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) | ch.velocity.sqrMagnitude <= 0.1f)
        {
            isSprint = false;
            animator.SetBool(aniSprint, false);
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
                audioSource.Stop();
                if (isSprint)
                {
                    animator.SetTrigger(aniSprintJump);
                    audioSource.PlayOneShot(playerData.jumpClip, 1.0f);
                }
                else
                {
                    animator.SetTrigger(aniStandingJump);
                    audioSource.PlayOneShot(playerData.jumpClip, 1.0f);
                }
            }
        }
    }
}
