using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraDirection : MonoBehaviour
{
    private Transform thirdCameraTr;                        // 3��Ī ���� ī�޶��� Ʈ������
    private Transform cameraPivotTr;                        // 3��Ī ���� ī�޶��� �ǹ� Ʈ������
    private Camera thirdCamera;                             // 3��Ī ���� ī�޶�
    private Camera OnestCamera;                             // 1��Ī ���� ī�޶�
    [SerializeField][Range(0f, 20f)] private float cameraDistance = 2.5f;  // 3��Ī ���������� �÷��̾�� ī�޶� ���� �Ÿ�, �⺻�� 2.5
    private float cameraHeight = 1.5f;                      // ī�޶��� �⺻ ����, �⺻�� 1.5
    [SerializeField][Range(0f, 1000f)] private float mouseSensivity = 200f;  // ���콺 ����, �⺻�� 200
    private int playerLayer;                                // �÷��̾��� ���̾�
    private Vector3 mouseMove;                              // ���콺 ������
    private bool isView = true;                             // 1��Ī�� 3��Ī�� �����ϴ� �÷���, �⺻�� 3��Ī

    void OnEnable()
    {
        cameraPivotTr = GameObject.Find("CameraPivot").transform;             // "CameraPivot" ��ü�� Ʈ������ ��������
        thirdCameraTr = cameraPivotTr.GetChild(0).transform;                  // "CameraPivot"�� ù ��° �ڽ� ��ü Ʈ������ ��������
        thirdCamera = thirdCameraTr.GetComponent<Camera>();                   // 3��Ī ���� ī�޶� ������Ʈ ��������
        OnestCamera = transform.GetChild(1).GetComponent<Camera>();           // �÷��̾� ��ü�� �� ��° �ڽ� ��ü���� 1��Ī ���� ī�޶� ������Ʈ ��������
        OnestCamera.enabled = false;                                          // 1��Ī ���� ī�޶� ��Ȱ��ȭ
        playerLayer = LayerMask.NameToLayer("Player");                        // "Player" ���̾� ��������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))                                      // 'V' Ű�� ������ ��
        {
            isView = !isView;                                                 // isView �÷��׸� ����Ͽ� ���� ����
        }
    }

    void LateUpdate()
    {
        if (isView)
            ThirdPerson();                                                    // isView�� true�� �� 3��Ī ���� Ȱ��ȭ
        else
            OnestPerson();                                                    // isView�� false�� �� 1��Ī ���� Ȱ��ȭ
    }

    private void ThirdPerson()
    {
        OnestCamera.depth = -1f;                                              // 1��Ī ī�޶� ��Ȱ��ȭ
        thirdCamera.depth = 1f;                                               // 3��Ī ī�޶� Ȱ��ȭ
        thirdCamera.gameObject.SetActive(true);                               // 3��Ī ī�޶� Ȱ��ȭ
        OnestCamera.gameObject.SetActive(false);                              // 1��Ī ī�޶� ��Ȱ��ȭ

        cameraPivotTr.position = transform.position + Vector3.up * cameraHeight;  // ī�޶� �ǹ� ��ġ ����

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f); // ���콺 �Է����� ī�޶� ȸ�� �� ����

        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);                    // ���� ȸ�� ������ -40������ 40�� ���̷� ����
        cameraPivotTr.localEulerAngles = mouseMove;                           // ī�޶� �ǹ��� ���� ���Ϸ� ������ ȸ�� �� ����

        RaycastHit hit;                                                       // ����ĳ��Ʈ �浹 ���� ���� ����
        Vector3 dir = thirdCameraTr.position - cameraPivotTr.position;        // ī�޶� �ǹ����� 3��Ī ī�޶������ ���� ���� ���

        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDistance, ~(1 << playerLayer)))
            thirdCameraTr.localPosition = Vector3.back * hit.distance;        // �浹 �߻� �� ī�޶� ��ġ ����
        else
            thirdCameraTr.localPosition = Vector3.back * cameraDistance;      // �浹�� ������ ī�޶� �ִ� �Ÿ��� �̵�

        Quaternion caracterRot = cameraPivotTr.rotation;                      // ī�޶� �ǹ��� ȸ�� ���� ����
        caracterRot.x = caracterRot.z = 0f;                                   // ȸ�� ���� x�� z�� 0���� �����Ͽ� ���� ȸ���� ����
        transform.rotation = Quaternion.Slerp(transform.rotation, caracterRot, 10f * Time.deltaTime); // �÷��̾��� ȸ���� ī�޶� �ǹ��� ���� ȸ���� ���� �ε巴�� �����Ͽ� ����
    }

    private void OnestPerson()
    {
        OnestCamera.depth = 1f;                                               // 1��Ī ī�޶� Ȱ��ȭ
        thirdCamera.depth = -1f;                                              // 3��Ī ī�޶� ��Ȱ��ȭ
        OnestCamera.gameObject.SetActive(true);                               // 1��Ī ī�޶� Ȱ��ȭ
        thirdCamera.gameObject.SetActive(false);                              // 3��Ī ī�޶� ��Ȱ��ȭ

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f); // ���콺 �Է����� ī�޶� ȸ�� �� ����

        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);                    // ���� ȸ�� ������ -40������ 40�� ���̷� ����
        Vector3 Rot = mouseMove;                                              // ���콺 ȸ�� �� ����
        Rot.z = Rot.x = 0f;                                                   // ȸ�� ���� z�� x�� 0���� �����Ͽ� ���� ȸ���� ����
        transform.localEulerAngles = Rot;                                     // �÷��̾��� ���� ���Ϸ� ������ ȸ�� �� ����
    }
}