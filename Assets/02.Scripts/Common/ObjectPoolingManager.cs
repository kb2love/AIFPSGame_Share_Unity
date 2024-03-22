using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager objPooling;
    private GameObject playerBullet;
    private GameObject enemyBullet;
    private GameObject hitEffect;
    private List<GameObject> playerBulletList = new List<GameObject>();
    private List<GameObject> enemyBulletList = new List<GameObject>();
    private List<GameObject> hitE_List = new List<GameObject>();
    private int maxPlayerBullet;
    private int maxEnmeyBullet;
    void Awake()
    {
        if (objPooling == null)
            objPooling = this;
        else if (objPooling != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        playerBullet = Resources.Load<GameObject>("Weapon/Bullet");
        enemyBullet = Resources.Load<GameObject>("Weapon/E_Bullet");
        hitEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        maxEnmeyBullet = 40;
        maxPlayerBullet = 30;
        CreatePlayerBullet();
        CreateHitEffect();  
        CreateEnemyBullet();
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
            GameObject _bullet = Instantiate(playerBullet, playerBulletGroup.transform);
            _bullet.name = (i + 1).ToString() + "¹ß";
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
    void CreateEnemyBullet()
    {
        GameObject enemyBulletGroup = new GameObject("EnemyBulletGroup");
        for (int i = 0; i < maxEnmeyBullet; i++)
        {
            GameObject e_bullet = Instantiate(enemyBullet, enemyBulletGroup.transform);
            e_bullet.name = "e_" + (i + 1).ToString() + "¹ß";
            e_bullet.gameObject.SetActive(false);
            enemyBulletList.Add(e_bullet);
        }
    }
    public GameObject GetEnemyBullet()
    {
        foreach(GameObject _bullet in enemyBulletList)
        {
            if( !_bullet.activeSelf)
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
            _effect.name = (i + 1).ToString() + "¹ø Effect";
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
