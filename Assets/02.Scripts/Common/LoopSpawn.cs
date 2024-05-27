using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LoopSpawn : MonoBehaviour
{
    [SerializeField] private GunData gunData; // 총 데이터 스크립터블 오브젝트
    private Transform[] spawnPoints; // 아이템 스폰 포인트 배열
    private Transform[] enemySpawnPoints; // 적 스폰 포인트 배열
    private Transform gunSpawnPoint; // 초기 총기 스폰 포인트
    private List<Transform> spawnPointsList = new List<Transform>(); // 아이템 스폰 포인트 리스트
    private List<Transform> enemySpawnPointsList = new List<Transform>(); // 적 스폰 포인트 리스트
    private PlayerDamage playerDamage; // 플레이어 데미지 스크립트 참조
    private int spawnTrIdx; // 아이템 스폰 포인트 인덱스
    private int e_spawnTrIdx; // 적 스폰 포인트 인덱스
    public int e_Count; // 스폰된 적의 수
    private int allSpawnTime; // 아이템 스폰 시간 간격
    private int allItemCount; // 총 스폰된 아이템 수
    private HashSet<int> e_spawnIdx = new HashSet<int>(); // 적 스폰 포인트 인덱스 집합
    private HashSet<int> spawnIdx = new HashSet<int>(); // 아이템 스폰 포인트 인덱스 집합

    void Start()
    {
        playerDamage = FindObjectOfType<PlayerDamage>(); // 플레이어 데미지 스크립트 찾기
        spawnPoints = GameObject.Find("SpawnPointsGroup").GetComponentsInChildren<Transform>(); // 아이템 스폰 포인트들 가져오기
        enemySpawnPoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>(); // 적 스폰 포인트들 가져오기
        gunSpawnPoint = GameObject.Find("GunSpawnPoint").transform; // 초기 총기 스폰 포인트 가져오기
        e_Count = 0; // 초기 스폰된 적의 수 초기화
        allItemCount = 0; // 초기 스폰된 아이템 수 초기화
        spawnIdx.Clear(); // 아이템 스폰 포인트 인덱스 집합 초기화
        for (int i = 1; i < spawnPoints.Length; i++) // 첫 번째 요소는 그룹의 부모이므로 제외하고 리스트에 추가
        {
            spawnPointsList.Add(spawnPoints[i]); // 아이템 스폰 포인트 리스트에 추가
        }
        for (int i = 1; i < enemySpawnPoints.Length; i++) // 첫 번째 요소는 그룹의 부모이므로 제외하고 리스트에 추가
        {
            enemySpawnPointsList.Add(enemySpawnPoints[i]); // 적 스폰 포인트 리스트에 추가
        }
        StartCoroutine(SpawnItem()); // 아이템 스폰 코루틴 시작
    }

    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(2f); // 초기 대기 시간
        Instantiate(gunData.rifle, gunSpawnPoint.position, Quaternion.identity); // 초기 라이플 스폰
        GameObject _rifleBulletBox1 = ObjectPoolingManager.objPooling.GetRifleBulletBox(); // 객체 풀에서 라이플 탄약 상자 가져오기
        _rifleBulletBox1.transform.position = gunSpawnPoint.position + (Vector3.right * 0.5f); // 위치 설정
        _rifleBulletBox1.transform.rotation = Quaternion.identity; // 회전 설정
        _rifleBulletBox1.SetActive(true); // 활성화
        do
        {
            spawnTrIdx = Random.Range(0, spawnPointsList.Count); // 랜덤 스폰 포인트 인덱스 선택
        } while (spawnIdx.Contains(spawnTrIdx)); // 이미 선택된 인덱스는 다시 선택하지 않도록
        Instantiate(gunData.shotgun, spawnPointsList[spawnTrIdx].position, Quaternion.identity); // 초기 샷건 스폰
        spawnIdx.Add(spawnTrIdx); // 선택된 인덱스 추가
        while (!playerDamage.isDie) // 플레이어가 죽지 않는 동안 반복
        {
            allSpawnTime = Random.Range(2, 3); // 랜덤한 시간 간격
            yield return new WaitForSeconds(allSpawnTime); // 대기
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
                spawnTrIdx = Random.Range(0, spawnList.Count); // 랜덤 스폰 포인트 인덱스 선택
            } while (idxSet.Contains(spawnTrIdx) && idxSet.Count < spawnList.Count); // 이미 선택된 인덱스는 다시 선택하지 않도록
            idxSet.Add(spawnTrIdx); // 선택된 인덱스 추가
            item.transform.position = spawnList[spawnTrIdx].position; // 위치 설정
            item.transform.rotation = Quaternion.identity; // 회전 설정
            item.SetActive(true); // 활성화
            itemCount++;
            if (itemCount >= spawnList.Count)
            {
                idxSet.Clear(); // 인덱스 집합 초기화
                itemCount = 0; // 아이템 카운트 초기화
            }
            return true; // 아이템 스폰 성공
        }
        return false; // 아이템 스폰 실패
    }

    private bool SpawnEnemy(GameObject enemy, ref HashSet<int> idxSet, List<Transform> spawnList, ref int enemyCount)
    {
        if (enemy != null)
        {
            do
            {
                e_spawnTrIdx = Random.Range(0, spawnList.Count); // 랜덤 스폰 포인트 인덱스 선택
            } while (idxSet.Contains(e_spawnTrIdx) && idxSet.Count < spawnList.Count); // 이미 선택된 인덱스는 다시 선택하지 않도록
            idxSet.Add(e_spawnTrIdx); // 선택된 인덱스 추가
            enemy.transform.position = spawnList[e_spawnTrIdx].position; // 위치 설정
            enemy.transform.rotation = Quaternion.identity; // 회전 설정
            enemy.SetActive(true); // 활성화
            enemyCount++;
            if (enemyCount >= spawnList.Count)
            {
                idxSet.Clear(); // 인덱스 집합 초기화
                enemyCount = 0; // 적 카운트 초기화
            }
            return true; // 적 스폰 성공
        }
        return false; // 적 스폰 실패
    }
}
