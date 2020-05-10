using UnityEngine;

/// <summary>
/// A "power up" collectible.
/// </summary>
public class Powerup : MonoBehaviour
{
    static readonly string TRIPLE_SHOT_TAG = "TripleShot";
    static readonly string SHIELD_TAG = "Shield";
    static readonly string SPEED_TAG = "Speed";

    // Vertical speed of the object
    [Min(0)]
    [SerializeField]
    [Tooltip("Power-up speed")]
    float speed = 4.0f;

    Player player;

    void Awake()
    {
        // Get a reference to the player
        this.player = GameObject.Find(Player.NAME).GetComponent<Player>();
    }

    void Update()
    {
        Move();

        // Destroy it if it has left the visible screen
        if (transform.position.y < Constants.bottomY)
            DestroySelf();
    }

    void Move()
    {
        Vector3 translation = Vector3.down * speed * Time.deltaTime;
        transform.Translate(translation);
    }

    // We have collided with some other object
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("<b><color=red>Power-up</color></b> hit <b><color=green>" + other.name + "</color></b>");

        if (other.CompareTag(Player.TAG))
        {
            // Collect power-up
            if (this.CompareTag(TRIPLE_SHOT_TAG))
            {
                player.ActivateTripleShot();
            }
            if (this.CompareTag(SPEED_TAG))
            {
                player.ActivateSpeedBoost();
            }
        }
        DestroySelf();
    }

    // Destroy this power-up object
    void DestroySelf()
    {
        Debug.Log("Destroying power-up");
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 0.2f);
    }

    /// <summary>
    /// A random position at the top of the screen.
    /// </summary>
    internal static Vector3 InitialPos()
    {
        return new Vector3(Random.Range(Constants.minX, Constants.maxX), Constants.topY, 0);
    }
}
