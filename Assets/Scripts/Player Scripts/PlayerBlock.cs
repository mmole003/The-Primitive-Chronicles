using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    [Header("Block Settings")]
    public float blockRotation = -60f;

    private Quaternion normalRotation;

    private bool isBlocking = false;

    public bool IsBlocking
    {
        get { return isBlocking; }
    }

    void Start()
    {
        normalRotation = transform.localRotation;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.isPressed)
        {
            StartBlocking();
        }
        else
        {
            StopBlocking();
        }
    }

    void StartBlocking()
    {
        if (isBlocking)
            return;

        isBlocking = true;

        transform.localRotation =
            normalRotation *
            Quaternion.Euler(0f, blockRotation, 0f);
    }

    void StopBlocking()
    {
        if (!isBlocking)
            return;

        isBlocking = false;

        transform.localRotation = normalRotation;
    }
}