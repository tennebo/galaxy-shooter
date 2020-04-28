using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle on-screen text displays.
/// </summary>
public class UIManager : MonoBehaviour
{
    internal static readonly string NAME = "UI_Manager";
    private static readonly string scorePrefix = "Score: ";
    private static readonly string killPrefix = "Kills: ";
    private static readonly int initialScore = -5;

    private GameManager gameManager;

    [SerializeField]
    private Text scoreText = default;

    [SerializeField]
    private Text killText = default;

    [SerializeField]
    private Text gameOverText = default;

    [SerializeField]
    private Text restartText = default;

    [SerializeField]
    private Sprite[] livesSprites = default;

    [SerializeField]
    private Image livesImage = default;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find(GameManager.NAME).GetComponent<GameManager>();
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);

        SetScore(initialScore);
        SetKills(0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    internal void OnGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        gameManager.OnGameOver();
    }

    internal void ResumePlay()
    {
        gameManager.ResumePlay();
    }

    internal void SetLives(int lives)
    {
        livesImage.sprite = livesSprites[lives];
    }

    internal void SetScore(int score)
    {
        scoreText.text = scorePrefix + score;
    }

    internal void SetKills(int kills)
    {
        killText.text = killPrefix + kills;
    }
}
