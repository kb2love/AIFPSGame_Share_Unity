using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraDirection : MonoBehaviour
{
    private Transform thirdCameraTr;                        //3인칭 시점의 카메라의 포지션
    private Transform cameraPivotTr;                        //3인칭 시점의 카메라의 부모 포지션
    private Animator animator;                              //IK방식을 위한 애니메이터
    private Camera thirdCamera;                             //3인칭 시점의 카메라
    private Camera OnestCamera;                             //1인칭 시점의 카메라
    private Transform leftHand;                             //IK방식을 위한 왼손
    private Transform rightHand;                            //IK방식을 위한 오른손
    [SerializeField] [Range(0f, 20f)] float cameraDistance; //3인칭 시점에서의 플레이어와 카메라 사이의 거리
    private float cameraHeight;                             //카메라의 기본높이
    [Range(0f, 1000f)] private float mouseSensivity;        //마우스 감도
    private int playerLayer;                                //플레이어의 레이어
    private Vector3 mouseMove;                              //마우스 움직임
    private bool isView;                                    //1인칭과 3인칭을 구분
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
