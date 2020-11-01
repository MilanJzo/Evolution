using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //MouseRotation1();
        MouseRotation2();
    }


    void MouseRotation1()
    {
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
        transform.localRotation = Quaternion.Euler(new Vector4(-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z));
    }

    void MouseRotation2()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        Vector3 rotation = new Vector3(-1f * (mouseY * 180f), mouseX * 360f, 0);
        transform.localEulerAngles = transform.localRotation.eulerAngles + rotation * Time.deltaTime * rotationSpeed;
    }
}
