using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement1 : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 move;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 3f;//The distance between tow lanes

    public bool isGrounded;
    private const float TURN = 0.35f;

    public float gravity = 12f;
    public float jumpForce = 8f;
    private float verticalVelocity;

    public Animator animator;


    bool toggle = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1.2f;
    }

    private void FixedUpdate()
    {
        //if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
        //   return;

        //Increase Speed
        if (toggle)
        {
            toggle = false;
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.fixedDeltaTime;
        }
        else
        {
            toggle = true;
            if (Time.timeScale < 2f)
                Time.timeScale += 0.005f * Time.fixedDeltaTime;
        }
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    void Update()
    {


        //if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
        //   return;

        //animator.SetBool("isGameStarted", true);
        move.z = forwardSpeed;



        //Gather the inputs on which lane we should be
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
            SwipeManager.swipeRight = false;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
            SwipeManager.swipeLeft = false;
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * 2 * laneDistance;
        }
        else if (desiredLane == 1)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        //transform.position = targetPosition;

        move.x = (targetPosition - transform.position).normalized.x * forwardSpeed;



        isGrounded = IsGrounded();
        //animator.SetBool("isGrounded", isGrounded);
        Debug.Log(isGrounded);

        bool fastDown = false;


        if (IsGrounded())
        {
            verticalVelocity = -0.1f;
            if (SwipeManager.swipeUp)
            {
                verticalVelocity = jumpForce;
                SwipeManager.swipeUp = false;
            }
            else if (SwipeManager.swipeDown)
            {
                //animator.SetTrigger("roll");
                SwipeManager.swipeDown = false;
            }

        }

        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);
            if (SwipeManager.swipeDown)
            {
                verticalVelocity = -jumpForce;
                fastDown = true;
                SwipeManager.swipeDown = false;
            }
        }

        if (fastDown)
        {
            //animator.SetTrigger("roll");
        }



        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);


    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            //PlayerManager.gameOver = true;
            //FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

}