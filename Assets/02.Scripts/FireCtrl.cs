using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Animator animator;
    private float fireTime;
    private readonly int aniShoot = Animator.StringToHash("Shoot");
    private bool isReload;
    void Start()
    {
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        bullet = Resources.Load<GameObject>("Weapon/Bullet");
        source = GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        fireTime = Time.time;
        isReload = false;
        fireClip = Resources.Load<AudioClip>("Sounds/Fires/p_ak_1");
    }

    void Update()
    {
        
    }
}
