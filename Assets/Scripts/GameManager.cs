using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle non-gameplay user inputs such as quitting and restarting.
///
/// Key P: Pause by freezing time and displaying the pause menu panel
/// Key R: Resume by un-freezing time and hiding the pause menu panel
/// Key S: Slow down time
/// </summary>
public class GameManager : MonoBehaviour
{
    internal static readonly string NAME = "Game_Manager";

    static readonly string gameScene = "Game";
    static readonly string mainMenuScene = "Main_Menu";

    [SerializeField]
    GameObject pauseMenuPanel = default;

    // Slow motion mode
    [SerializeField]
    bool slowMo;

    // True if we are in a 'game over' state
    bool gameOver;

    bool IsPaused { get { return pauseMenuPanel.activeSelf; } }

    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
        {
            if (Keyboard.current[Key.R].wasPressedThisFrame)
            {
                SceneManager.LoadScene(gameScene);
            }
        }
        else
        {
            // In normal play mode
            if (Keyboard.current[Key.P].wasPressedThisFrame)
            {
                PausePlay();
            }
            if (Keyboard.current[Key.Escape].wasPressedThisFrame)
            {
                LoadMainMenu();
            }
            if (Keyboard.current[Key.Z].wasPressedThisFrame)
            {
                FlipSlowMo();
            }
        }
        if (IsPaused)
        {
            // In paused mode
            if (Keyboard.current[Key.R].wasPressedThisFrame)
            {
                ResumePlay();
            }
            if (Keyboard.current[Key.M].wasPressedThisFrame)
            {
                LoadMainMenu();
            }
        }
    }

    void FlipSlowMo()
    {
        slowMo = !slowMo;
        Time.timeScale = slowMo ? 0.1f : 1;
        Debug.Log("Time scale is <b>" + Time.timeScale + "</b>");
    }

    void PausePlay()
    {
        Debug.Log("Pausing");
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
    }

    void ResumePlay()
    {
        Debug.Log("Resuming");
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    internal void LoadMainMenu()
    {
        Debug.Log("Loading the main menu scene");
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1;
    }

    internal void OnGameOver()
    {
        Debug.Log("Setting mode to 'game over'");
        gameOver = true;
    }
}
