using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle   non-gameplay user inputs such as quitting and restarting.
/// </summary>
public class GameManager : MonoBehaviour
{
    internal static readonly string NAME = "Game_Manager";

    private static readonly string gameScene = "Game";
    private static readonly string mainMenuScene = "Main_Menu";

    [SerializeField]
    private GameObject pauseMenuPanel = default;

    private bool gameOver;


    bool IsPaused { get { return pauseMenuPanel.activeSelf; } }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(gameScene);
            }
        }
        else
        {
            // In normal play mode
            if (Input.GetKeyDown(KeyCode.P))
            {
                PausePlay();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainMenu();
            }
        }
        if (IsPaused)
        {
            // In paused mode
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResumePlay();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                LoadMainMenu();
            }
        }
    }

    internal void PausePlay()
    {
        Debug.Log("Pausing");
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
    }

    public void ResumePlay()
    {
        Debug.Log("Resuming");
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
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
