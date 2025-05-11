using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // degrees per second
    public Transform cameraTransform;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        if(Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1, 0.5f,1);
            moveSpeed = 2.5f;
        }
        else
        {
            transform.localScale = new Vector3(1, 1,1);
            moveSpeed = 5;
        }
        if (inputDirection.magnitude == 1)
        {
            // Rotate player to match camera + movement direction
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + transform.eulerAngles.y;

            // Move in the direction the player is facing
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
            Vector3 velocity = moveDirection.normalized * moveSpeed;
            velocity.y = Physics.gravity.y; // Apply gravity
            controller.Move(velocity * Time.deltaTime);
        }
        if (inputDirection.magnitude == 0)
        {
            controller.Move(new Vector3(0,0,0));
            Vector3 velocity = Vector3.zero;
            velocity.y = Physics.gravity.y; // Apply gravity
            controller.Move(velocity * Time.deltaTime);
        }
        //Debug.Log(inputDirection.magnitude);

    }
}
