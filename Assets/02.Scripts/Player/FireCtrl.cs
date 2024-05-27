using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] private GunData gunData;           // 총 스크립터블 데이터
    [SerializeField] private GranadeData granadeData;   // 수류탄 스크립터블 데이터
    private Transform rifleFirePos;                     // 라이플 총이 발사될 트랜스폼
    private Transform shotgunFirePos;                   // 샷건이 발사될 트랜스폼
    private Transform granadePos;                       // 수류탄이 던져질 트랜스폼
    private AudioSource source;                         // 오디오 소스
    private Animator animator;                          // 애니메이터
    private PlayerDamage playerDamage;                  // 플레이어가 받을 데미지
    private ParticleSystem rifleFlash;                  // 라이플 발사 시 플레이될 파티클
    private ParticleSystem shotgunFlash;                // 샷건 발사 시 플레이될 파티클
    private CanvasGroup canvasGroup;                    // 인벤토리를 보이거나 안 보이게 함
    public Image weaponImage;                           // 총을 바꿀 때 UI에 보이게 할 이미지
    public Text bulletText;                             // 총알 개수를 텍스트로 표시
    private RectTransform itemEmptyGroup;               // 총알을 다 썼을 때 인벤토리의 그룹
    private MeshRenderer rifleMesh;                     // 총을 바꿀 때 나타날 라이플 메쉬
    private MeshRenderer shotgunMesh;                   // 총을 바꿀 때 나타날 샷건 메쉬
    public MeshRenderer granadeMesh;                    // 수류탄으로 바꿀 때 나타날 수류탄 메쉬
    private bool tabOn;                                 // 인벤토리를 켰는지 여부
    private float curTime;                              // 현재 시간 (총 발사 시간 계산용)
    private float fireTime;                             // 총 발사 시간
    private readonly int aniFire = Animator.StringToHash("FireTrigger");         // 발사 애니메이션 해시
    private readonly int aniReload = Animator.StringToHash("ReloadTrigger");     // 재장전 애니메이션 해시
    private readonly int aniIsReload = Animator.StringToHash("IsReload");        // 재장전 중 여부 애니메이션 해시
    private readonly int aniIsGun = Animator.StringToHash("IsGun");              // 총기 변경 애니메이션 해시
    private readonly int aniGranade = Animator.StringToHash("GranadeTrigger");   // 수류탄 애니메이션 해시
    public int rifleBulletCount;                        // 라이플 총알 개수
    private int rifleBulletMaxCount;                    // 라이플 총알 최대 개수
    public int shotgunBulletCount;                      // 샷건 총알 개수
    private int shotgunBulletMaxCount;                  // 샷건 총알 최대 개수
    private bool isReload;                              // 재장전 여부
    private bool isThrow;                               // 수류탄 던짐 여부
    public bool isGranade;                              // 수류탄 사용 여부
    public bool isRifle;                                // 라이플 사용 여부
    public bool isShotGun;                              // 샷건 사용 여부

    void OnEnable()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();                 // 자식 오브젝트의 애니메이터 가져오기
        rifleFirePos = GameObject.Find("FirePos_Rifle").transform;                 // 라이플 발사 위치 가져오기
        shotgunFirePos = GameObject.Find("FirePos_ShotGun").transform;             // 샷건 발사 위치 가져오기
        granadePos = GameObject.Find("GrandePos").transform;                       // 수류탄 발사 위치 가져오기

        rifleFlash = rifleFirePos.GetChild(0).GetComponent<ParticleSystem>();      // 라이플 발사 시 파티클 시스템 가져오기
        shotgunFlash = shotgunFirePos.GetChild(0).GetComponent<ParticleSystem>();  // 샷건 발사 시 파티클 시스템 가져오기

        rifleMesh = rifleFirePos.parent.GetComponent<MeshRenderer>();              // 라이플 메쉬 가져오기
        shotgunMesh = shotgunFirePos.parent.GetComponent<MeshRenderer>();          // 샷건 메쉬 가져오기
        granadeMesh = GameObject.Find("SCIGrenade").GetComponent<MeshRenderer>();  // 수류탄 메쉬 가져오기

        itemEmptyGroup = GameObject.Find("Item_EmptyGroup").GetComponent<RectTransform>();  // 빈 아이템 그룹 가져오기
        source = GetComponent<AudioSource>();                                      // 오디오 소스 가져오기
        canvasGroup = GameObject.Find("Canvas_ui").transform.GetChild(0).GetComponent<CanvasGroup>();  // 캔버스 그룹 가져오기
        weaponImage = GameObject.Find("Panel-Weapon").transform.GetChild(0).GetComponent<Image>();    // 무기 이미지 가져오기
        bulletText = weaponImage.transform.parent.GetChild(1).GetComponent<Text>();  // 총알 텍스트 가져오기
        playerDamage = GetComponent<PlayerDamage>();                               // 플레이어 데미지 컴포넌트 가져오기

        curTime = Time.time;                        // 현재 시간 초기화
        fireTime = 0.1f;                            // 발사 시간 초기화
        rifleBulletMaxCount = 30;                   // 라이플 최대 총알 수 초기화
        rifleBulletCount = gunData.Rf_Count;        // 라이플 총알 수 초기화
        shotgunBulletMaxCount = 10;                 // 샷건 최대 총알 수 초기화
        shotgunBulletCount = gunData.Sg_Count;      // 샷건 총알 수 초기화

        Cursor.lockState = CursorLockMode.Locked;   // 커서 잠금 설정
        isThrow = false;                            // 수류탄 던짐 여부 초기화
        Cursor.visible = false;                     // 커서 보이지 않게 설정
        isGranade = false;                          // 수류탄 사용 여부 초기화
        isShotGun = false;                          // 샷건 사용 여부 초기화
        isRifle = false;                            // 라이플 사용 여부 초기화
        tabOn = false;                              // 인벤토리 여부 초기화
        isReload = false;                           // 재장전 여부 초기화

        rifleFlash.Stop();                          // 라이플 파티클 중지
        shotgunFlash.Stop();                        // 샷건 파티클 중지

        StartCoroutine(OnFire());                   // 발사 코루틴 시작

        granadeData.Count = 0;                      // 수류탄 개수 초기화
        gunData.Rf_Count = 0;                       // 라이플 총알 수 초기화
        gunData.Sg_Count = 0;                       // 샷건 총알 수 초기화
    }

    IEnumerator OnFire()
    {
        while (!playerDamage.isDie)                      // 플레이어가 죽지 않았을 때
        {
            yield return new WaitForSeconds(0.002f);     // 0.002초 대기
            ChangeWeapon();                              // 무기 변경 함수 호출
            if (isRifle) RifleFireAndReload();           // 라이플 사용 시 발사 및 재장전 함수 호출
            else rifleMesh.enabled = false;              // 라이플 메쉬 비활성화
            if (isShotGun) ShotGunFireAndReload();       // 샷건 사용 시 발사 및 재장전 함수 호출
            else shotgunMesh.enabled = false;            // 샷건 메쉬 비활성화
            if (isGranade) GranadeThrow();               // 수류탄 사용 시 던지기 함수 호출
            else granadeMesh.enabled = false;            // 수류탄 메쉬 비활성화
            TabInventory();                              // 인벤토리 함수 호출
        }
    }

    private void TabInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) tabOn = !tabOn;                    // Tab 키 입력 시 인벤토리 상태 변경
        if (tabOn)                                                           // 인벤토리 열렸을 때
        {
            Cursor.lockState = CursorLockMode.None;                          // 커서 잠금 해제
            Cursor.visible = true;                                           // 커서 보이게 설정
            canvasGroup.alpha = 1;                                           // 캔버스 그룹 알파값 설정
        }
        else                                                                 // 인벤토리 닫혔을 때
        {
            Cursor.lockState = CursorLockMode.Locked;                        // 커서 잠금 설정
            Cursor.visible = false;                                          // 커서 숨기기
            canvasGroup.alpha = 0;                                           // 캔버스 그룹 알파값 설정
        }
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.Instance.getRifle) ChangeRifle();          // 라이플 변경
        else if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.Instance.getShotGun) ChangeShotGun();  // 샷건 변경
        else if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.Instance.getGranade) ChangeGranade();  // 수류탄 변경
    }

    private void RifleFireAndReload()
    {
        if (Time.time - curTime > fireTime && Input.GetMouseButton(0) && isRifle)                      // 발사 조건
        {
            if (!isReload && !tabOn && rifleBulletCount > 0)                                           // 재장전 중이 아니고 인벤토리 열리지 않았고 총알이 남아 있을 때
            {
                ShootFire(rifleFlash, "RifleFlashStop", rifleBulletCount, gunData.rifleClip, gunData.Rf_Count);  // 발사 함수 호출
                rifleBulletCount--;                                                                    // 총알 수 감소
                if (rifleBulletCount == 0 & gunData.Rf_Count > 0) StartCoroutine(RifleReload());       // 총알이 없고 예비 탄환이 있을 때 재장전
            }
            curTime = Time.time;                                                                       // 현재 시간 업데이트
        }
        if (Input.GetKeyDown(KeyCode.R) && rifleBulletCount != gunData.Rf_Count && !isReload & gunData.Rf_Count > 0 && rifleBulletCount < 30 && isRifle)
            StartCoroutine(RifleReload());                                                             // 재장전 키 입력 시 재장전 조건 만족하면 재장전
    }

    private void ShotGunFireAndReload()
    {
        if (Input.GetMouseButtonDown(0) && isShotGun)                                                  // 샷건 발사 조건
        {
            if (!isReload && !tabOn && shotgunBulletCount > 0)                                         // 재장전 중이 아니고 인벤토리 열리지 않았고 총알이 남아 있을 때
            {
                ShootFire(shotgunFlash, "ShotGunFlashStop", shotgunBulletCount, gunData.shotgunClip, gunData.Sg_Count);  // 발사 함수 호출
                shotgunBulletCount--;                                                                  // 총알 수 감소
                if (shotgunBulletCount == 0 && gunData.Sg_Count > 0) StartCoroutine(ShotGunReload());  // 총알이 없고 예비 탄환이 있을 때 재장전
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && shotgunBulletCount != gunData.Sg_Count && !isReload & gunData.Sg_Count > 0 && shotgunBulletCount < 10 && isShotGun)
            StartCoroutine(ShotGunReload());                                                           // 재장전 키 입력 시 재장전 조건 만족하면 재장전
    }

    private void GranadeThrow()
    {
        if (Input.GetMouseButtonDown(0) && isGranade && !isThrow)                                      // 수류탄 던지기 조건
        {
            if (granadeData.Count > 0 && !tabOn)                                                       // 수류탄 개수가 남아 있고 인벤토리 열리지 않았을 때
            {
                animator.SetTrigger(aniGranade);                                                       // 수류탄 애니메이션 트리거
                StartCoroutine(ThrowGranade());                                                        // 수류탄 던지기 코루틴 시작
            }
        }
        else if (granadeData.Count == 0)                                                              // 수류탄 개수가 0일 때
        {
            granadeMesh.enabled = false;                                                              // 수류탄 메쉬 비활성화
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.granadeIdx].SetParent(itemEmptyGroup);  // 수류탄 인벤토리 설정
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.granadeIdx].GetComponent<Image>().enabled = false;  // 이미지 비활성화
            GameManager.Instance.itemEmptyText[GameManager.Instance.granadeIdx].enabled = false;       // 텍스트 비활성화
            GameManager.Instance.getGranade = false;                                                  // 수류탄 보유 상태 업데이트
        }
    }

    IEnumerator ThrowGranade()
    {
        isThrow = true;                                                                                // 던짐 상태 업데이트
        granadeData.Count--;                                                                           // 수류탄 개수 감소
        yield return new WaitForSeconds(1.8f);                                                         // 대기
        SoundManager.soundInst.PlayeOneShot(granadeData.throwClip, source);                            // 수류탄 소리 재생
        bulletText.text = granadeData.Count.ToString();                                                // 총알 텍스트 업데이트
        GameObject _granade = ObjectPoolingManager.objPooling.GetWeaponGranade();                      // 수류탄 객체 풀에서 가져오기
        _granade.transform.position = granadePos.position;                                             // 수류탄 위치 설정
        _granade.transform.rotation = granadePos.rotation;                                             // 수류탄 회전 설정
        _granade.SetActive(true);                                                                      // 수류탄 활성화
        GameManager.Instance.itemEmptyText[GameManager.Instance.granadeIdx].text = gunData.Rf_Count.ToString();  // 텍스트 업데이트
        yield return new WaitForSeconds(2f);                                                           // 대기
        isThrow = false;                                                                               // 던짐 상태 업데이트
    }

    public void ChangeGranade()
    {
        isGranade = true;                                                                               // 수류탄 사용 상태 업데이트
        isRifle = false;                                                                                // 라이플 사용 상태 업데이트
        isShotGun = false;                                                                              // 샷건 사용 상태 업데이트
        granadeMesh.enabled = true;                                                                     // 수류탄 메쉬 활성화
        animator.SetBool(aniIsGun, false);                                                              // 총기 애니메이션 상태 업데이트
        bulletText.text = granadeData.Count.ToString();                                                 // 총알 텍스트 업데이트
        weaponImage.sprite = granadeData.ui_Sprite;                                                     // 무기 이미지 업데이트
    }

    public void ChangeShotGun()
    {
        animator.SetBool(aniIsGun, true);                                                               // 총기 애니메이션 상태 업데이트
        isRifle = false;                                                                                // 라이플 사용 상태 업데이트
        isGranade = false;                                                                              // 수류탄 사용 상태 업데이트
        isShotGun = true;                                                                               // 샷건 사용 상태 업데이트
        rifleMesh.enabled = false;                                                                      // 라이플 메쉬 비활성화
        shotgunMesh.enabled = true;                                                                     // 샷건 메쉬 활성화
        gunData.g_damage = 50;                                                                          // 샷건 데미지 설정
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();          // 총알 텍스트 업데이트
        weaponImage.sprite = gunData.sg_UISprite;                                                       // 무기 이미지 업데이트
    }

    public void ChangeRifle()
    {
        animator.SetBool(aniIsGun, true);                                                               // 총기 애니메이션 상태 업데이트
        isShotGun = false;                                                                              // 샷건 사용 상태 업데이트
        isRifle = true;                                                                                 // 라이플 사용 상태 업데이트
        isGranade = false;                                                                              // 수류탄 사용 상태 업데이트
        shotgunMesh.enabled = false;                                                                    // 샷건 메쉬 비활성화
        rifleMesh.enabled = true;                                                                       // 라이플 메쉬 활성화
        gunData.g_damage = 15;                                                                          // 라이플 데미지 설정
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();            // 총알 텍스트 업데이트
        weaponImage.sprite = gunData.rf_UISprite;                                                       // 무기 이미지 업데이트
    }

    void ShootFire(ParticleSystem particle, string stopFunction, int bulletCount, AudioClip clip, int itemDatabaseCount)
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();                         // 총알 객체 풀에서 가져오기
        _bullet.transform.position = shotgunFirePos.position;                                           // 총알 위치 설정
        _bullet.transform.rotation = shotgunFirePos.rotation;                                           // 총알 회전 설정
        _bullet.SetActive(true);                                                                        // 총알 활성화
        particle.Play();                                                                                // 파티클 재생
        Invoke(stopFunction, 0.1f);                                                                     // 파티클 중지 함수 호출
        animator.SetTrigger(aniFire);                                                                   // 발사 애니메이션 트리거
        bulletText.text = bulletCount.ToString() + " / " + itemDatabaseCount.ToString();                // 총알 텍스트 업데이트
        SoundManager.soundInst.PlayeOneShot(clip, source);                                              // 발사 소리 재생
    }

    IEnumerator RifleReload()
    {
        isReload = true;                                                                               // 재장전 상태 업데이트
        animator.SetTrigger(aniReload);                                                                // 재장전 애니메이션 트리거
        animator.SetBool(aniIsReload, true);                                                           // 재장전 애니메이션 상태 업데이트
        yield return new WaitForSeconds(1.55f);                                                        // 대기
        SoundManager.soundInst.PlayeOneShot(gunData.rifleReloadClip, source);                          // 재장전 소리 재생
        animator.SetBool(aniIsReload, false);                                                          // 재장전 애니메이션 상태 업데이트
        isReload = false;                                                                              // 재장전 상태 업데이트

        if (gunData.Rf_Count >= rifleBulletMaxCount)                                                  // 예비 탄환이 최대 탄환 수 이상일 때
        {
            if (rifleBulletCount == 0)                                                                // 총알이 없을 때
            {
                rifleBulletCount += rifleBulletMaxCount;                                              // 총알 수 업데이트
                gunData.Rf_Count -= rifleBulletCount;                                                 // 예비 탄환 수 업데이트
            }
            else if (rifleBulletCount > 0)                                                            // 총알이 있을 때
            {
                int cot = rifleBulletMaxCount;                                                        // 최대 탄환 수 변수 설정
                cot -= rifleBulletCount;                                                              // 남은 탄환 계산
                rifleBulletCount += cot;                                                              // 총알 수 업데이트
                gunData.Rf_Count -= cot;                                                              // 예비 탄환 수 업데이트
            }
        }
        else if (gunData.Rf_Count < rifleBulletMaxCount)                                              // 예비 탄환이 최대 탄환 수 이하일 때
        {
            if (rifleBulletCount == 0)                                                                // 총알이 없을 때
            {
                rifleBulletCount += gunData.Rf_Count;                                                 // 총알 수 업데이트
                gunData.Rf_Count -= rifleBulletCount;                                                 // 예비 탄환 수 업데이트
            }
            else if (rifleBulletCount > 0)                                                            // 총알이 있을 때
            {
                int let = rifleBulletMaxCount;                                                        // 최대 탄환 수 변수 설정
                let -= rifleBulletCount;                                                              // 남은 탄환 계산
                if (gunData.Rf_Count >= let)                                                          // 예비 탄환이 남은 탄환 이상일 때
                {
                    rifleBulletCount += let;                                                          // 총알 수 업데이트
                    gunData.Rf_Count -= let;                                                          // 예비 탄환 수 업데이트
                }
                else if (gunData.Rf_Count < let)                                                      // 예비 탄환이 남은 탄환 이하일 때
                {
                    rifleBulletCount += gunData.Rf_Count;                                             // 총알 수 업데이트
                    gunData.Rf_Count -= gunData.Rf_Count;                                             // 예비 탄환 수 업데이트
                }
            }
        }
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();          // 총알 텍스트 업데이트
        GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].text = gunData.Rf_Count.ToString();  // 예비 탄환 텍스트 업데이트

        if (gunData.Rf_Count == 0)                                                                    // 예비 탄환이 없을 때
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.rifleBulletIdx].SetParent(itemEmptyGroup);  // 인벤토리 업데이트
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.rifleBulletIdx].GetComponent<Image>().enabled = false;  // 이미지 비활성화
            GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].enabled = false;   // 텍스트 비활성화
        }
    }

    IEnumerator ShotGunReload()
    {
        isReload = true;                                                                               // 재장전 상태 업데이트
        animator.SetTrigger(aniReload);                                                                // 재장전 애니메이션 트리거
        animator.SetBool(aniIsReload, true);                                                           // 재장전 애니메이션 상태 업데이트
        yield return new WaitForSeconds(1.55f);                                                        // 대기
        SoundManager.soundInst.PlayeOneShot(gunData.shotgunReloadClip, source);                        // 재장전 소리 재생
        animator.SetBool(aniIsReload, false);                                                          // 재장전 애니메이션 상태 업데이트
        isReload = false;                                                                              // 재장전 상태 업데이트

        if (gunData.Sg_Count >= shotgunBulletMaxCount)                                                 // 예비 탄환이 최대 탄환 수 이상일 때
        {
            if (shotgunBulletCount == 0)                                                               // 총알이 없을 때
            {
                shotgunBulletCount += shotgunBulletMaxCount;                                           // 총알 수 업데이트
                gunData.Sg_Count -= shotgunBulletCount;                                                // 예비 탄환 수 업데이트
            }
            else if (shotgunBulletCount > 0)                                                           // 총알이 있을 때
            {
                int shot = shotgunBulletMaxCount;                                                      // 최대 탄환 수 변수 설정
                shot -= shotgunBulletCount;                                                            // 남은 탄환 계산
                shotgunBulletCount += shot;                                                            // 총알 수 업데이트
                gunData.Sg_Count -= shot;                                                              // 예비 탄환 수 업데이트
            }
        }
        else if (gunData.Sg_Count < shotgunBulletMaxCount)                                             // 예비 탄환이 최대 탄환 수 이하일 때
        {
            if (shotgunBulletCount == 0)                                                               // 총알이 없을 때
            {
                shotgunBulletCount += gunData.Sg_Count;                                                // 총알 수 업데이트
                gunData.Sg_Count -= shotgunBulletCount;                                                // 예비 탄환 수 업데이트
            }
            else if (shotgunBulletCount > 0)                                                           // 총알이 있을 때
            {
                int gun = shotgunBulletMaxCount;                                                       // 최대 탄환 수 변수 설정
                gun -= shotgunBulletCount;                                                             // 남은 탄환 계산
                if (gunData.Sg_Count >= gun)                                                           // 예비 탄환이 남은 탄환 이상일 때
                {
                    shotgunBulletCount += gun;                                                         // 총알 수 업데이트
                    gunData.Sg_Count -= gun;                                                           // 예비 탄환 수 업데이트
                }
                else if (gunData.Sg_Count < gun)                                                       // 예비 탄환이 남은 탄환 이하일 때
                {
                    shotgunBulletCount += gunData.Sg_Count;                                            // 총알 수 업데이트
                    gunData.Sg_Count -= gunData.Sg_Count;                                              // 예비 탄환 수 업데이트
                }
            }
        }
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();          // 총알 텍스트 업데이트
        GameManager.Instance.itemEmptyText[GameManager.Instance.shotgunBulletIdx].text = gunData.Sg_Count.ToString();  // 예비 탄환 텍스트 업데이트

        if (gunData.Sg_Count == 0)                                                                     // 예비 탄환이 없을 때
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.shotgunBulletIdx].SetParent(itemEmptyGroup);  // 인벤토리 업데이트
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.shotgunBulletIdx].GetComponent<Image>().enabled = false;  // 이미지 비활성화
            GameManager.Instance.itemEmptyText[GameManager.Instance.shotgunBulletIdx].enabled = false;  // 텍스트 비활성화
        }
    }

    void RifleFlashStop()
    {
        rifleFlash.Stop();                                                                              // 라이플 파티클 중지
    }

    void ShotGunFlashStop()
    {
        shotgunFlash.Stop();                                                                            // 샷건 파티클 중지
    }
}