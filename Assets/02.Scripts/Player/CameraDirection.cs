using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraDirection : MonoBehaviour
{
    private Transform thirdCameraTr;                        //3��Ī ������ ī�޶��� ������
    private Transform cameraPivotTr;                        //3��Ī ������ ī�޶��� �θ� ������
    private Animator animator;                              //IK����� ���� �ִϸ�����
    private Camera thirdCamera;                             //3��Ī ������ ī�޶�
    private Camera OnestCamera;                             //1��Ī ������ ī�޶�
    private Transform leftHand;                             //IK����� ���� �޼�
    private Transform rightHand;                            //IK����� ���� ������
    [SerializeField] [Range(0f, 20f)] float cameraDistance; //3��Ī ���������� �÷��̾�� ī�޶� ������ �Ÿ�
    private float cameraHeight;                             //ī�޶��� �⺻����
    [Range(0f, 1000f)] private float mouseSensivity;        //���콺 ����
    private int playerLayer;                                //�÷��̾��� ���̾�
    private Vector3 mouseMove;                              //���콺 ������
    private bool isView;                                    //1��Ī�� 3��Ī�� ����
    private float weight = 1.0f;
    private float ikRotWeight = 1.0f;
    private float armAngleLimit = 60f;
    private int aniLayerIdx;

    void OnEnable()
    {
        cameraPivotTr = GameObject.Find("CameraPivot").transform;
        thirdCameraTr = cameraPivotTr.GetChild(0).transform;
        thirdCamera = thirdCameraTr.GetComponent<Camera>();
        OnestCamera = transform.GetChild(1).GetComponent<Camera>();
        OnestCamera.enabled = false;
        mouseSensivity = 200f;
        cameraDistance = 2.5f;
        cameraHeight = 1.5f;
        isView = true;
        animator = GetComponentInChildren<Animator>();
        leftHand = GameObject.Find("Lthumb").transform;
        rightHand = GameObject.Find("Rthumb").transform;
        playerLayer = LayerMask.NameToLayer("Player");
        aniLayerIdx = animator.GetLayerIndex("Shot&Reload Layer");
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            isView = !isView;
        }
    }
    private void LateUpdate()
    {
        if (isView)
            ThirdPerson();
        else if (!isView)
            OnestPerson();
    }

    private void ThirdPerson()
    {
        OnestCamera.depth = -1f;
        thirdCamera.depth = 1f;
        thirdCamera.gameObject.SetActive(true);
        OnestCamera.gameObject.SetActive(false);
        cameraPivotTr.position = transform.position + Vector3.up * cameraHeight;
        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f);
        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);
        cameraPivotTr.localEulerAngles = mouseMove;
        RaycastHit hit;
        Vector3 dir = thirdCameraTr.position - cameraPivotTr.position;
        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDistance, ~(1 << playerLayer)))
            thirdCameraTr.localPosition = Vector3.back * hit.distance;
        else
            thirdCameraTr.localPosition = Vector3.back * cameraDistance;
        Quaternion caracterRot = cameraPivotTr.rotation;
        caracterRot.x = caracterRot.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, caracterRot, 10f * Time.deltaTime);
    }
    private void OnAnimatorIK(int layerIdx)
    {
        if (layerIdx != aniLayerIdx) return;
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.transform.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.transform.rotation);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.transform.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.transform.rotation);
        leftHand.localEulerAngles = mouseMove;  
        rightHand.localEulerAngles = mouseMove;
    }
    private void OnestPerson()
    {
        OnestCamera.depth = 1f;
        thirdCamera.depth = -1f;
        OnestCamera.gameObject.SetActive(true);
        OnestCamera.enabled = true;
        thirdCamera.gameObject.SetActive(false);
        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f);
        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);
        Vector3 Rot = mouseMove;
        transform.localEulerAngles = Rot;
    }
}
