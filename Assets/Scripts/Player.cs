using UnityEngine;

/// <summary>   
/// The game player (shooter).
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    internal static readonly string NAME = "Player";
    internal static readonly string TAG = "Player";

    private static readonly Vector3 uBound = new Vector3(11, 6, 0);
    private static readonly Vector3 lBound = new Vector3(-11, -6, 0);
    private static readonly Vector3 fireOffset = new Vector3(0, 1f, 0);

    [SerializeField]
    private float speed = 6.0f;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private int score = 0;
    private int kills = 0;

    [SerializeField]
    private bool wrapHorizontal = false;

    [SerializeField]
    private bool isTripleShotActive = false;

    [SerializeField]
    private GameObject tripleShotPrefab = default;

    [SerializeField]
    private GameObject laserPrefab = default;

    [SerializeField]
    private GameObject leftEngineFire = default;

    [SerializeField]
    private GameObject rightEngineFire = default;

    [SerializeField]
    private AudioClip laserAudioClip = default;

    private AudioSource laserAudioSource;
    private SpawnManager spawnManager;
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        this.spawnManager = GameObject.Find(SpawnManager.NAME).GetComponent<SpawnManager>();
        this.uiManager = GameObject.Find(UIManager.NAME).GetComponent<UIManager>();
        this.laserAudioSource = GetComponent<AudioSource>();
        this.laserAudioSource.clip = this.laserAudioClip;

        print("Created player positioned at origin");
        this.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Move first, then fire
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // Fire at a slight offset from the player
        var fireAt = transform.position + fireOffset;
        if (isTripleShotActive)
        {
            print("Firing triple-shot laser at " + fireAt);
            Instantiate(tripleShotPrefab, fireAt, Quaternion.identity);
        }
        else
        {
            print("Firing single laser at " + fireAt);
            Instantiate(laserPrefab, fireAt, Quaternion.identity);
        }
        // Play audio
        laserAudioSource.Play();

    }

    internal void EnemyKill(int addition)
    {
        kills++;
        score += addition;
        uiManager.SetScore(score);
        uiManager.SetKills(kills);
    }

    internal void Damage()
    {
        lives--;
        print("Damage, remaining lives: " + lives);
        uiManager.SetLives(lives);

        switch (lives)
        {
            case 0:
                GameOver();
                break;
            case 1:
                // Make both engines burn
                leftEngineFire.SetActive(true);
                rightEngineFire.SetActive(true);
                break;
            case 2:
                // Make one engine burn
                float r = Random.Range(0f, 1f);
                if (r < 0.5)
                    leftEngineFire.SetActive(true);
                else
                    rightEngineFire.SetActive(true);
                break;
            default:
                // Nothing
                break;
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        Vector3 translation = direction * speed * Time.deltaTime;
        transform.Translate(translation);
        transform.position = Clamp(transform.position);
        //print("Moving player to " + boundedTranslation);
    }

    private void GameOver()
    {
        print("Game over");
        uiManager.OnGameOver();
        spawnManager.OnGameOver();
        Destroy(this.gameObject, 1f);
    }

    private Vector3 Clamp(Vector3 v) {
        float x = wrapHorizontal ?
            WrapX(v.x) : Mathf.Clamp(v.x, lBound.x, uBound.x);
        return new Vector3(
            x,
            Mathf.Clamp(v.y, lBound.y, uBound.y),
            Mathf.Clamp(v.z, lBound.z, uBound.z));
    }

    private float WrapX(float x) {
        if (x < lBound.x)
            return uBound.x;
        if (uBound.x < x)
            return lBound.x;
        return x;
    }
}
