using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LoopSpawn : MonoBehaviour
{
    [SerializeField] private GunData gunData; // �� ������ ��ũ���ͺ� ������Ʈ
    private Transform[] spawnPoints; // ������ ���� ����Ʈ �迭
    private Transform[] enemySpawnPoints; // �� ���� ����Ʈ �迭
    private Transform gunSpawnPoint; // �ʱ� �ѱ� ���� ����Ʈ
    private List<Transform> spawnPointsList = new List<Transform>(); // ������ ���� ����Ʈ ����Ʈ
    private List<Transform> enemySpawnPointsList = new List<Transform>(); // �� ���� ����Ʈ ����Ʈ
    private PlayerDamage playerDamage; // �÷��̾� ������ ��ũ��Ʈ ����
    private int spawnTrIdx; // ������ ���� ����Ʈ �ε���
    private int e_spawnTrIdx; // �� ���� ����Ʈ �ε���
    public int e_Count; // ������ ���� ��
    private int allSpawnTime; // ������ ���� �ð� ����
    private int allItemCount; // �� ������ ������ ��
    private HashSet<int> e_spawnIdx = new HashSet<int>(); // �� ���� ����Ʈ �ε��� ����
    private HashSet<int> spawnIdx = new HashSet<int>(); // ������ ���� ����Ʈ �ε��� ����

    void Start()
    {
        playerDamage = FindObjectOfType<PlayerDamage>(); // �÷��̾� ������ ��ũ��Ʈ ã��
        spawnPoints = GameObject.Find("SpawnPointsGroup").GetComponentsInChildren<Transform>(); // ������ ���� ����Ʈ�� ��������
        enemySpawnPoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>(); // �� ���� ����Ʈ�� ��������
        gunSpawnPoint = GameObject.Find("GunSpawnPoint").transform; // �ʱ� �ѱ� ���� ����Ʈ ��������
        e_Count = 0; // �ʱ� ������ ���� �� �ʱ�ȭ
        allItemCount = 0; // �ʱ� ������ ������ �� �ʱ�ȭ
        spawnIdx.Clear(); // ������ ���� ����Ʈ �ε��� ���� �ʱ�ȭ
        for (int i = 1; i < spawnPoints.Length; i++) // ù ��° ��Ҵ� �׷��� �θ��̹Ƿ� �����ϰ� ����Ʈ�� �߰�
        {
            spawnPointsList.Add(spawnPoints[i]); // ������ ���� ����Ʈ ����Ʈ�� �߰�
        }
        for (int i = 1; i < enemySpawnPoints.Length; i++) // ù ��° ��Ҵ� �׷��� �θ��̹Ƿ� �����ϰ� ����Ʈ�� �߰�
        {
            enemySpawnPointsList.Add(enemySpawnPoints[i]); // �� ���� ����Ʈ ����Ʈ�� �߰�
        }
        StartCoroutine(SpawnItem()); // ������ ���� �ڷ�ƾ ����
    }

    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(2f); // �ʱ� ��� �ð�
        Instantiate(gunData.rifle, gunSpawnPoint.position, Quaternion.identity); // �ʱ� ������ ����
        GameObject _rifleBulletBox1 = ObjectPoolingManager.objPooling.GetRifleBulletBox(); // ��ü Ǯ���� ������ ź�� ���� ��������
        _rifleBulletBox1.transform.position = gunSpawnPoint.position + (Vector3.right * 0.5f); // ��ġ ����
        _rifleBulletBox1.transform.rotation = Quaternion.identity; // ȸ�� ����
        _rifleBulletBox1.SetActive(true); // Ȱ��ȭ
        do
        {
            spawnTrIdx = Random.Range(0, spawnPointsList.Count); // ���� ���� ����Ʈ �ε��� ����
        } while (spawnIdx.Contains(spawnTrIdx)); // �̹� ���õ� �ε����� �ٽ� �������� �ʵ���
        Instantiate(gunData.shotgun, spawnPointsList[spawnTrIdx].position, Quaternion.identity); // �ʱ� ���� ����
        spawnIdx.Add(spawnTrIdx); // ���õ� �ε��� �߰�
        while (!playerDamage.isDie) // �÷��̾ ���� �ʴ� ���� �ݺ�
        {
            allSpawnTime = Random.Range(2, 3); // ������ �ð� ����
            yield return new WaitForSeconds(allSpawnTime); // ���
            if (SpawnItem(ObjectPoolingManager.objPooling.GetRifleBulletBox(), ref spawnIdx, spawnPointsList, ref allItemCount)) continue;
            if (SpawnItem(ObjectPoolingManager.objPooling.GetShotGunBulletBox(), ref spawnIdx, spawnPointsList, ref allItemCount)) continue;
            if (SpawnItem(ObjectPoolingManager.objPooling.GetMadicine(), ref spawnIdx, spawnPointsList, ref allItemCount)) continue;
            if (SpawnItem(ObjectPoolingManager.objPooling.GetSpawnGranade(), ref spawnIdx, spawnPointsList, ref allItemCount)) continue;
            if (SpawnEnemy(ObjectPoolingManager.objPooling.GetEnemy(), ref e_spawnIdx, enemySpawnPointsList, ref e_Count)) continue;
        }
    }

    private bool SpawnItem(GameObject item, ref HashSet<int> idxSet, List<Transform> spawnList, ref int itemCount)
    {
        if (item != null)
        {
            do
            {
                spawnTrIdx = Random.Range(0, spawnList.Count); // ���� ���� ����Ʈ �ε��� ����
            } while (idxSet.Contains(spawnTrIdx) && idxSet.Count < spawnList.Count); // �̹� ���õ� �ε����� �ٽ� �������� �ʵ���
            idxSet.Add(spawnTrIdx); // ���õ� �ε��� �߰�
            item.transform.position = spawnList[spawnTrIdx].position; // ��ġ ����
            item.transform.rotation = Quaternion.identity; // ȸ�� ����
            item.SetActive(true); // Ȱ��ȭ
            itemCount++;
            if (itemCount >= spawnList.Count)
            {
                idxSet.Clear(); // �ε��� ���� �ʱ�ȭ
                itemCount = 0; // ������ ī��Ʈ �ʱ�ȭ
            }
            return true; // ������ ���� ����
        }
        return false; // ������ ���� ����
    }

    private bool SpawnEnemy(GameObject enemy, ref HashSet<int> idxSet, List<Transform> spawnList, ref int enemyCount)
    {
        if (enemy != null)
        {
            do
            {
                e_spawnTrIdx = Random.Range(0, spawnList.Count); // ���� ���� ����Ʈ �ε��� ����
            } while (idxSet.Contains(e_spawnTrIdx) && idxSet.Count < spawnList.Count); // �̹� ���õ� �ε����� �ٽ� �������� �ʵ���
            idxSet.Add(e_spawnTrIdx); // ���õ� �ε��� �߰�
            enemy.transform.position = spawnList[e_spawnTrIdx].position; // ��ġ ����
            enemy.transform.rotation = Quaternion.identity; // ȸ�� ����
            enemy.SetActive(true); // Ȱ��ȭ
            enemyCount++;
            if (enemyCount >= spawnList.Count)
            {
                idxSet.Clear(); // �ε��� ���� �ʱ�ȭ
                enemyCount = 0; // �� ī��Ʈ �ʱ�ȭ
            }
            return true; // �� ���� ����
        }
        return false; // �� ���� ����
    }
}
