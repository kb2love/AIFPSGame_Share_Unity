using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager objPooling;
    private GameObject Playerbullet;
    private List<GameObject> playerBulletList = new List<GameObject>();
    int maxPlayerBullet;
    void Awake()
    {
        if (objPooling == null)
            objPooling = this;
        else if (objPooling != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Playerbullet = Resources.Load<GameObject>("Weapon/Bullet");
        maxPlayerBullet = 20;
        CreatePlayerBullet();
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

}
