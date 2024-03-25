using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoopSpawn : MonoBehaviour
{
    private Transform[] spawnPoints;
    private List<Transform> spawnPointsList = new List<Transform>();
    private PlayerDamage playerDamage;
    private GameObject rifleItem;
    private GameObject shotgunItem;
    public int rifleSpawn;
    public int shotgunSpawn;
    void Start()
    {
        playerDamage =GameObject.FindWithTag("Player").GetComponent<PlayerDamage>();
        rifleItem = Resources.Load<GameObject>("Spawn/RifleItem");
        shotgunItem = Resources.Load<GameObject>("Spawn/ShotGunItem");
        spawnPoints = GameObject.Find("SpawnPointsGroup").GetComponentsInChildren<Transform>();
        for (int i = 1; i < spawnPoints.Length; i++)
        {
            spawnPointsList.Add(spawnPoints[i]);
        }
        StartCoroutine(SpawnItem());
    }
    IEnumerator SpawnItem()
    {
        rifleSpawn = Random.Range(0, spawnPointsList.Count);
        shotgunSpawn = Random.Range(0,spawnPointsList.Count);
        yield return new WaitForSeconds(2f);
        Instantiate(rifleItem, spawnPointsList[rifleSpawn].position, Quaternion.identity);
        Instantiate(shotgunItem, spawnPointsList[shotgunSpawn].position, Quaternion.identity);
        if (rifleSpawn == shotgunSpawn)
        {
            if(rifleSpawn > 0)
                rifleSpawn--;
            else if( rifleSpawn <= 0)
                rifleSpawn++;
        }
        while(!playerDamage.isDie)
        {
            float spawnTime = Random.Range(2f, 10f);
            yield return new WaitForSeconds(spawnTime);
            GameObject _rifleBulletBox = ObjectPoolingManager.objPooling.GetRifleBulletBox();
            _rifleBulletBox.transform.position = spawnPointsList[Random.Range(0, spawnPointsList.Count)].transform.position;
            _rifleBulletBox.transform.rotation = Quaternion.identity;
        }
    }
}
