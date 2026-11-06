using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    [Header("Menu References")]
    public PauseMenu pauseMenu;
    public GameOver gameOverMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentState = GameState.Playing;
        ApplyGameState();
        OnGameStateChanged?.Invoke(CurrentState);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (CurrentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (CurrentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        ChangeState(GameState.Playing);
    }

    public void TriggerGameOver()
    {
        if (CurrentState == GameState.GameOver)
            return;

        ChangeState(GameState.GameOver);
    }

    private void ChangeState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        ApplyGameState();
        OnGameStateChanged?.Invoke(CurrentState);
    }

    private void ApplyGameState()
    {
        switch (CurrentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;

                if (pauseMenu != null)
                    pauseMenu.HideMenu();

                if (gameOverMenu != null)
                    gameOverMenu.HideMenu();

                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;

                if (pauseMenu != null)
                    pauseMenu.ShowMenu();

                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;

                if (pauseMenu != null)
                    pauseMenu.HideMenu();

                if (gameOverMenu != null)
                    gameOverMenu.ShowGameOver();

                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}