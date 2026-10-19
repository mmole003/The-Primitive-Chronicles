
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walking,
        Attacking,
        Dead
    }

    [Header("Movement")]
    public float speed = 5.0f;
    public float rotationSpeed = 120.0f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.15f;
    public Transform cameraTransform;
    public float fixedCameraPitch = 45f;

    [Header("State Machine References")]
    public SwordAttack swordAttack;
    public PlayerHealth playerHealth;

    public PlayerState State { get; private set; } = PlayerState.Idle;

    private Rigidbody rb;
    private float yaw = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.angularDamping = Mathf.Max(rb.angularDamping, 2f);

        // Find references automatically if they are not assigned.
        if (swordAttack == null)
            swordAttack = GetComponentInChildren<SwordAttack>();

        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
            playerHealth = GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null)
            Debug.LogWarning("PlayerController: PlayerHealth was not found.");

        if (swordAttack == null)
            Debug.LogWarning("PlayerController: SwordAttack was not found.");

        yaw = transform.eulerAngles.y;

        // Set the camera to its fixed downward angle.
        if (cameraTransform != null)
        {
            cameraTransform.localRotation =
                Quaternion.Euler(fixedCameraPitch, 0f, 0f);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Death always takes priority over every other state.
        if (playerHealth != null && playerHealth.IsDead)
        {
            SwitchTo(PlayerState.Dead);
            return;
        }

        if (State == PlayerState.Dead)
            return;

        // Mouse movement rotates the player horizontally only.
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float xDelta = mouseDelta.x * mouseSensitivity;
            yaw += xDelta;
        }

        UpdateState();
    }

    private void UpdateState()
    {
        // An active sword swing takes priority over Idle and Walking.
        if (swordAttack != null && swordAttack.IsAttacking)
        {
            SwitchTo(PlayerState.Attacking);
            return;
        }

        // Only forward/backward input counts as walking.
        // A/D can still rotate the player while Idle.
        if (IsWalkingInputPressed())
        {
            SwitchTo(PlayerState.Walking);
        }
        else
        {
            SwitchTo(PlayerState.Idle);
        }
    }

    private void FixedUpdate()
    {
        if (State == PlayerState.Dead)
            return;

        Keyboard keyboard = Keyboard.current;
        Vector2 moveInput = Vector2.zero;

        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                moveInput.y = 1f;

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                moveInput.y = -1f;

            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                moveInput.x = -1f;

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                moveInput.x = 1f;
        }

        // Move forward and backward relative to the player.
        Vector3 movement =
            transform.forward * moveInput.y * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        // Preserve the original keyboard turning behavior.
        float turnDirection = moveInput.x;

        if (moveInput.y < 0)
            turnDirection = -turnDirection;

        float keyboardTurnDegrees =
            turnDirection * rotationSpeed * Time.fixedDeltaTime;

        yaw += keyboardTurnDegrees;

        Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(targetRotation);
    }

    private void LateUpdate()
    {
        if (State == PlayerState.Dead)
            return;

        // Keep the camera at a constant downward angle.
        if (cameraTransform != null)
        {
            cameraTransform.localRotation =
                Quaternion.Euler(fixedCameraPitch, 0f, 0f);
        }
    }

    private void SwitchTo(PlayerState nextState)
    {
        if (State == nextState)
            return;

        State = nextState;
        Debug.Log("Player state changed to: " + State);

        if (State == PlayerState.Dead)
        {
            if (swordAttack != null)
                swordAttack.CancelAttack();
        }
    }

    private bool IsWalkingInputPressed()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return false;

        return keyboard.wKey.isPressed ||
               keyboard.upArrowKey.isPressed ||
               keyboard.sKey.isPressed ||
               keyboard.downArrowKey.isPressed;
    }
}
