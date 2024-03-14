using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtlr : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private Rigidbody rb;
    private float bulletSpeed;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        bulletSpeed = 1000f;
    }
    void OnEnable()
    {
        rb.AddForce(transform.forward * bulletSpeed);
        Invoke("OffBullet", 3.0f);
    }
    void OffBullet()
    {
        gameObject.SetActive(false);
        rb.Sleep();
    }
    void OnDisable()
    {
        trailRenderer.Clear();
        transform.position = Vector3.zero;
    }
}
