using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;


    private CharacterController controller;
    private Transform mainCamera;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    private void Update()
    {
        HandleMovement();

        HandleJump();
        ApplyGravity();
    }

    private void LateUpdate()
    {

    }

    private void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Calculate movement direction relative to camera
        Vector3 cameraForward = mainCamera.forward;
        Vector3 cameraRight = mainCamera.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Determine speed
        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Move the character
        if (moveDirection.magnitude >= 0.1f)
        {
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            // Rotate character to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


    private void HandleJump()
    {
        if (Input.GetAxis("Jump") !=0 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}