using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle user events from the main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    private static readonly string gameScene = "Game";

    public void LoadGame()
    {
        Debug.Log("Loading new game");
        SceneManager.LoadScene(gameScene);
    }
}
