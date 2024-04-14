using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GranadeCtrl : MonoBehaviour
{
    [SerializeField]
    private GranadeData granadeData;
    private Rigidbody rb;
    private EnemyDamage enemyDamage;
    private PlayerDamage playerDamage;
    private AudioSource source;
    private int enemyLayer;
    private int playerLayer;
    void OnEnable()
    {
        source = GetComponent<AudioSource>();
        enemyDamage = FindObjectOfType<EnemyDamage>();   
        playerDamage = FindObjectOfType<PlayerDamage>();
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        rb = GetComponent<Rigidbody>();
        Vector3 throwDis = new Vector3(0, 1, 1);
        throwDis = transform.TransformDirection(throwDis);
        rb.AddForce(throwDis * granadeData.throwSpeed);
        StartCoroutine(Explozion());
    }
    private IEnumerator Explozion()
    {
        yield return new WaitForSeconds(2);
        GameObject _expEffect = ObjectPoolingManager.objPooling.GetExpEffect();
        _expEffect.transform.position = transform.position;
        _expEffect.transform.rotation = Quaternion.identity;
        _expEffect.SetActive(true);
        SoundManager.soundInst.PlaySound(granadeData.expClip, source);
        Collider[] cols = Physics.OverlapSphere(transform.position, 10f, enemyLayer | playerLayer);
        foreach (Collider col in cols)
        {
            if (col.gameObject.layer == enemyLayer)
            {
                col.gameObject.GetComponent<EnemyDamage>().ReceiveDamage(50);
                Debug.Log("µÅ?");
            }
            else if (col.gameObject.layer == playerLayer)
            {
                col.gameObject.GetComponent<PlayerDamage>().PlayerReceiveDamage(50);
                Debug.Log("¾ÈµÅ?");
            }
            Vector3 dis = (transform.position - col.transform.position);
            //col.transform.DOJump(dis, 2, 1, 2);
        }

        yield return new WaitForSeconds(1);
        _expEffect.SetActive(false);
        gameObject.SetActive(false);
    }
}
