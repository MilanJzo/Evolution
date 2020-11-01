using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.001f;
    [SerializeField]
    private Vector3 rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(rotationSpeed);

        gameObject.transform.position +=new Vector3(PlayerInputMovement().x, PlayerInputMovement().z, PlayerInputMovement().y) *speed*Time.deltaTime;
        gameObject.transform.Rotate(new Vector3(PlayerInputKeyboard().x, 0,  PlayerInputKeyboard().y));
    }


    Vector3 PlayerInputMovement() 
    {
        Vector3 input = new Vector3();

        if (Input.GetKey(KeyCode.D)) input = new Vector3(1, input.y, input.z);
            else if (Input.GetKey(KeyCode.A)) input = new Vector3(-1, input.y, input.z);

        if (Input.GetKey(KeyCode.S)) input = new Vector3(input.x,-1, input.z);
            else if (Input.GetKey(KeyCode.W)) input = new Vector3(input.x, 1, input.z);

        if (Input.GetKey(KeyCode.LeftShift)) input = new Vector3(input.x, input.y, -1);
            else if (Input.GetKey(KeyCode.Space)) input = new Vector3(input.x, input.y, 1);



        return input;
    }

    Vector2 PlayerInputKeyboard()
    {
        Vector2 input = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow)) input = new Vector2(1, 0);
            else if (Input.GetKey(KeyCode.DownArrow)) input = new Vector2(-1, 0);

        if (Input.GetKey(KeyCode.LeftArrow)) input = new Vector2(0, 1);
            else if (Input.GetKey(KeyCode.RightArrow)) input = new Vector2(0, -1);

        return input;
    }
}
