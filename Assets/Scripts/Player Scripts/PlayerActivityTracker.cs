using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActivityTracker : MonoBehaviour
{
    // Event to notify when the player has gone idle for a minute
    public event Action OnPlayerIdle;

    public float idleTime = 60f;

    private float currentIdleTime = 0f;
    private bool hasGoneIdle = false;

    private void Update()
    {
        if (HasPlayerInput())
        {
            currentIdleTime = 0f;
            hasGoneIdle = false;
            return;
        }

        currentIdleTime += Time.deltaTime;

        if (currentIdleTime >= idleTime && !hasGoneIdle)
        {
            hasGoneIdle = true;

            OnPlayerIdle?.Invoke();
        }
    }

    private bool HasPlayerInput()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        if (Mouse.current != null &&
            (Mouse.current.leftButton.wasPressedThisFrame ||
             Mouse.current.rightButton.wasPressedThisFrame))
            return true;

        return false;
    }
}