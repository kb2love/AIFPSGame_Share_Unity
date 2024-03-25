using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerDamage : MonoBehaviour
{
    public bool isDie;
    public int HP;
    public int maxHp;
    private string e_bulletTag = "E_Bullet";
    public Image hpBarImage;
    private GameObject _effect;
    void Start()
    {
        _effect = ObjectPoolingManager.objPooling.GetHitEffect();
        hpBarImage = GameObject.Find("Image-HpBar").GetComponent<Image>();
        isDie = false;
        maxHp = 100;
        HP = maxHp;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(e_bulletTag))
        {
            HP -= (int)col.gameObject.GetComponent<EnemyBulletCtrl>().damage;
            hpBarImage.fillAmount = (float)HP / (float)maxHp;
            Vector3 normal = col.contacts[0].normal;
            _effect.transform.position = col.contacts[0].point;
            _effect.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, normal);
            _effect.SetActive(true);
            Invoke("EffectOff", 1f);    
            if(HP <= 0) 
                isDie = true;
        }
    }
    private void EffectOff()
    {
        _effect.SetActive(false);
    }
}
