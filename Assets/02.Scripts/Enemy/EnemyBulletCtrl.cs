using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;
    private TrailRenderer trailRenderer;
    private Rigidbody rb;
    private float bulletSpeed;
    public float damage;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        bulletSpeed = 1000f;
    }
    void OnEnable()
    {
        rb.AddForce(transform.forward * bulletSpeed);
        Invoke("OffBullet", 3.0f);
        damage = _enemyData.e_Damage;
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
