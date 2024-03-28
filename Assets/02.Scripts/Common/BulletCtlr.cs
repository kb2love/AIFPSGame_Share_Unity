using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtlr : MonoBehaviour
{
    [SerializeField] GunData shot;
    private TrailRenderer trailRenderer;
    private FireCtrl ctrl;
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private float bulletSpeed;
    public float damage;
    void Awake()
    {
        damage = 15;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        ctrl = FindObjectOfType<FireCtrl>();
        sphereCollider = GetComponent<SphereCollider>();
        bulletSpeed = 1000f;
    }
    void OnEnable()
    {
        damage = shot.g_damage;
        if (ctrl.isShotGun)
            sphereCollider.radius = 0.2f;
        else if (ctrl.isRifle)
            sphereCollider.radius = 0.05f;
        rb.AddForce(transform.forward * bulletSpeed);
        Invoke("OffBullet", 3.0f);
    }
    void OffBullet()
    {
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        trailRenderer.Clear();
        rb.Sleep();
        transform.position = Vector3.zero;
    }
}
