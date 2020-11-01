using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float rotationSpeed = 200;
    [SerializeField]
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Rotate(rotationSpeed);

        gameObject.transform.Translate(PlayerInputMovement() * speed * Time.deltaTime);

        //gameObject.transform.Rotate(new Vector3(0, PlayerInputKeyboard().y,0) *rotationSpeed*Time.deltaTime);
        //camera.transform.RotateAround(gameObject.transform.position,  gameObject.transform.right, PlayerInputKeyboard().x  *  Time.deltaTime  *  rotationSpeed);
    }


    Vector3 PlayerInputMovement()
    {
        Vector3 input = new Vector3();
        


        if (Input.GetKey(KeyCode.D)) input += camera.transform.right;
        else if (Input.GetKey(KeyCode.A)) input -= camera.transform.right;

        if (Input.GetKey(KeyCode.S)) input -= camera.transform.forward;
        else if (Input.GetKey(KeyCode.W)) input += camera.transform.forward;

        input.y = 0;
        input = input.normalized;

        if (Input.GetKey(KeyCode.LeftShift)) input = new Vector3(input.x,-1, input.z);
        else if (Input.GetKey(KeyCode.Space)) input = new Vector3(input.x, 1, input.z);

        return input;
    }

    /*Vector2 PlayerInputKeyboard()
    {
        Vector2 input = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow)) input = new Vector2(-1, 0);
        else if (Input.GetKey(KeyCode.DownArrow)) input = new Vector2(1, 0);

        if (Input.GetKey(KeyCode.LeftArrow)) input = new Vector2(0, -1);
        else if (Input.GetKey(KeyCode.RightArrow)) input = new Vector2(0, 1);

        return input;
    }*/
}