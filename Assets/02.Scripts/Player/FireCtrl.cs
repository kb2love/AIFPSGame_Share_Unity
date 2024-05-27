using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] private GunData gunData;           // �� ��ũ���ͺ� ������
    [SerializeField] private GranadeData granadeData;   // ����ź ��ũ���ͺ� ������
    private Transform rifleFirePos;                     // ������ ���� �߻�� Ʈ������
    private Transform shotgunFirePos;                   // ������ �߻�� Ʈ������
    private Transform granadePos;                       // ����ź�� ������ Ʈ������
    private AudioSource source;                         // ����� �ҽ�
    private Animator animator;                          // �ִϸ�����
    private PlayerDamage playerDamage;                  // �÷��̾ ���� ������
    private ParticleSystem rifleFlash;                  // ������ �߻� �� �÷��̵� ��ƼŬ
    private ParticleSystem shotgunFlash;                // ���� �߻� �� �÷��̵� ��ƼŬ
    private CanvasGroup canvasGroup;                    // �κ��丮�� ���̰ų� �� ���̰� ��
    public Image weaponImage;                           // ���� �ٲ� �� UI�� ���̰� �� �̹���
    public Text bulletText;                             // �Ѿ� ������ �ؽ�Ʈ�� ǥ��
    private RectTransform itemEmptyGroup;               // �Ѿ��� �� ���� �� �κ��丮�� �׷�
    private MeshRenderer rifleMesh;                     // ���� �ٲ� �� ��Ÿ�� ������ �޽�
    private MeshRenderer shotgunMesh;                   // ���� �ٲ� �� ��Ÿ�� ���� �޽�
    public MeshRenderer granadeMesh;                    // ����ź���� �ٲ� �� ��Ÿ�� ����ź �޽�
    private bool tabOn;                                 // �κ��丮�� �״��� ����
    private float curTime;                              // ���� �ð� (�� �߻� �ð� ����)
    private float fireTime;                             // �� �߻� �ð�
    private readonly int aniFire = Animator.StringToHash("FireTrigger");         // �߻� �ִϸ��̼� �ؽ�
    private readonly int aniReload = Animator.StringToHash("ReloadTrigger");     // ������ �ִϸ��̼� �ؽ�
    private readonly int aniIsReload = Animator.StringToHash("IsReload");        // ������ �� ���� �ִϸ��̼� �ؽ�
    private readonly int aniIsGun = Animator.StringToHash("IsGun");              // �ѱ� ���� �ִϸ��̼� �ؽ�
    private readonly int aniGranade = Animator.StringToHash("GranadeTrigger");   // ����ź �ִϸ��̼� �ؽ�
    public int rifleBulletCount;                        // ������ �Ѿ� ����
    private int rifleBulletMaxCount;                    // ������ �Ѿ� �ִ� ����
    public int shotgunBulletCount;                      // ���� �Ѿ� ����
    private int shotgunBulletMaxCount;                  // ���� �Ѿ� �ִ� ����
    private bool isReload;                              // ������ ����
    private bool isThrow;                               // ����ź ���� ����
    public bool isGranade;                              // ����ź ��� ����
    public bool isRifle;                                // ������ ��� ����
    public bool isShotGun;                              // ���� ��� ����

    void OnEnable()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();                 // �ڽ� ������Ʈ�� �ִϸ����� ��������
        rifleFirePos = GameObject.Find("FirePos_Rifle").transform;                 // ������ �߻� ��ġ ��������
        shotgunFirePos = GameObject.Find("FirePos_ShotGun").transform;             // ���� �߻� ��ġ ��������
        granadePos = GameObject.Find("GrandePos").transform;                       // ����ź �߻� ��ġ ��������

        rifleFlash = rifleFirePos.GetChild(0).GetComponent<ParticleSystem>();      // ������ �߻� �� ��ƼŬ �ý��� ��������
        shotgunFlash = shotgunFirePos.GetChild(0).GetComponent<ParticleSystem>();  // ���� �߻� �� ��ƼŬ �ý��� ��������

        rifleMesh = rifleFirePos.parent.GetComponent<MeshRenderer>();              // ������ �޽� ��������
        shotgunMesh = shotgunFirePos.parent.GetComponent<MeshRenderer>();          // ���� �޽� ��������
        granadeMesh = GameObject.Find("SCIGrenade").GetComponent<MeshRenderer>();  // ����ź �޽� ��������

        itemEmptyGroup = GameObject.Find("Item_EmptyGroup").GetComponent<RectTransform>();  // �� ������ �׷� ��������
        source = GetComponent<AudioSource>();                                      // ����� �ҽ� ��������
        canvasGroup = GameObject.Find("Canvas_ui").transform.GetChild(0).GetComponent<CanvasGroup>();  // ĵ���� �׷� ��������
        weaponImage = GameObject.Find("Panel-Weapon").transform.GetChild(0).GetComponent<Image>();    // ���� �̹��� ��������
        bulletText = weaponImage.transform.parent.GetChild(1).GetComponent<Text>();  // �Ѿ� �ؽ�Ʈ ��������
        playerDamage = GetComponent<PlayerDamage>();                               // �÷��̾� ������ ������Ʈ ��������

        curTime = Time.time;                        // ���� �ð� �ʱ�ȭ
        fireTime = 0.1f;                            // �߻� �ð� �ʱ�ȭ
        rifleBulletMaxCount = 30;                   // ������ �ִ� �Ѿ� �� �ʱ�ȭ
        rifleBulletCount = gunData.Rf_Count;        // ������ �Ѿ� �� �ʱ�ȭ
        shotgunBulletMaxCount = 10;                 // ���� �ִ� �Ѿ� �� �ʱ�ȭ
        shotgunBulletCount = gunData.Sg_Count;      // ���� �Ѿ� �� �ʱ�ȭ

        Cursor.lockState = CursorLockMode.Locked;   // Ŀ�� ��� ����
        isThrow = false;                            // ����ź ���� ���� �ʱ�ȭ
        Cursor.visible = false;                     // Ŀ�� ������ �ʰ� ����
        isGranade = false;                          // ����ź ��� ���� �ʱ�ȭ
        isShotGun = false;                          // ���� ��� ���� �ʱ�ȭ
        isRifle = false;                            // ������ ��� ���� �ʱ�ȭ
        tabOn = false;                              // �κ��丮 ���� �ʱ�ȭ
        isReload = false;                           // ������ ���� �ʱ�ȭ

        rifleFlash.Stop();                          // ������ ��ƼŬ ����
        shotgunFlash.Stop();                        // ���� ��ƼŬ ����

        StartCoroutine(OnFire());                   // �߻� �ڷ�ƾ ����

        granadeData.Count = 0;                      // ����ź ���� �ʱ�ȭ
        gunData.Rf_Count = 0;                       // ������ �Ѿ� �� �ʱ�ȭ
        gunData.Sg_Count = 0;                       // ���� �Ѿ� �� �ʱ�ȭ
    }

    IEnumerator OnFire()
    {
        while (!playerDamage.isDie)                      // �÷��̾ ���� �ʾ��� ��
        {
            yield return new WaitForSeconds(0.002f);     // 0.002�� ���
            ChangeWeapon();                              // ���� ���� �Լ� ȣ��
            if (isRifle) RifleFireAndReload();           // ������ ��� �� �߻� �� ������ �Լ� ȣ��
            else rifleMesh.enabled = false;              // ������ �޽� ��Ȱ��ȭ
            if (isShotGun) ShotGunFireAndReload();       // ���� ��� �� �߻� �� ������ �Լ� ȣ��
            else shotgunMesh.enabled = false;            // ���� �޽� ��Ȱ��ȭ
            if (isGranade) GranadeThrow();               // ����ź ��� �� ������ �Լ� ȣ��
            else granadeMesh.enabled = false;            // ����ź �޽� ��Ȱ��ȭ
            TabInventory();                              // �κ��丮 �Լ� ȣ��
        }
    }

    private void TabInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) tabOn = !tabOn;                    // Tab Ű �Է� �� �κ��丮 ���� ����
        if (tabOn)                                                           // �κ��丮 ������ ��
        {
            Cursor.lockState = CursorLockMode.None;                          // Ŀ�� ��� ����
            Cursor.visible = true;                                           // Ŀ�� ���̰� ����
            canvasGroup.alpha = 1;                                           // ĵ���� �׷� ���İ� ����
        }
        else                                                                 // �κ��丮 ������ ��
        {
            Cursor.lockState = CursorLockMode.Locked;                        // Ŀ�� ��� ����
            Cursor.visible = false;                                          // Ŀ�� �����
            canvasGroup.alpha = 0;                                           // ĵ���� �׷� ���İ� ����
        }
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.Instance.getRifle) ChangeRifle();          // ������ ����
        else if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.Instance.getShotGun) ChangeShotGun();  // ���� ����
        else if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.Instance.getGranade) ChangeGranade();  // ����ź ����
    }

    private void RifleFireAndReload()
    {
        if (Time.time - curTime > fireTime && Input.GetMouseButton(0) && isRifle)                      // �߻� ����
        {
            if (!isReload && !tabOn && rifleBulletCount > 0)                                           // ������ ���� �ƴϰ� �κ��丮 ������ �ʾҰ� �Ѿ��� ���� ���� ��
            {
                ShootFire(rifleFlash, "RifleFlashStop", rifleBulletCount, gunData.rifleClip, gunData.Rf_Count);  // �߻� �Լ� ȣ��
                rifleBulletCount--;                                                                    // �Ѿ� �� ����
                if (rifleBulletCount == 0 & gunData.Rf_Count > 0) StartCoroutine(RifleReload());       // �Ѿ��� ���� ���� źȯ�� ���� �� ������
            }
            curTime = Time.time;                                                                       // ���� �ð� ������Ʈ
        }
        if (Input.GetKeyDown(KeyCode.R) && rifleBulletCount != gunData.Rf_Count && !isReload & gunData.Rf_Count > 0 && rifleBulletCount < 30 && isRifle)
            StartCoroutine(RifleReload());                                                             // ������ Ű �Է� �� ������ ���� �����ϸ� ������
    }

    private void ShotGunFireAndReload()
    {
        if (Input.GetMouseButtonDown(0) && isShotGun)                                                  // ���� �߻� ����
        {
            if (!isReload && !tabOn && shotgunBulletCount > 0)                                         // ������ ���� �ƴϰ� �κ��丮 ������ �ʾҰ� �Ѿ��� ���� ���� ��
            {
                ShootFire(shotgunFlash, "ShotGunFlashStop", shotgunBulletCount, gunData.shotgunClip, gunData.Sg_Count);  // �߻� �Լ� ȣ��
                shotgunBulletCount--;                                                                  // �Ѿ� �� ����
                if (shotgunBulletCount == 0 && gunData.Sg_Count > 0) StartCoroutine(ShotGunReload());  // �Ѿ��� ���� ���� źȯ�� ���� �� ������
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && shotgunBulletCount != gunData.Sg_Count && !isReload & gunData.Sg_Count > 0 && shotgunBulletCount < 10 && isShotGun)
            StartCoroutine(ShotGunReload());                                                           // ������ Ű �Է� �� ������ ���� �����ϸ� ������
    }

    private void GranadeThrow()
    {
        if (Input.GetMouseButtonDown(0) && isGranade && !isThrow)                                      // ����ź ������ ����
        {
            if (granadeData.Count > 0 && !tabOn)                                                       // ����ź ������ ���� �ְ� �κ��丮 ������ �ʾ��� ��
            {
                animator.SetTrigger(aniGranade);                                                       // ����ź �ִϸ��̼� Ʈ����
                StartCoroutine(ThrowGranade());                                                        // ����ź ������ �ڷ�ƾ ����
            }
        }
        else if (granadeData.Count == 0)                                                              // ����ź ������ 0�� ��
        {
            granadeMesh.enabled = false;                                                              // ����ź �޽� ��Ȱ��ȭ
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.granadeIdx].SetParent(itemEmptyGroup);  // ����ź �κ��丮 ����
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.granadeIdx].GetComponent<Image>().enabled = false;  // �̹��� ��Ȱ��ȭ
            GameManager.Instance.itemEmptyText[GameManager.Instance.granadeIdx].enabled = false;       // �ؽ�Ʈ ��Ȱ��ȭ
            GameManager.Instance.getGranade = false;                                                  // ����ź ���� ���� ������Ʈ
        }
    }

    IEnumerator ThrowGranade()
    {
        isThrow = true;                                                                                // ���� ���� ������Ʈ
        granadeData.Count--;                                                                           // ����ź ���� ����
        yield return new WaitForSeconds(1.8f);                                                         // ���
        SoundManager.soundInst.PlayeOneShot(granadeData.throwClip, source);                            // ����ź �Ҹ� ���
        bulletText.text = granadeData.Count.ToString();                                                // �Ѿ� �ؽ�Ʈ ������Ʈ
        GameObject _granade = ObjectPoolingManager.objPooling.GetWeaponGranade();                      // ����ź ��ü Ǯ���� ��������
        _granade.transform.position = granadePos.position;                                             // ����ź ��ġ ����
        _granade.transform.rotation = granadePos.rotation;                                             // ����ź ȸ�� ����
        _granade.SetActive(true);                                                                      // ����ź Ȱ��ȭ
        GameManager.Instance.itemEmptyText[GameManager.Instance.granadeIdx].text = gunData.Rf_Count.ToString();  // �ؽ�Ʈ ������Ʈ
        yield return new WaitForSeconds(2f);                                                           // ���
        isThrow = false;                                                                               // ���� ���� ������Ʈ
    }

    public void ChangeGranade()
    {
        isGranade = true;                                                                               // ����ź ��� ���� ������Ʈ
        isRifle = false;                                                                                // ������ ��� ���� ������Ʈ
        isShotGun = false;                                                                              // ���� ��� ���� ������Ʈ
        granadeMesh.enabled = true;                                                                     // ����ź �޽� Ȱ��ȭ
        animator.SetBool(aniIsGun, false);                                                              // �ѱ� �ִϸ��̼� ���� ������Ʈ
        bulletText.text = granadeData.Count.ToString();                                                 // �Ѿ� �ؽ�Ʈ ������Ʈ
        weaponImage.sprite = granadeData.ui_Sprite;                                                     // ���� �̹��� ������Ʈ
    }

    public void ChangeShotGun()
    {
        animator.SetBool(aniIsGun, true);                                                               // �ѱ� �ִϸ��̼� ���� ������Ʈ
        isRifle = false;                                                                                // ������ ��� ���� ������Ʈ
        isGranade = false;                                                                              // ����ź ��� ���� ������Ʈ
        isShotGun = true;                                                                               // ���� ��� ���� ������Ʈ
        rifleMesh.enabled = false;                                                                      // ������ �޽� ��Ȱ��ȭ
        shotgunMesh.enabled = true;                                                                     // ���� �޽� Ȱ��ȭ
        gunData.g_damage = 50;                                                                          // ���� ������ ����
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();          // �Ѿ� �ؽ�Ʈ ������Ʈ
        weaponImage.sprite = gunData.sg_UISprite;                                                       // ���� �̹��� ������Ʈ
    }

    public void ChangeRifle()
    {
        animator.SetBool(aniIsGun, true);                                                               // �ѱ� �ִϸ��̼� ���� ������Ʈ
        isShotGun = false;                                                                              // ���� ��� ���� ������Ʈ
        isRifle = true;                                                                                 // ������ ��� ���� ������Ʈ
        isGranade = false;                                                                              // ����ź ��� ���� ������Ʈ
        shotgunMesh.enabled = false;                                                                    // ���� �޽� ��Ȱ��ȭ
        rifleMesh.enabled = true;                                                                       // ������ �޽� Ȱ��ȭ
        gunData.g_damage = 15;                                                                          // ������ ������ ����
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();            // �Ѿ� �ؽ�Ʈ ������Ʈ
        weaponImage.sprite = gunData.rf_UISprite;                                                       // ���� �̹��� ������Ʈ
    }

    void ShootFire(ParticleSystem particle, string stopFunction, int bulletCount, AudioClip clip, int itemDatabaseCount)
    {
        GameObject _bullet = ObjectPoolingManager.objPooling.GetPlayerBullet();                         // �Ѿ� ��ü Ǯ���� ��������
        _bullet.transform.position = shotgunFirePos.position;                                           // �Ѿ� ��ġ ����
        _bullet.transform.rotation = shotgunFirePos.rotation;                                           // �Ѿ� ȸ�� ����
        _bullet.SetActive(true);                                                                        // �Ѿ� Ȱ��ȭ
        particle.Play();                                                                                // ��ƼŬ ���
        Invoke(stopFunction, 0.1f);                                                                     // ��ƼŬ ���� �Լ� ȣ��
        animator.SetTrigger(aniFire);                                                                   // �߻� �ִϸ��̼� Ʈ����
        bulletText.text = bulletCount.ToString() + " / " + itemDatabaseCount.ToString();                // �Ѿ� �ؽ�Ʈ ������Ʈ
        SoundManager.soundInst.PlayeOneShot(clip, source);                                              // �߻� �Ҹ� ���
    }

    IEnumerator RifleReload()
    {
        isReload = true;                                                                               // ������ ���� ������Ʈ
        animator.SetTrigger(aniReload);                                                                // ������ �ִϸ��̼� Ʈ����
        animator.SetBool(aniIsReload, true);                                                           // ������ �ִϸ��̼� ���� ������Ʈ
        yield return new WaitForSeconds(1.55f);                                                        // ���
        SoundManager.soundInst.PlayeOneShot(gunData.rifleReloadClip, source);                          // ������ �Ҹ� ���
        animator.SetBool(aniIsReload, false);                                                          // ������ �ִϸ��̼� ���� ������Ʈ
        isReload = false;                                                                              // ������ ���� ������Ʈ

        if (gunData.Rf_Count >= rifleBulletMaxCount)                                                  // ���� źȯ�� �ִ� źȯ �� �̻��� ��
        {
            if (rifleBulletCount == 0)                                                                // �Ѿ��� ���� ��
            {
                rifleBulletCount += rifleBulletMaxCount;                                              // �Ѿ� �� ������Ʈ
                gunData.Rf_Count -= rifleBulletCount;                                                 // ���� źȯ �� ������Ʈ
            }
            else if (rifleBulletCount > 0)                                                            // �Ѿ��� ���� ��
            {
                int cot = rifleBulletMaxCount;                                                        // �ִ� źȯ �� ���� ����
                cot -= rifleBulletCount;                                                              // ���� źȯ ���
                rifleBulletCount += cot;                                                              // �Ѿ� �� ������Ʈ
                gunData.Rf_Count -= cot;                                                              // ���� źȯ �� ������Ʈ
            }
        }
        else if (gunData.Rf_Count < rifleBulletMaxCount)                                              // ���� źȯ�� �ִ� źȯ �� ������ ��
        {
            if (rifleBulletCount == 0)                                                                // �Ѿ��� ���� ��
            {
                rifleBulletCount += gunData.Rf_Count;                                                 // �Ѿ� �� ������Ʈ
                gunData.Rf_Count -= rifleBulletCount;                                                 // ���� źȯ �� ������Ʈ
            }
            else if (rifleBulletCount > 0)                                                            // �Ѿ��� ���� ��
            {
                int let = rifleBulletMaxCount;                                                        // �ִ� źȯ �� ���� ����
                let -= rifleBulletCount;                                                              // ���� źȯ ���
                if (gunData.Rf_Count >= let)                                                          // ���� źȯ�� ���� źȯ �̻��� ��
                {
                    rifleBulletCount += let;                                                          // �Ѿ� �� ������Ʈ
                    gunData.Rf_Count -= let;                                                          // ���� źȯ �� ������Ʈ
                }
                else if (gunData.Rf_Count < let)                                                      // ���� źȯ�� ���� źȯ ������ ��
                {
                    rifleBulletCount += gunData.Rf_Count;                                             // �Ѿ� �� ������Ʈ
                    gunData.Rf_Count -= gunData.Rf_Count;                                             // ���� źȯ �� ������Ʈ
                }
            }
        }
        bulletText.text = rifleBulletCount.ToString() + " / " + gunData.Rf_Count.ToString();          // �Ѿ� �ؽ�Ʈ ������Ʈ
        GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].text = gunData.Rf_Count.ToString();  // ���� źȯ �ؽ�Ʈ ������Ʈ

        if (gunData.Rf_Count == 0)                                                                    // ���� źȯ�� ���� ��
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.rifleBulletIdx].SetParent(itemEmptyGroup);  // �κ��丮 ������Ʈ
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.rifleBulletIdx].GetComponent<Image>().enabled = false;  // �̹��� ��Ȱ��ȭ
            GameManager.Instance.itemEmptyText[GameManager.Instance.rifleBulletIdx].enabled = false;   // �ؽ�Ʈ ��Ȱ��ȭ
        }
    }

    IEnumerator ShotGunReload()
    {
        isReload = true;                                                                               // ������ ���� ������Ʈ
        animator.SetTrigger(aniReload);                                                                // ������ �ִϸ��̼� Ʈ����
        animator.SetBool(aniIsReload, true);                                                           // ������ �ִϸ��̼� ���� ������Ʈ
        yield return new WaitForSeconds(1.55f);                                                        // ���
        SoundManager.soundInst.PlayeOneShot(gunData.shotgunReloadClip, source);                        // ������ �Ҹ� ���
        animator.SetBool(aniIsReload, false);                                                          // ������ �ִϸ��̼� ���� ������Ʈ
        isReload = false;                                                                              // ������ ���� ������Ʈ

        if (gunData.Sg_Count >= shotgunBulletMaxCount)                                                 // ���� źȯ�� �ִ� źȯ �� �̻��� ��
        {
            if (shotgunBulletCount == 0)                                                               // �Ѿ��� ���� ��
            {
                shotgunBulletCount += shotgunBulletMaxCount;                                           // �Ѿ� �� ������Ʈ
                gunData.Sg_Count -= shotgunBulletCount;                                                // ���� źȯ �� ������Ʈ
            }
            else if (shotgunBulletCount > 0)                                                           // �Ѿ��� ���� ��
            {
                int shot = shotgunBulletMaxCount;                                                      // �ִ� źȯ �� ���� ����
                shot -= shotgunBulletCount;                                                            // ���� źȯ ���
                shotgunBulletCount += shot;                                                            // �Ѿ� �� ������Ʈ
                gunData.Sg_Count -= shot;                                                              // ���� źȯ �� ������Ʈ
            }
        }
        else if (gunData.Sg_Count < shotgunBulletMaxCount)                                             // ���� źȯ�� �ִ� źȯ �� ������ ��
        {
            if (shotgunBulletCount == 0)                                                               // �Ѿ��� ���� ��
            {
                shotgunBulletCount += gunData.Sg_Count;                                                // �Ѿ� �� ������Ʈ
                gunData.Sg_Count -= shotgunBulletCount;                                                // ���� źȯ �� ������Ʈ
            }
            else if (shotgunBulletCount > 0)                                                           // �Ѿ��� ���� ��
            {
                int gun = shotgunBulletMaxCount;                                                       // �ִ� źȯ �� ���� ����
                gun -= shotgunBulletCount;                                                             // ���� źȯ ���
                if (gunData.Sg_Count >= gun)                                                           // ���� źȯ�� ���� źȯ �̻��� ��
                {
                    shotgunBulletCount += gun;                                                         // �Ѿ� �� ������Ʈ
                    gunData.Sg_Count -= gun;                                                           // ���� źȯ �� ������Ʈ
                }
                else if (gunData.Sg_Count < gun)                                                       // ���� źȯ�� ���� źȯ ������ ��
                {
                    shotgunBulletCount += gunData.Sg_Count;                                            // �Ѿ� �� ������Ʈ
                    gunData.Sg_Count -= gunData.Sg_Count;                                              // ���� źȯ �� ������Ʈ
                }
            }
        }
        bulletText.text = shotgunBulletCount.ToString() + " / " + gunData.Sg_Count.ToString();          // �Ѿ� �ؽ�Ʈ ������Ʈ
        GameManager.Instance.itemEmptyText[GameManager.Instance.shotgunBulletIdx].text = gunData.Sg_Count.ToString();  // ���� źȯ �ؽ�Ʈ ������Ʈ

        if (gunData.Sg_Count == 0)                                                                     // ���� źȯ�� ���� ��
        {
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.shotgunBulletIdx].SetParent(itemEmptyGroup);  // �κ��丮 ������Ʈ
            GameManager.Instance.itemEmptyRectList[GameManager.Instance.shotgunBulletIdx].GetComponent<Image>().enabled = false;  // �̹��� ��Ȱ��ȭ
            GameManager.Instance.itemEmptyText[GameManager.Instance.shotgunBulletIdx].enabled = false;  // �ؽ�Ʈ ��Ȱ��ȭ
        }
    }

    void RifleFlashStop()
    {
        rifleFlash.Stop();                                                                              // ������ ��ƼŬ ����
    }

    void ShotGunFlashStop()
    {
        shotgunFlash.Stop();                                                                            // ���� ��ƼŬ ����
    }
}