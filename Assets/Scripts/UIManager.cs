using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle on-screen text displays.
/// </summary>
public class UIManager : MonoBehaviour
{
    internal static readonly string NAME = "UI_Manager";

    static readonly string scorePrefix = "Score: ";
    static readonly string killPrefix = "Kills: ";
    static readonly int initialScore = -5;

    GameManager gameManager;

    [SerializeField]
    Text scoreText = default;

    [SerializeField]
    Text killText = default;

    [SerializeField]
    Text gameOverText = default;

    [SerializeField]
    Text restartText = default;

    [SerializeField]
    Sprite[] livesSprites = default;

    [SerializeField]
    Image livesImage = default;


    void Awake()
    {
        gameManager = GameObject.Find(GameManager.NAME).GetComponent<GameManager>();
    }

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);

        SetScore(initialScore);
        SetKills(0);
    }

    internal void OnGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        gameManager.OnGameOver();
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
