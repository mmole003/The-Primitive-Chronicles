using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private VisualElement pauseMenu;
    private Button resumeButton;
    private Button exitButton;

    private bool isPaused = false;

    private void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("PauseMenu: No UIDocument found!");
            return;
        }

        VisualElement root = uiDocument.rootVisualElement;

        pauseMenu = root.Q<VisualElement>("PauseMenu");
        resumeButton = root.Q<Button>("resumeButton");
        exitButton = root.Q<Button>("exitButton");

        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu: Could not find PauseMenu!");
            return;
        }

        if (resumeButton == null)
        {
            Debug.LogError("PauseMenu: Could not find resumeButton!");
        }
        else
        {
            resumeButton.clicked += ResumeGame;
            Debug.Log("PauseMenu: Resume button connected.");
        }

        if (exitButton == null)
        {
            Debug.LogError("PauseMenu: Could not find exitButton!");
        }
        else
        {
            exitButton.clicked += QuitGame;
            Debug.Log("PauseMenu: Exit button connected.");
        }

        // Make sure the pause menu starts hidden.
        pauseMenu.style.display = DisplayStyle.None;

        // Make absolutely sure the game isn't starting paused.
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Don't allow the pause menu after the player dies.
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

            if (playerHealth != null && playerHealth.IsDead)
            {
                return;
            }

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        Debug.Log("PAUSE");

        isPaused = true;

        pauseMenu.style.display = DisplayStyle.Flex;

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;

        Debug.Log("Time.timeScale is now: " + Time.timeScale);
    }

    private void ResumeGame()
    {
        Debug.Log("RESUME BUTTON PRESSED");

        isPaused = false;

        Time.timeScale = 1f;

        pauseMenu.style.display = DisplayStyle.None;

        Debug.Log("Game resumed.");
        Debug.Log("Time.timeScale is now: " + Time.timeScale);
    }

    private void QuitGame()
    {
        Debug.Log("EXIT BUTTON PRESSED");
        Debug.Log("Quitting game...");

        // Make sure the game isn't left paused.
        Time.timeScale = 1f;

        Application.Quit();
    }
}
