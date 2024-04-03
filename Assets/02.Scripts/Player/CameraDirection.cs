using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirection : MonoBehaviour
{
    private Transform thirdCameraTr;
    private Transform cameraPivotTr;

    private Camera thirdCamera;
    private Camera OnestCamera;
    private Transform OneCaTr;
    [SerializeField]
    [Range(0f, 20f)] float cameraDistance;
    private float cameraHeight;
    [Range(0f, 1000f)] private float mouseSensivity;
    private int playerLayer;
    private Vector3 mouseMove;
    private bool isPerson;

    void OnEnable()
    {
        cameraPivotTr = GameObject.Find("CameraPivot").transform;
        thirdCameraTr = cameraPivotTr.GetChild(0).transform;
        thirdCamera = thirdCameraTr.GetComponent<Camera>();
        OnestCamera = transform.GetChild(1).GetComponent<Camera>();
        OneCaTr = OnestCamera.transform;
        OnestCamera.enabled = false;
        mouseSensivity = 200f;
        cameraDistance = 2.5f;
        cameraHeight = 1.5f;
        isPerson = true;
        playerLayer = LayerMask.NameToLayer("Player");
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            isPerson = !isPerson;
        }
    }
    private void LateUpdate()
    {
        if (isPerson)
            ThirdPerson();
        else if (!isPerson)
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
        Rot.x = Rot.z = 0f;
        transform.localEulerAngles = Rot;
    }
}
