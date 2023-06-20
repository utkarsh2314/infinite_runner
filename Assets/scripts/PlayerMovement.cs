using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public  float LANE_LENGTH = 2f;
    public  float TURN = 0.35f;

    private Animator animator;
    private CharacterController characterController;
    public float jumpForce = 8.0f;
    public float gravity = 12.0f;
    private float verticalVelocity;
    public float speed = 7.0f;
    public float maxSpeed;
    private int desiredLane = 1;

    [SerializeField] private AudioSource coincollect;
    [SerializeField] private AudioSource obstacleaudio;
    [SerializeField] private AudioSource jumpaudio;

    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;
    private bool swipeUp = false;
    private bool swipeDown = false;
    public static bool tap = false;


    public bool detectSwipeAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        swipeDown = false;
        swipeUp = false;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                tap = true;
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }

            //Detects Swipe while finger is still moving on screen
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeAfterRelease)
                {
                    fingerDownPos = touch.position;
                    DetectSwipe();
                }
            }

            //Detects swipe after finger is released from screen
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                DetectSwipe();
            }
        }

        if (!PlayerManager.isGameStarted)
            return;
        if(speed < maxSpeed)
        {
            speed += 0.1f * Time.deltaTime;
        }
        //Changes
        //Our future location
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * 2* LANE_LENGTH;
        }
        else if (desiredLane == 1)
        {
            //animator.SetBool("isRight", true);
            targetPosition += Vector3.left * LANE_LENGTH;
        }

        //Move Delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        //Y axis
       // Debug.Log(IsGrounded());
        if (IsGrounded())
        {
            verticalVelocity = -0.1f;
            if (swipeUp)
            {
                jumpaudio.Play();
                animator.SetTrigger("jump");
                verticalVelocity = jumpForce;
            }
            if (swipeDown)
            {
                Debug.Log("roll");
                animator.SetTrigger("roll");
            }
        }

        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);
            if (swipeDown)
            {
                verticalVelocity = -2 * jumpForce;
            }
        }



        moveVector.y = verticalVelocity;
        moveVector.z = speed;



        //Move Char
        characterController.Move(moveVector * Time.deltaTime);

        Vector3 direction = characterController.velocity;
        direction.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, direction, TURN);

    }

    void DetectSwipe()
    {

        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
        {
            //Debug.Log("Vertical Swipe Detected!");
            if (fingerDownPos.y - fingerUpPos.y > 0)
            {
                swipeUp = true;
            }
            else if (fingerDownPos.y - fingerUpPos.y < 0)
            {
                swipeDown = true;
            }
            fingerUpPos = fingerDownPos;

        }
        else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
        {
            //Debug.Log("Horizontal Swipe Detected!");
            if (fingerDownPos.x - fingerUpPos.x > 0)
            {
                MoveLane(true);
            }
            else if (fingerDownPos.x - fingerUpPos.x < 0)
            {
                MoveLane(false);
            }
            fingerUpPos = fingerDownPos;

        }
        else
        {
            //Debug.Log("No Swipe Detected!");
        }
    }

    float VerticalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    float HorizontalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    void OnSwipeUp()
    {
        //Do something when swiped up
    }

    void OnSwipeDown()
    {
        //Do something when swiped down
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(characterController.bounds.center.x, (characterController.bounds.center.y - characterController.bounds.extents.y) + 0.2f, characterController.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "obstacle")
        {
            obstacleaudio.Play();
            animator.SetTrigger("collide");
            PlayerManager.gameOver = true;
        }
        if (hit.transform.tag == "coin")
        {
            coincollect.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "coin")
        {
            coincollect.Play();
        }
    }

}