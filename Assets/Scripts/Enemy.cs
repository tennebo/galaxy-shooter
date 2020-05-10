using UnityEngine;

/// <summary>
/// An enemy object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
class Enemy : MonoBehaviour
{
    internal static readonly string TAG = "Enemy";
    internal static readonly string DEATH_TRIGGER = "OnEnemyDeath";

    // Points per enemy kill
    static readonly int killScore = 11;

    // Enemy speed while exploding
    static readonly float speedAfterKill = 1.0f;

    // Time it takes enemy to explode
    static readonly float deathTimespan = 2.8f;

    // Vertical drop speed
    [Min(0)]
    [SerializeField]
    [Tooltip("Vertical enemy drop speed")]
    float speed = 3.0f;

    [SerializeField]
    AudioClip explosionClip = default;

    [SerializeField]
    AudioClip ohNoClip = default;

    [SerializeField]
    AudioClip screamClip = default;

    AudioSource explosionAudioSource;
    Animator explosionAnimator;
    Player player;

    void Awake()
    {
        // Get a reference to the player
        this.player = GameObject.Find(Player.NAME).GetComponent<Player>();
        this.explosionAudioSource = GetComponent<AudioSource>();
        this.explosionAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        // This generates a lot of log statements
        //Debug.Log("Created enemy with audio source " + this.explosionAudioSource.clip.name);
        //Debug.Log("Created enemy with animator " + this.explosionAnimator.name);
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// A random position at the top of the screen.
    /// </summary>
    internal static Vector3 InitialPos()
    {
        return new Vector3(Random.Range(Constants.minX, Constants.maxX), Constants.topY, 0);
    }

    // Enemy has collided with some other object
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("<b><color=red>Enemy</color></b> collided with <b><color=green>" + other.name + "</color></b>");

        if (other.CompareTag(Laser.TAG))
        {
            Destroy(other.gameObject);
            player.OnEnemyKill(killScore);
        }
        if (other.CompareTag(Player.TAG))
        {
            player.InflictDamage();
        }
        BlowUpSelf();
    }

    // Destroy this enemy
    void BlowUpSelf()
    {
        Debug.Log("Destroying enemy");
        speed = speedAfterKill; // Stop (or slow down) the movement
        explosionAnimator.SetTrigger(DEATH_TRIGGER); // Start explosion animation

        explosionAudioSource.clip = RandomClip();
        explosionAudioSource.Play();

        Destroy(GetComponent<Collider2D>()); // Disable further hits while we die
        Destroy(this.gameObject, deathTimespan);
    }

    // Move this enemy
    void Move()
    {
        Vector3 translation = Vector3.down * speed * Time.deltaTime;
        transform.Translate(translation);

        // Wrap around if it dropped off
        if (transform.position.y < Constants.bottomY)
        {
            transform.position = InitialPos();
        }
    }

    // Pick an audio clip to play
    AudioClip RandomClip()
    {
        var clips = new AudioClip[] { explosionClip, ohNoClip, screamClip };
        int ix = Random.Range(0, clips.Length);
        return clips[ix];
    }
}
