using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager objPooling;
    private GameObject Playerbullet;
    private List<GameObject> playerBulletList = new List<GameObject>();
    private GameObject hitEffect;
    private List<GameObject> hitE_List = new List<GameObject>();
    int maxPlayerBullet;
    void Awake()
    {
        if (objPooling == null)
            objPooling = this;
        else if (objPooling != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Playerbullet = Resources.Load<GameObject>("Weapon/Bullet");
        hitEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        maxPlayerBullet = 20;
        CreatePlayerBullet();
        CreateHitEffect();  
    }
    private void Start()
    {
    }
    void Update()
    {

    }
    void CreatePlayerBullet()
    {
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for (int i = 0; i < maxPlayerBullet; i++)
        {
            GameObject _bullet = Instantiate(Playerbullet, playerBulletGroup.transform);
            _bullet.name = (i + 1).ToString() + "��";
            _bullet.gameObject.SetActive(false);
            playerBulletList.Add(_bullet);
        }
    }
    public GameObject GetPlayerBullet()
    {
        foreach (GameObject _bullet in playerBulletList)
        {
            if(!_bullet.activeSelf)
                return _bullet;
        }
        return null;
    }
    void CreateHitEffect()
    {
        GameObject hitEffectGroup = new GameObject("HitEffectGroup");
        for(int i = 0; i < 6;  i++)
        {
            GameObject _effect = Instantiate(hitEffect, hitEffectGroup.transform);
            _effect.name = (i + 1).ToString() + "�� Effect";
            _effect.gameObject.SetActive(false);
            hitE_List.Add(_effect);
        }
    }
    public GameObject GetHitEffect()
    {
        foreach(GameObject _effect in hitE_List)
        {
            if(!_effect.activeSelf)
                return _effect;
        }
        return null;
    }
}
