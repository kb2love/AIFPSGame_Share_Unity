using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private Rigidbody rb;
    private float bulletSpeed;
    public float damage;
    void Awake()
    {
        damage = ItemDataBase.itemDataBase.rifleBulletDamage;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
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
    }
    void OnDisable()
    {
        trailRenderer.Clear();
        rb.Sleep();
        transform.position = Vector3.zero;
    }
}
