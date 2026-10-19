using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button playButton = root.Q<Button>("playButton");

        playButton.clicked += PlayGame;
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }
}