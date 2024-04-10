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
    private GameObject dieUi;
    void OnEnable()
    {
        hpBarImage = GameObject.Find("Image-HpBar").GetComponent<Image>();  
        isDie = false;
        hp = playerData.maxHp;
        dieUi = GameObject.Find("Canvas_ui").transform.GetChild(4).gameObject;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(e_bulletTag))
        {
            PlayerReceiveDamage((int)col.gameObject.GetComponent<EnemyBulletCtrl>().damage);
            HitEffect(col);
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "DeadLine")
        {
            isDie = true;
            StartCoroutine(PlayerDie());
        }
    }
    public void PlayerReceiveDamage(int damage)
    {
        hp -= damage;
        hpBarImage.fillAmount = (float)hp / (float)playerData.maxHp;
        if (hp <= 0)
        {
            isDie = true;
            StartCoroutine(PlayerDie());
        }
    }
    IEnumerator PlayerDie()
    {
        yield return new WaitForSeconds(1f);
        dieUi.SetActive(true );
        yield return new WaitForSeconds(2f);
        GameManager.Instance.ScoreSave();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneMove.sceneInst.EndScene();
    }
    private void HitEffect(Collision col)
    {
        Vector3 normal = col.contacts[0].normal;
        GameObject _effect = ObjectPoolingManager.objPooling.GetHitEffect();
        _effect.transform.position = col.contacts[0].point;
        _effect.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, normal);
        _effect.SetActive(true);
        StartCoroutine(EffectFalse(_effect));
    }
    private IEnumerator EffectFalse(GameObject gameObject)
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
