using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public float movementSpeed = 6f;
    public float jumpForce = 6f;
    public float maxVerticalLookAngle = 90f;
    public LayerMask groundLayer;
    public bool canMove;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Rigidbody rb;
    private CapsuleCollider capsule;
    private float originalHeight;
    private float originalMovementSpeed;
    private bool isCrouching = false;


    [SerializeField] private float crouchHeightMultiplier = 0.7f;
    [SerializeField] private float crouchMovementSpeedMultiplier = 0.2f;

    [SerializeField] private AudioClip footsteps;
    private AudioSource audioSource;

    private void Start()
    {
        canMove = true;
        //Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        capsule = GetComponent<CapsuleCollider>();
        originalHeight = capsule.height;
        originalMovementSpeed = movementSpeed;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (canMove)
        {
            CameraRotation();
            Movement();
            Jump();

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                
                if (isCrouching)
                {
                    Camera.main.transform.localPosition = new Vector3(0, 1, 0);
                    movementSpeed = originalMovementSpeed;

                }
                else
                {
                    Camera.main.transform.localPosition = new Vector3(0, 0f, 0);
                    movementSpeed = originalMovementSpeed * crouchMovementSpeedMultiplier;
                }
                isCrouching = !isCrouching;
                
            }
            else
            {
              
            }
        }

    }

    bool IsGrounded()
    {
        RaycastHit hit;
        float raycastDistance = capsule.height/2+0.2f;
        Vector3 raycastOrigin = capsule.transform.position + Vector3.down * (capsule.height * 0.5f) + new Vector3(0f, 0.05f, 0f);
        bool isGrounded = Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance, groundLayer);

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, Color.red);

        return isGrounded;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CameraRotation()
    {
        horizontalRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalLookAngle, maxVerticalLookAngle);

        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    void Movement()
    {

        Vector3 prevVelocity = rb.velocity;
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        if (IsGrounded()) {
            Vector3 movement;
            movement = (transform.right * xMovement + transform.forward * zMovement) * movementSpeed;
            movement.y = rb.velocity.y;
            rb.velocity = movement;

            if (rb.velocity == prevVelocity) { Debug.Log("character stops"); }
            if ((rb.velocity != Vector3.zero) /*&&*//* IsGrounded()*/)
            {
                if (!audioSource.isPlaying)
                    PlayMovementSound();
            }
            else
                StopMovementSound();
        }
        else
        {
            StopMovementSound();
        }
    }

    void PlayMovementSound()
    {
        audioSource.PlayOneShot(footsteps, audioSource.volume);
    }
    void StopMovementSound()
    {
        audioSource.Stop();
    }

    /*void Crouch()
    {
        capsule.height = capsuleHeightCrouching;
        movementSpeed = movementSpeed / 2;
        
    }

    void Stand()
    {
        capsule.height = capsuleHeightStanding;
        movementSpeed = movementSpeed * 2;
    }*/
}
