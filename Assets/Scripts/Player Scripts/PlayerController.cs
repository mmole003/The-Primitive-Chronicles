using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves forward/backward and rotates with WASD/Arrow keys and mouse look.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("Forward/back speed (units/sec).")]
    public float speed = 5.0f;

    [Tooltip("Turn speed (degrees/sec) from keyboard input.")]
    public float rotationSpeed = 120.0f;

    [Tooltip("Mouse sensitivity (degrees per pixel).")]
    public float mouseSensitivity = 0.15f;

    [Tooltip("Minimum camera pitch (degrees).")]
    public float pitchMin = -80f;

    [Tooltip("Maximum camera pitch (degrees).")]
    public float pitchMax = 80f;

    [Tooltip("Invert vertical look.")]
    public bool invertY = false;

    public float jumpForce = 5.0f;
    public float groundCheckDistance = 0.6f; // adjust to fit your collider/scale

    // Assign the camera (or camera parent pivot) here in inspector.
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool jumpRequested = false;

    // Internal rotation state (degrees)
    private float pitch = 0f;
    private float yaw = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogWarning("PlayerController needs a Rigidbody.");
        else
        {
            // Prevent tipping while allowing yaw rotation
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.angularDamping = Mathf.Max(rb.angularDamping, 2f);
        }

        // initialize yaw from current transform
        yaw = transform.eulerAngles.y;

        // initialize pitch from camera local rotation if available
        if (cameraTransform != null)
            pitch = cameraTransform.localEulerAngles.x > 180f ? cameraTransform.localEulerAngles.x - 360f : cameraTransform.localEulerAngles.x;

        // lock cursor for FPS-like control (optional)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Mouse look (read in Update for smoothness)
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float yDelta = mouseDelta.y * mouseSensitivity;
            float xDelta = mouseDelta.x * mouseSensitivity;

            yaw += xDelta;
            pitch += (invertY ? yDelta : -yDelta);

            // Clamp pitch so camera cannot loop/flip
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        }

        // Request jump from Update (input) so physics is handled in FixedUpdate
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (IsGrounded())
                jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = Vector2.zero;

        // Forward/backward
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y = -1f;

        // Left/right (keyboard rotation)
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1f;

        // Apply jump (physics) if requested
        if (jumpRequested && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpRequested = false;
        }

        // Move in facing direction 
        Vector3 movement = transform.forward * moveInput.y * speed * Time.fixedDeltaTime;
        if (rb != null) rb.MovePosition(rb.position + movement);

        // Keyboard turn (additive to mouse yaw)
        float turnDirection = moveInput.x;
        if (moveInput.y < 0)
            turnDirection = -turnDirection;

        float keyboardTurnDegrees = turnDirection * rotationSpeed * Time.fixedDeltaTime;
        yaw += keyboardTurnDegrees;

        // Apply yaw to Rigidbody rotation (keeps physics)
        if (rb != null)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
            rb.MoveRotation(targetRotation);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }
    }

    private void LateUpdate()
    {
        // Apply pitch to camera in LateUpdate so it uses final player rotation this frame
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    private bool IsGrounded()
    {
        // Simple downward raycast to check if player is near the ground.
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}