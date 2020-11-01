using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cubeRotation2();
    }

    void cubeRotation1()
    {
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;

        transform.localRotation = Quaternion.Euler(new Vector4(transform.localRotation.y, mouseX * 360f, transform.localRotation.z));
    }

    void cubeRotation2()
    {
        gameObject.transform.forward = new Vector3(Camera.main.transform.forward.x, 0,Camera.main.transform.forward.z);

    }
}
