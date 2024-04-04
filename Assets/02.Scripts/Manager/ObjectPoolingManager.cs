using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager objPooling;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GranadeData granadeData;
    [SerializeField] private GunData gunData;
    [SerializeField] private MadicinData madicinData;
    private List<GameObject> expEffectList = new List<GameObject>();
    private List<GameObject> s_granadeList = new List<GameObject>();
    private List<GameObject> w_granadeList = new List<GameObject>();
    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> rifleBulletBoxList = new List<GameObject>();
    private List<GameObject> shotgunBulletBoxList = new List<GameObject>();
    private List<GameObject> madicineList = new List<GameObject>();
    private List<GameObject> playerBulletList = new List<GameObject>();
    private List<GameObject> enemyBulletList = new List<GameObject>();
    private List<GameObject> hitE_List = new List<GameObject>();
    private int maxPlayerBullet;
    private int maxEnmeyBullet;
    private int itemSpawnCount;
    void Awake()
    {
        /*if (objPooling == null)*/
            objPooling = this;
        /*else if (objPooling != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);*/
    }
    void OnEnable()
    {
        maxEnmeyBullet = 40;
        maxPlayerBullet = 30;
        itemSpawnCount = 10;
        CreatePlayerBullet();
        CreateHitEffect();  
        CreateEnemyBullet();
        CreateMadicine();
        CreateRifleBulletBox();
        CreateShotGunBulletBox();
        CreateEnemy();
        CreateWeaponGranade();
        CreateSpawnGranade();
        CreateExpEffect();
    }
    void CreatePlayerBullet()
    {
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for (int i = 0; i < maxPlayerBullet; i++)
        {
            GameObject _bullet = Instantiate(playerData.bullet, playerBulletGroup.transform);
            _bullet.name = (i + 1).ToString() + "발";
            _bullet.gameObject.SetActive(false);
            playerBulletList.Add(_bullet);
        }
    }
    void CreateEnemyBullet()
    {
        GameObject enemyBulletGroup = new GameObject("EnemyBulletGroup");
        for (int i = 0; i < maxEnmeyBullet; i++)
        {
            GameObject e_bullet = Instantiate(enemyData.e_bullet, enemyBulletGroup.transform); 
            e_bullet.name = "e_" + (i + 1).ToString() + "발";
            e_bullet.gameObject.SetActive(false);
            enemyBulletList.Add(e_bullet);
        }
    }
    void CreateHitEffect()
    {
        GameObject hitEffectGroup = new GameObject("HitEffectGroup");
        for (int i = 0; i < itemSpawnCount; i++)
        {
            GameObject _effect = Instantiate(enemyData.hitParticle, hitEffectGroup.transform);
            _effect.name = (i + 1).ToString() + "번 Effect";
            _effect.gameObject.SetActive(false);
            hitE_List.Add(_effect);
        }
    }
    void CreateMadicine()
    {
        GameObject madicineGroup = new GameObject("MadicineGroup");
        for (int i = 0; i < itemSpawnCount; i++)
        {
            GameObject _madicine = Instantiate(madicinData.madicine, madicineGroup.transform);
            _madicine.name = (i + 1).ToString() + "번 Madicine";
            _madicine.gameObject.SetActive(false);
            madicineList.Add(_madicine);
        }
    }
    void CreateRifleBulletBox()
    {
        GameObject rifleBulletBoxGroup = new GameObject("RifleBulletBoxGroup");
        for (int i = 0; i < itemSpawnCount; i++)
        {
            GameObject _rifleBulletBox = Instantiate(gunData.rifleBullet, rifleBulletBoxGroup.transform);
            _rifleBulletBox.name = (i + 1).ToString() + "번 rifleBulletBox";
            _rifleBulletBox.gameObject.SetActive(false);
            rifleBulletBoxList.Add(_rifleBulletBox);
        }
    }
    void CreateShotGunBulletBox()
    {
        GameObject shotgunBulletBoxGroup = new GameObject("ShotGunBulletBoxGroup");
        for (int i = 0; i < itemSpawnCount; i++)
        {
            GameObject _shotgunBulletBox = Instantiate(gunData.shotgunBullet, shotgunBulletBoxGroup.transform);
            _shotgunBulletBox.name = (i + 1).ToString() + "번 rifleBulletBox";
            _shotgunBulletBox.gameObject.SetActive(false);
            shotgunBulletBoxList.Add(_shotgunBulletBox);
        }
    }
    void CreateEnemy()
    {
        GameObject enemyGroup = new GameObject("EnemyGroup");
        for (int i = 0; i < 6; i++)
        {
            GameObject _enemy = Instantiate(enemyData.spawnEnemy, enemyGroup.transform);
            _enemy.name = (i + 1).ToString() + "번 Enemy";
            _enemy.gameObject.SetActive(false);
            enemyList.Add(_enemy);
        }
    }
    void CreateWeaponGranade()
    {
        GameObject w_GranadeGroup = new GameObject("W_GranadeGroup");
        for (int i = 0; i < 4; i++)
        {
            GameObject _w_granade = Instantiate(granadeData.throwGranade, w_GranadeGroup.transform);
            _w_granade.name = (i + 1).ToString() + "번 Granade";
            _w_granade.gameObject.SetActive(false);
            w_granadeList.Add(_w_granade);
        }
    }
    void CreateSpawnGranade()
    {
        GameObject s_GranadeGroup = new GameObject("S_GranadeGroup");
        for (int i = 0; i < itemSpawnCount; i++)
        {
            GameObject _s_granade = Instantiate(granadeData.spawnGranade, s_GranadeGroup.transform);
            _s_granade.name = (i + 1).ToString() + "번 Granade";
            _s_granade.gameObject.SetActive(false);
            s_granadeList.Add(_s_granade);
        }
    }
    void CreateExpEffect()
    {
        GameObject expEffectGroup = new GameObject("ExpEffectGroup");
        for (int i = 0; i < 6; i++)
        {
            GameObject _expEffect = Instantiate(granadeData.expEffect, expEffectGroup.transform);
            _expEffect.name = (i + 1).ToString() + "번 expEffect";
            _expEffect.gameObject.SetActive(false);
            expEffectList.Add(_expEffect);
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
    
    public GameObject GetEnemyBullet()
    {
        foreach(GameObject _bullet in enemyBulletList)
        {
            if( !_bullet.activeSelf)
                return _bullet;
        }
        return null;
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
    public GameObject GetMadicine()
    {
        foreach (GameObject _madicine in madicineList)
        {
            if (!_madicine.activeSelf)
                return _madicine;
        }
        return null;
    }
    public GameObject GetRifleBulletBox()
    {
        foreach (GameObject _rifleBulletBox in rifleBulletBoxList)
        {
            if (!_rifleBulletBox.activeSelf)
                return _rifleBulletBox;
        }
        return null;
    }
    public GameObject GetShotGunBulletBox()
    {
        foreach (GameObject _shotgunBulletBox in shotgunBulletBoxList)
        {
            if (!_shotgunBulletBox.activeSelf)
                return _shotgunBulletBox;
        }
        return null;
    }
    public GameObject GetEnemy()
    {
        foreach (GameObject _enemy in enemyList)
        {
            if (!_enemy.activeSelf)
                return _enemy;
        }
        return null;
    }
    public GameObject GetWeaponGranade()
    {
        foreach (GameObject _w_granade in w_granadeList)
        {
            if (!_w_granade.activeSelf)
                return _w_granade;
        }
        return null;
    }
    public GameObject GetSpawnGranade()
    {
        foreach (GameObject _s_granade in s_granadeList)
        {
            if (!_s_granade.activeSelf)
                return _s_granade;
        }
        return null;
    }
    public GameObject GetExpEffect()
    {
        foreach (GameObject _expEffect in expEffectList)
        {
            if (!_expEffect.activeSelf)
                return _expEffect;
        }
        return null;
    }
}
