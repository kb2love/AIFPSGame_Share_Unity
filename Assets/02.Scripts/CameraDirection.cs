using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirection : MonoBehaviour
{
    private Transform cameraTr;
    private Transform cameraPivotTr;
    private Transform modelTr;
    
    private Camera OnestCamera;
    [SerializeField]
    [Range(0f, 20f)] float cameraDistance;
    private float cameraHeight;
    [Range(0f, 1000f)] private float mouseSensivity;
    private int playerLayer;
    private Vector3 mouseMove;

    void Start()
    {
        cameraTr = Camera.main.transform;
        cameraPivotTr = cameraTr.parent.transform;
        modelTr = transform.GetChild(0).transform;
        OnestCamera = transform.GetChild(1).GetComponent<Camera>();
        OnestCamera.enabled = false;
        mouseSensivity = 200f;
        cameraDistance = 2.5f;
        cameraHeight = 1.5f;
        playerLayer = LayerMask.NameToLayer("Player");
    }
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        cameraPivotTr.position = transform.position + Vector3.up * cameraHeight;
        mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * mouseSensivity * Time.deltaTime,
                                  Input.GetAxisRaw("Mouse X") * mouseSensivity * Time.deltaTime, 0f);
        mouseMove.x = Mathf.Clamp(mouseMove.x, -40f, 40f);
        cameraPivotTr.localEulerAngles = mouseMove;
        RaycastHit hit;
        Vector3 dir = cameraTr.position - cameraPivotTr.position;
        if (Physics.Raycast(cameraPivotTr.position, dir, out hit, cameraDistance, ~(1 << playerLayer)))
            cameraTr.localPosition = Vector3.back * hit.distance;
        else
            cameraTr.localPosition = Vector3.back * cameraDistance;
        Quaternion caracterRot = cameraPivotTr.rotation;
        caracterRot.x = caracterRot.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, caracterRot, 10f * Time.deltaTime);
    }
}
