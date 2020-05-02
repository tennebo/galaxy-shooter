using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle user events from the main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    static readonly string gameScene = "Game";

    internal void LoadGame()
    {
        Debug.Log("Loading new game");
        SceneManager.LoadScene(gameScene);
    }
}
