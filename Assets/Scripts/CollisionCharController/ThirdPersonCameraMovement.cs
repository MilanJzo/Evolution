using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
    TODO:
        -fix camera clipping
*/
public class ThirdPersonCameraMovement : MonoBehaviour
{
    [SerializeField]
    bool lockCursor, smoothing;

    [SerializeField]
    float   mouseSensitivity = 10, 
            distanceFromTarget = 2, 
            rotationSmoothTime = 0.12f;

    [SerializeField]
    Vector2 pitchMinMax = new Vector2(-30, 90);

    [SerializeField]
    Transform target;

    Vector3 rotationSmoothVelocity, currenRotation;
    float yaw, pitch;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        yaw += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currenRotation = smoothing ? Vector3.SmoothDamp(currenRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime): new Vector3(pitch, yaw); 

        transform.eulerAngles = currenRotation;

        Vector3 angles = transform.eulerAngles;
        angles.x = 0;

        target.eulerAngles = angles;
        transform.position = target.position - transform.forward * distanceFromTarget;

    }
}
