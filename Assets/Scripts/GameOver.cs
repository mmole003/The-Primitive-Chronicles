using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button restartButton;
    private Button mainMenuButton;
    private Button exitButton;

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        // Hide Game Over when the scene starts
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;

        restartButton = uiDocument.rootVisualElement.Q<Button>("restartButton");
        mainMenuButton = uiDocument.rootVisualElement.Q<Button>("mainMenuButton");
        exitButton = uiDocument.rootVisualElement.Q<Button>("exitButton");

        restartButton.clicked += RestartGame;
        mainMenuButton.clicked += ReturnToMainMenu;
        exitButton.clicked += ExitGame;
    }

    void OnDestroy()
    {
        if (restartButton != null)
        {
            restartButton.clicked -= RestartGame;
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowGameOver()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}