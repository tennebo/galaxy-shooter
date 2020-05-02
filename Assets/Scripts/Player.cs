using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>   
/// The game player (shooter).
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    internal static readonly string NAME = "Player";
    internal static readonly string TAG = "Player";

    // Upper and lower screen boundaries
    static readonly Vector3 uBound = new Vector3(Constants.maxX, Constants.topY, 0);
    static readonly Vector3 lBound = new Vector3(Constants.minX, Constants.bottomY, 0);

    // Laser originates this far from self
    static readonly Vector3 fireOffset = new Vector3(0, 1f, 0);

    [SerializeField]
    float speed = 9.0f;

    [SerializeField]
    int lives = 3;

    [SerializeField]
    int score = 0;

    // Accumulated count of enemy kills
    int kills = 0;

    [SerializeField]
    bool wrapHorizontal = false;

    [SerializeField]
    bool isTripleShotActive = false;

    [SerializeField]
    GameObject tripleShotPrefab = default;

    [SerializeField]
    GameObject laserPrefab = default;

    [SerializeField]
    GameObject leftEngineFire = default;

    [SerializeField]
    GameObject rightEngineFire = default;

    [SerializeField]
    AudioClip laserAudioClip = default;

    AudioSource laserAudioSource;
    SpawnManager spawnManager;
    UIManager uiManager;
    PlayerInputActions playerActions;

    float horizontalInput;
    float verticalInput;

    void Awake()
    {
        this.playerActions = new PlayerInputActions();
        this.spawnManager = GameObject.Find(SpawnManager.NAME).GetComponent<SpawnManager>();
        this.uiManager = GameObject.Find(UIManager.NAME).GetComponent<UIManager>();
        this.laserAudioSource = GetComponent<AudioSource>();
        this.laserAudioSource.clip = this.laserAudioClip;
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }

    void Start()
    {
        Debug.Log("Created player positioned at origin");
        this.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Move first
        Move();

        // ...then fire
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            Fire();
        }
        if (Keyboard.current[Key.T].wasPressedThisFrame)
        {
            isTripleShotActive = !isTripleShotActive;
        }
    }

    internal void OnEnemyKill(int scoreIncrement)
    {
        kills++;
        score += scoreIncrement;
        uiManager.SetScore(score);
        uiManager.SetKills(kills);
    }

    internal void InflictDamage()
    {
        lives--;
        Debug.Log("Damage, remaining lives: <color=green>" + lives + "</color>");
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

    // Called if the Unity input behavior is 'SendMessages'
    public void OnFire(InputValue value)
    {
        Debug.Log("Fire with input <color=red>" + value.GetType() + "</color>");
        Fire();
    }

    // Called if the Unity input behavior is 'SendMessages'
    public void OnMove(InputValue value)
    {
        // Capture the input for use by the Update function later
        var input = value.Get<Vector2>();
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    void Move()
    {
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        Vector3 translation = direction * speed * Time.deltaTime;
        transform.Translate(translation);
        transform.position = Clamp(transform.position);
    }

    void GameOver()
    {
        Debug.Log("Game over");
        uiManager.OnGameOver();
        spawnManager.OnGameOver();
        Destroy(this.gameObject, 1f);
    }

    void Fire()
    {
        // Fire at a slight offset from the player
        var fireAt = transform.position + fireOffset;
        if (isTripleShotActive)
        {
            Debug.Log("Firing triple-shot laser at " + fireAt);
            Instantiate(tripleShotPrefab, fireAt, Quaternion.identity);
        }
        else
        {
            Debug.Log("Firing single laser at " + fireAt);
            Instantiate(laserPrefab, fireAt, Quaternion.identity);
        }
        // Play audio
        laserAudioSource.Play();

    }

    Vector3 Clamp(Vector3 v) {
        float x = wrapHorizontal ?
            WrapX(v.x) : Mathf.Clamp(v.x, lBound.x, uBound.x);
        return new Vector3(
            x,
            Mathf.Clamp(v.y, lBound.y, uBound.y),
            Mathf.Clamp(v.z, lBound.z, uBound.z));
    }

    static float WrapX(float x) {
        if (x < lBound.x)
            return uBound.x;
        if (uBound.x < x)
            return lBound.x;
        return x;
    }
}
