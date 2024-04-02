using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerDamage : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    public bool isDie;
    public int hp;
    private string e_bulletTag = "E_Bullet";
    public Image hpBarImage;
    private GameObject _effect;
    void Start()
    {
        _effect = ObjectPoolingManager.objPooling.GetHitEffect();
        hpBarImage = GameObject.Find("Image-HpBar").GetComponent<Image>();
        isDie = false;
        hp = playerData.maxHp;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(e_bulletTag))
        {
            PlayerReceiveDamage((int)col.gameObject.GetComponent<EnemyBulletCtrl>().damage);
            Vector3 normal = col.contacts[0].normal;
            _effect.transform.position = col.contacts[0].point;
            _effect.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, normal);
            _effect.SetActive(true);
            Invoke("EffectOff", 1f);
        }
    }
    public void PlayerReceiveDamage(int damage)
    {
        hp -= damage;
        hpBarImage.fillAmount = (float)hp / (float)playerData.maxHp;
        if (hp <= 0)
        {
            isDie = true;
        }
    }
    private void EffectOff()
    {
        _effect.SetActive(false);
    }
}
