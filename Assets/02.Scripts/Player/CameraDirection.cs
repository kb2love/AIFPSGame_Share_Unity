using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraDirection : MonoBehaviour
{
    private Transform thirdCameraTr;                        // 3인칭 시점 카메라의 트랜스폼
    private Transform cameraPivotTr;                        // 3인칭 시점 카메라의 피벗 트랜스폼
    private Camera thirdCamera;                             // 3인칭 시점 카메라
    private Camera OnestCamera;                             // 1인칭 시점 카메라
    [SerializeField][Range(0f, 20f)] private float cameraDistance = 2.5f;  // 3인칭 시점에서의 플레이어와 카메라 사이 거리, 기본값 2.5
    private float cameraHeight = 1.5f;                      // 카메라의 기본 높이, 기본값 1.5
    [SerializeField][Range(0f, 1000f)] private float mouseSensivity = 200f;  // 마우스 감도, 기본값 200
    private int playerLayer;                                // 플레이어의 레이어
    private Vector3 mouseMove;                              // 마우스 움직임
    private bool isView = true;                             // 1인칭과 3인칭을 구분하는 플래그, 기본값 3인칭

    void OnEnable()
    {
        cameraPivotTr = GameObject.Find("CameraPivot").transform;             // "CameraPivot" 객체의 트랜스폼 가져오기
        thirdCameraTr = cameraPivotTr.GetChild(0).transform;                  // "CameraPivot"의 첫 번째 자식 객체 트랜스폼 가져오기
        thirdCamera = thirdCameraTr.GetComponent<Camera>();                   // 3인칭 시점 카메라 컴포넌트 가져오기
        OnestCamera = transform.GetChild(1).GetComponent<Camera>();           // 플레이어 객체의 두 번째 자식 객체에서 1인칭 시점 카메라 컴포넌트 가져오기
        OnestCamera.enabled = false;                                          // 1인칭 시점 카메라 비활성화
        playerLayer = LayerMask.NameToLayer("Player");                        // "Player" 레이어 가져오기
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))                                      // 'V' 키를 눌렀을 때
        {
            isView = !isView;                                                 // isView 플래그를 토글하여 시점 변경
        }
    }

    void LateUpdate()
    {
        if (isView)
            ThirdPerson();                                                    // isView가 true일 때 3인칭 시점 활성화
        else
            OnestPerson();                                                    // isView가 false일 때 1인칭 시점 활성화
    }

    private void ThirdPerson()
    {
        OnestCamera.depth = -1f;                                              // 1인칭 카메라 비활성화
        thirdCamera.depth = 1f;                                               // 3인칭 카메라 활성화
        thirdCamera.gameObject.SetActive(true);                               // 3인칭 카메라 활성화
        OnestCamera.gameObject.SetActive(false);                              // 1인칭 카메라 비활성화

        cameraPivotTr.position = transform.position + Vector3.up * cameraHeight;  // 카메라 피벗 위치 설정

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f); // 마우스 입력으로 카메라 회전 값 갱신

        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);                    // 수직 회전 각도를 -40도에서 40도 사이로 제한
        cameraPivotTr.localEulerAngles = mouseMove;                           // 카메라 피벗의 로컬 오일러 각도로 회전 값 적용

        RaycastHit hit;                                                       // 레이캐스트 충돌 정보 저장 변수
        Vector3 dir = thirdCameraTr.position - cameraPivotTr.position;        // 카메라 피벗에서 3인칭 카메라까지의 방향 벡터 계산

        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDistance, ~(1 << playerLayer)))
            thirdCameraTr.localPosition = Vector3.back * hit.distance;        // 충돌 발생 시 카메라 위치 조정
        else
            thirdCameraTr.localPosition = Vector3.back * cameraDistance;      // 충돌이 없으면 카메라를 최대 거리로 이동

        Quaternion caracterRot = cameraPivotTr.rotation;                      // 카메라 피벗의 회전 값을 저장
        caracterRot.x = caracterRot.z = 0f;                                   // 회전 값의 x와 z를 0으로 설정하여 수평 회전만 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, caracterRot, 10f * Time.deltaTime); // 플레이어의 회전을 카메라 피벗의 수평 회전에 맞춰 부드럽게 보간하여 적용
    }

    private void OnestPerson()
    {
        OnestCamera.depth = 1f;                                               // 1인칭 카메라 활성화
        thirdCamera.depth = -1f;                                              // 3인칭 카메라 비활성화
        OnestCamera.gameObject.SetActive(true);                               // 1인칭 카메라 활성화
        thirdCamera.gameObject.SetActive(false);                              // 3인칭 카메라 비활성화

        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f); // 마우스 입력으로 카메라 회전 값 갱신

        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);                    // 수직 회전 각도를 -40도에서 40도 사이로 제한
        Vector3 Rot = mouseMove;                                              // 마우스 회전 값 저장
        Rot.z = Rot.x = 0f;                                                   // 회전 값의 z와 x를 0으로 설정하여 수평 회전만 적용
        transform.localEulerAngles = Rot;                                     // 플레이어의 로컬 오일러 각도로 회전 값 적용
    }
}