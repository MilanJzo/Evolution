using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    TODO: 
        -milans input delay
        -fine tuning vars
        -neues ground checking
        -vllt "Kletterfeature" fixen


*/
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
    float movementSpeed, smoothSpeed;

    [Header("Jump Options")]
    [SerializeField]
    float jumpForce = 3f;
    
    [Header("Gravity")]
    [SerializeField]
    float gravity = 0.5f;

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

    //Gravity Private Variables
    bool grounded;

    //Ground Check Private Variables
    RaycastHit groundHit;
    Vector3 groundCheckPoint = new Vector3(0,-0.87f,0);

    //Jumping Private Variables     
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
        velocity = new Vector3(0, velocity.y, 0);
        velocity += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }

    void FinalMove()
    {
        transform.position += transform.TransformDirection(velocity) * Time.deltaTime * movementSpeed;
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

        if (grounded) velocity.y = 0;
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
                    transform.position = new Vector3(transform.position.x, groundHit.point.y + playerHeight / 2, transform.position.z); //d:TODO HÄSSLICH
                }
                break;
            }
        }


        if(num <= 1 && tempHit.distance <= snapDistance && !inputJump && col[0] != null)
        {
            Ray ray = new Ray(transform.TransformPoint(liftPoint), Vector3.down);
            RaycastHit hit;

            grounded = !(Physics.Raycast(ray, out hit, snapDistance, checkLayers) && hit.transform != col[0].transform);
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
                transform.position = transform.position + dir * dist;
                velocity -= Vector3.Project(velocity, -dir);
            }
        }
    }

    #endregion

    #region Jumping

    void Jump()
    {
        bool canJump = false;

        if(grounded)
        {
            inputJump = false;
        }

        if(grounded && !Physics.Raycast(new Ray(transform.position, Vector3.up), playerHeight, checkLayers) && Input.GetKey(KeyCode.Space))
        {
            inputJump = true;
            transform.position += Vector3.up * (groundCheckRadius + 0.2f) * 2; //NEIN TODO
            velocity.y += jumpForce;
        }                    
    }
    #endregion
}
