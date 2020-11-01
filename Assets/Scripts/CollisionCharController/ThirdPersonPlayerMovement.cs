using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Player Options")]
    [SerializeField]
    float playerHeight;

    [Header("Movement Options")]
    [SerializeField]
    bool smooth;
    [SerializeField]
    float movementSpeed, maxSlope, smoothSpeed;

    [Header("Jump Options")]
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float jumpSpeed, jumpDecrease;

    [Header("Gravity")]
    [SerializeField]
    float gravity = 2.5f;

    [Header("GroundCheck")]
    [SerializeField]
    Vector3 liftPoint = new Vector3(0, 1.2f, 0);
    [SerializeField]
    float sphereRadius = 0.17f, maxCheckDistance = 20, groundCheckRadius = 0.57f, snapDistance = 3.1f;
    [SerializeField]
    LayerMask checkLayers;

    [Header("References")]
    [SerializeField]
    SphereCollider sphereCol;


    //Movement Private Variables
    Vector3 velocity;
    Vector3 move;
    Vector3 vel;

    //Gravity Private Variables
    bool grounded;
    float currentGravity;

    //Ground Check Private Variables
    RaycastHit groundHit;
    Vector3 groundClamp, groundCheckPoint = new Vector3(0,-0.87f,0);

    //Jumping Private Variables
    float jumpHeight = 0;
    bool inputJump;
    #endregion

    #region Main Method
    void FixedUpdate()
    {
        Gravity();
        SimpleMove();
        Jump();
        FinalMove();
        GroundCheck();
        CollsionCheck();
    }
    #endregion

    #region Movement
    void SimpleMove()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        velocity += move;
    }
    void FinalMove()
    {
        vel = new Vector3(velocity.x, velocity.y, velocity.z);
        vel = transform.TransformDirection(vel);

        transform.position += vel.normalized * Time.deltaTime * movementSpeed;

        velocity = Vector3.zero;
    }
    #endregion

    #region Gravity/Grounding
    void Gravity()
    {
        if (!grounded) velocity.y -= gravity;
    }
   

    void GroundCheck()
    {
        Ray ray = new Ray(transform.TransformPoint(liftPoint), Vector3.down);
        RaycastHit tempHit = new RaycastHit();
        if (Physics.SphereCast(ray, sphereRadius, out tempHit, maxCheckDistance, checkLayers))
        {
            GroundConfirm(tempHit);
        }
        else grounded = false;
    }

    private void GroundConfirm (RaycastHit tempHit)
    {
        //float currentSlope = Vector3.Angle(tempHit.normal, Vector3.up);

        Collider[] col = new Collider[3];
        int num = Physics.OverlapSphereNonAlloc(transform.TransformPoint(groundCheckPoint), groundCheckRadius, col, checkLayers);

        grounded = false;

        for(int i = 0; i < num; i++)
        {
            if (col[i].transform == tempHit.transform)
            {
                groundHit = tempHit;
                grounded = true;
                if (!inputJump)
                {
                    if (!smooth)
                    {
                        transform.position = new Vector3(transform.position.x, groundHit.point.y + playerHeight / 2, transform.position.z);
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, groundHit.point.y + playerHeight / 2, transform.position.z), smoothSpeed * Time.deltaTime);
                    }
                }

                break;
            }
        }


        if(num <= 1 && tempHit.distance <= snapDistance && !inputJump)
        {
            if(col[0] != null)
            {
                Ray ray = new Ray(transform.TransformPoint(liftPoint), Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, snapDistance, checkLayers))
                {
                    if(hit.transform != col[0].transform)
                    {
                        grounded = false;
                        return;
                    }
                }
            }
        }
    }
    #endregion

    #region Collision
    private void CollsionCheck()
    {
        Collider[] overlaps = new Collider[4];
        int num = Physics.OverlapSphereNonAlloc(transform.TransformPoint(sphereCol.center),sphereCol.radius,overlaps,checkLayers,QueryTriggerInteraction.UseGlobal);

        for(int i = 0; i < num; i++)
        {
            Transform t = overlaps[i].transform;
            Vector3 dir;
            float dist;

            if (Physics.ComputePenetration(sphereCol, transform.position, transform.rotation, overlaps[i], t.position, t.rotation, out dir, out dist))
            {
                Vector3 penetrationVector = dir * dist;
                Vector3 velocityProjected = Vector3.Project(velocity, -dir);
                transform.position = transform.position + penetrationVector;
                vel -= velocityProjected;

            }

        }
    }

    #endregion

    #region Jumping

    void Jump()
    {
        bool canJump = false;

        canJump = !Physics.Raycast(new Ray(transform.position, Vector3.up), playerHeight, checkLayers);

        if(grounded)
        {
            jumpHeight = 0;
            inputJump = false;
        }

        if (grounded && canJump)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                inputJump = true;
                transform.position += Vector3.up * (groundCheckRadius + 0.2f) * 2;
                jumpHeight += jumpForce;

            }
        }
        else
        {
            if(!grounded)
            {
                jumpHeight -=  gravity * Time.deltaTime;
            }
        }

        velocity.y += jumpHeight;

    }

    #endregion
}
