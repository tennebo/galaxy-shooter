using UnityEngine;

/// <summary>
/// An enemy object.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    internal static readonly string TAG = "Enemy";
    internal static readonly string DEATH_TRIGGER = "OnEnemyDeath";

    // Points per enemy kill
    private static readonly int killScore = 11;

    // Enemy speed while exploding
    private static readonly float speedAfterKill = 1.0f;

    // Time it takes enemy to explode
    private static readonly float deathTimespan = 2.8f;

    private static readonly float topY = 8f;
    private static readonly float bottomY = -8f;
    private static readonly float minX = -11f;
    private static readonly float maxX = 11f;

    // Vertical drop speed
    [SerializeField]
    private float speed = 3.0f;

    private AudioSource explosionAudioSource;
    private Animator explosionAnimator;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        this.explosionAudioSource = GetComponent<AudioSource>();
        print("Created enemy with audio source " + this.explosionAudioSource.clip.name);

        this.explosionAnimator = GetComponent<Animator>();
        print("Created enemy with animator " + this.explosionAnimator.name);

        // Get a reference to the player
        this.player = GameObject.Find(Player.NAME).GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// A random position at the top of the screen.
    /// </summary>
    internal static Vector3 InitialPos()
    {
        return new Vector3(Random.Range(minX, maxX), topY, 0);
    }

    // Enemy has collided with some other object
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Enemy collided with " + other.name);

        if (other.CompareTag(Laser.TAG))
        {
            print("Enemy hit by laser");
            Destroy(other.gameObject);
            player.EnemyKill(killScore);
        }
        if (other.CompareTag(Player.TAG))
        {
            print("Player collided with enemy");
            player.Damage();
        }
        BlowUpSelf();
    }

    // Destroy this enemy
    private void BlowUpSelf()
    {
        print("Destroying enemy");
        speed = speedAfterKill; // Stop (or slow down) the movement
        explosionAnimator.SetTrigger(DEATH_TRIGGER); // Start explosion animation
        explosionAudioSource.Play();

        Destroy(GetComponent<Collider2D>()); // Disable further hits while we die
        Destroy(this.gameObject, deathTimespan);
    }

    // Move this enemy
    private void Move()
    {
        Vector3 translation = Vector3.down * speed * Time.deltaTime;
        transform.Translate(translation);

        // Wrap around if it dropped off
        if (transform.position.y < bottomY)
        {
            transform.position = InitialPos();
        }
    }
}
