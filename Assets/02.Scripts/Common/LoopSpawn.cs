using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoopSpawn : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    private Transform[] spawnPoints;
    private Transform[] enemySpanwPoint;
    private Transform gunSpawnPoint;
    private List<Transform> spawnPointsList = new List<Transform>();
    private List<Transform> enemySpanwPointList = new List<Transform>();
    private PlayerDamage playerDamage;
    private int spawnTrIdx;
    private int e_spawnTrIdx;
    public int e_Count;
    public int allSpawnTime;
    private int allItemCount;
    private HashSet<int> e_spawnIdx = new HashSet<int>();
    private HashSet<int> spawnIdx = new HashSet<int>();
    void Start()
    {
        playerDamage =FindObjectOfType<PlayerDamage>();
        spawnPoints = GameObject.Find("SpawnPointsGroup").GetComponentsInChildren<Transform>();
        enemySpanwPoint = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();
        gunSpawnPoint = GameObject.Find("GunSpawnPoint").transform;
        e_Count = 0;
        allItemCount = 0;
        spawnIdx.Clear();
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
        Instantiate(gunData.rifle, gunSpawnPoint.position, Quaternion.identity);
        GameObject _rifleBulletBox1 = ObjectPoolingManager.objPooling.GetRifleBulletBox();
        _rifleBulletBox1.transform.position = gunSpawnPoint.position + (Vector3.right * 0.5f);
        _rifleBulletBox1.transform.rotation = Quaternion.identity;
        _rifleBulletBox1.SetActive(true);
        do
        {
            spawnTrIdx = Random.Range(0, spawnPointsList.Count);
        } while (spawnIdx.Contains(spawnTrIdx));
        Instantiate(gunData.shotgun, spawnPointsList[spawnTrIdx].position, Quaternion.identity);
        spawnIdx.Add(spawnTrIdx);
        while (!playerDamage.isDie )
        {
            allSpawnTime = Random.Range(2, 3);
            yield return new WaitForSeconds(allSpawnTime);
            if (ObjectPoolingManager.objPooling.GetRifleBulletBox() != null)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx) && spawnIdx.Count < spawnPointsList.Count);
                spawnIdx.Add(spawnTrIdx);
                GameObject _rifleBulletBox = ObjectPoolingManager.objPooling.GetRifleBulletBox();
                _rifleBulletBox.transform.position = spawnPointsList[spawnTrIdx].transform.position;
                _rifleBulletBox.transform.rotation = Quaternion.identity;
                _rifleBulletBox.SetActive(true);
                allItemCount++;
                if (allItemCount >= spawnPointsList.Count)
                {
                    spawnIdx.Clear();
                    allItemCount = 0;
                }
            }
            if (ObjectPoolingManager.objPooling.GetShotGunBulletBox() != null)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx) && spawnIdx.Count < spawnPointsList.Count);
                spawnIdx.Add(spawnTrIdx);
                GameObject _shotgunBulletBox = ObjectPoolingManager.objPooling.GetShotGunBulletBox();
                _shotgunBulletBox.transform.position = spawnPointsList[spawnTrIdx].position;
                _shotgunBulletBox.transform.rotation = Quaternion.identity;
                _shotgunBulletBox.SetActive(true);
                allItemCount++;
                if (allItemCount >= spawnPointsList.Count)
                {
                    spawnIdx.Clear();
                    allItemCount = 0;
                }
            }
            if (ObjectPoolingManager.objPooling.GetMadicine() != null)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx) && spawnIdx.Count < spawnPointsList.Count);
                spawnIdx.Add(spawnTrIdx);
                GameObject _madicine = ObjectPoolingManager.objPooling.GetMadicine();
                _madicine.transform.position = spawnPointsList[spawnTrIdx].position;
                _madicine.transform.rotation = Quaternion.identity;
                _madicine.SetActive(true);
                allItemCount++;
                if (allItemCount >= spawnPointsList.Count)
                {
                    spawnIdx.Clear();
                    allItemCount = 0;
                }
            }
            if (ObjectPoolingManager.objPooling.GetSpawnGranade() != null)
            {
                do
                {
                    spawnTrIdx = Random.Range(0, spawnPointsList.Count);
                } while (spawnIdx.Contains(spawnTrIdx) && spawnIdx.Count < spawnPointsList.Count);
                spawnIdx.Add(spawnTrIdx);
                GameObject _granade = ObjectPoolingManager.objPooling.GetSpawnGranade();
                _granade.transform.position = spawnPointsList[spawnTrIdx].position;
                _granade.transform.rotation = Quaternion.identity;
                _granade.SetActive(true);
                allItemCount++;
                if (allItemCount >= spawnPointsList.Count)
                {
                    spawnIdx.Clear();
                    allItemCount= 0;
                }
            }
            if (ObjectPoolingManager.objPooling.GetEnemy() != null)
            {
                do
                {
                    e_spawnTrIdx = Random.Range(0, enemySpanwPointList.Count);
                } while (e_spawnIdx.Contains(e_spawnTrIdx) && e_spawnIdx.Count < enemySpanwPointList.Count);
                GameObject _enemy = ObjectPoolingManager.objPooling.GetEnemy();
                _enemy.transform.position = enemySpanwPointList[e_spawnTrIdx].position;
                _enemy.transform.rotation = Quaternion.identity;
                _enemy.SetActive(true);
                e_Count++;
                if(e_Count >= enemySpanwPointList.Count)
                {
                    e_spawnIdx.Clear();
                    e_Count = 0;
                }
            }

        }
    }
}
