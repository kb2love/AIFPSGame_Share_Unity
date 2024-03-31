using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeCtrl : MonoBehaviour
{
    [SerializeField]
    private GranadeData granadeData;
    private Rigidbody rb;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 throwFower = new Vector3(0, 1, 1);
        throwFower = transform.TransformDirection(throwFower);
        rb.AddForce(throwFower * granadeData.throwSpeed, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Bomb());
    }
    private IEnumerator Bomb()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        rb.Sleep();
    }
}
