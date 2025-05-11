using UnityEngine;


public class CameraControl : MonoBehaviour
{
    [Header("Mouse Look")]
    public Transform cameraPivot; // Empty GameObject to rotate for camera look
    public float mouseSensitivity = 2f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    private float verticalCameraRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        // Horizontal (Yaw) rotation (player rotates left/right)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);

        // Vertical (Pitch) rotation (camera looks up/down)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalCameraRotation -= mouseY;
        verticalCameraRotation = Mathf.Clamp(verticalCameraRotation, minVerticalAngle, maxVerticalAngle);
        cameraPivot.localRotation = Quaternion.Euler(verticalCameraRotation, 0, 0);
    }
}