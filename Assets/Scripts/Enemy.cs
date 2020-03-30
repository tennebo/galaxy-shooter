using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    internal static readonly string TAG = "Enemy";
    internal static readonly string DEATH_TRIGGER = "OnEnemyDeath";

    private static readonly float topY = 8f;
    private static readonly float bottomY = -8f;
    private static readonly float minX = -11f;
    private static readonly float maxX = 11f;

    [SerializeField]
    private float speed = 3.0f;

    [SerializeField]
    private AudioSource explosionAudioSource;

    [SerializeField]
    private Animator explosionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        explosionAudioSource = GetComponent<AudioSource>();
        print("Created enemy with audio source " + explosionAudioSource.clip.name);

        explosionAnimator= GetComponent<Animator>();
        print("Created enemy with animator " + explosionAnimator.name);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Enemy collided with " + other.name);

        if (other.CompareTag(Laser.TAG))
        {
            print("Enemy hit by laser");
            Destroy(other.gameObject);
            BlowUpSelf();
        }

        if (other.CompareTag(Player.TAG))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                print("Collided with enemy");
                player.Damage();
            }
            BlowUpSelf();
        }
    }

    private void BlowUpSelf()
    {
        print("Destroying enemy");
        speed = 0; // Stop the movement
        explosionAnimator.SetTrigger(DEATH_TRIGGER); // Start explosion animation
        explosionAudioSource.Play();
        Destroy(this.gameObject, 2.8f);
    }

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

    internal static Vector3 InitialPos()
    {
        return new Vector3(Random.Range(minX, maxX), topY, 0);
    }
}
