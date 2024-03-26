using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoopSpawn : MonoBehaviour
{
    private Transform[] spawnPoints;
    private Transform[] enemySpanwPoint;
    private Transform gunSpawnPoint;
    private List<Transform> spawnPointsList = new List<Transform>();
    private List<Transform> enemySpanwPointList = new List<Transform>();
    private PlayerDamage playerDamage;
    private GameObject rifleItem;
    private GameObject shotgunItem;
    private int spawnTrIdx;
    private int e_spawnTrIdx;
    public int spawnSgBulletCount;
    public int spawnRfBulletCount;
    public int spawnMadicineCount;
    public int spawnSgMaxBulletCount;
    public int spawnRfMaxBulletCount;
    public int spawnMaxMadicineCount;
    public int enemySpawnCount;
    public int enemyMaxSpawnCount;
    public float spawnTime;
    private HashSet<int> e_spawnIdx = new HashSet<int>();
    private HashSet<int> spawnIdx = new HashSet<int>();
    void Start()
    {
        playerDamage =FindObjectOfType<PlayerDamage>();
        rifleItem = Resources.Load<GameObject>("Spawn/RifleItem");
        shotgunItem = Resources.Load<GameObject>("Spawn/ShotGunItem");
        spawnPoints = GameObject.Find("SpawnPointsGroup").GetComponentsInChildren<Transform>();
        enemySpanwPoint = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();
        gunSpawnPoint = GameObject.Find("GunSpawnPoint").transform;
        spawnSgBulletCount = 0;
        spawnRfBulletCount = 0;
        spawnMadicineCount = 0;
        spawnSgMaxBulletCount = 10;
        spawnRfMaxBulletCount = 10;
        spawnMaxMadicineCount = 10;
        enemyMaxSpawnCount = 5;
        e_spawnTrIdx = 0;
        spawnIdx.Clear();
        e_spawnIdx.Clear();
        for (int i = 1; i < spawnPoints.Length; i++)
        {
            spawnPointsList.Add(spawnPoints[i]);
        }
        for(int i = 1; i < enemySpanwPoint.Length; i++)
        {
            enemySpanwPointList.Add(enemySpanwPoint[i]);
        }
        StartCoroutine(SpawnItem());
    }
    IEnumerator SpawnItem()
    {
       
        yield return new WaitForSeconds(2f);
        /*do
        {
            spawnTrIdx = Random.Range(0, spawnPointsList.Count);
        } while (spawnIdx.Contains(spawnTrIdx));
        spawnIdx.Add(spawnTrIdx);
        Instantiate(rifleItem, spawnPointsList[spawnTrIdx].position, Quaternion.identity);*/
        Instantiate(rifleItem, gunSpawnPoint.position, Quaternion.identity);
        GameObject _rifleBulletBox1 = ObjectPoolingManager.objPooling.GetRifleBulletBox();
        _rifleBulletBox1.transform.position = gunSpawnPoint.position + (Vector3.right * 0.5f);
        _rifleBulletBox1.transform.rotation = Quaternion.identity;
        _rifleBulletBox1.SetActive(true);
        --spawnRfMaxBulletCount;
        do
        {
            spawnTrIdx = Random.Range(0, spawnPointsList.Count);
        } while (spawnIdx.Contains(spawnTrIdx));
        Instantiate(shotgunItem, spawnPointsList[spawnTrIdx].position, Quaternion.identity);
        spawnIdx.Add(spawnTrIdx);
        while (!playerDamage.isDie )
        {
            spawnTime = Random.Range(2f, 3f);
            yield return new WaitForSeconds(spawnTime);
            if (spawnRfBulletCount < spawnRfMaxBulletCount)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx));
                spawnIdx.Add(spawnTrIdx);
                GameObject _rifleBulletBox = ObjectPoolingManager.objPooling.GetRifleBulletBox();
                _rifleBulletBox.transform.position = spawnPointsList[spawnTrIdx].transform.position;
                _rifleBulletBox.transform.rotation = Quaternion.identity;
                _rifleBulletBox.SetActive(true);
                spawnRfBulletCount++;
            }
            if (spawnSgBulletCount < spawnSgMaxBulletCount)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx));
                spawnIdx.Add(spawnTrIdx);
                GameObject _shotgunBulletBox = ObjectPoolingManager.objPooling.GetShotGunBulletBox();
                _shotgunBulletBox.transform.position = spawnPointsList[spawnTrIdx].position;
                _shotgunBulletBox.transform.rotation = Quaternion.identity;
                _shotgunBulletBox.SetActive(true);
                spawnSgBulletCount++;
            }
            if (spawnMadicineCount < spawnMaxMadicineCount)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx));
                spawnIdx.Add(spawnTrIdx);
                GameObject _madicine = ObjectPoolingManager.objPooling.GetMadicine();
                _madicine.transform.position = spawnPointsList[spawnTrIdx].position;
                _madicine.transform.rotation = Quaternion.identity;
                _madicine.SetActive(true);
                spawnMadicineCount++;
            }
            if (enemySpawnCount <= enemyMaxSpawnCount)
            {
                do
                {
                    e_spawnTrIdx = Random.Range(0, enemySpanwPointList.Count);
                } while (e_spawnIdx.Contains(e_spawnTrIdx));
                e_spawnIdx.Add(e_spawnTrIdx);
                GameObject _enemy = ObjectPoolingManager.objPooling.GetEnemy();
                _enemy.transform.position = enemySpanwPointList[e_spawnTrIdx].position;
                _enemy.transform.rotation = Quaternion.identity;
                _enemy.SetActive(true);
                enemySpawnCount++;
            }
        }
    }
}
