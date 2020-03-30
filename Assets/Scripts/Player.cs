using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    internal static readonly string TAG = "Player";

    private static readonly Vector3 uBound = new Vector3(11, 6, 0);
    private static readonly Vector3 lBound = new Vector3(-11, -6, 0);
    private static readonly Vector3 fireOffset = new Vector3(0, 1f, 0);

    [SerializeField]
    private float speed = 6.0f;

    [SerializeField]
    private int lives = 9;

    [SerializeField]
    private bool wrapHorizontal = false;

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private AudioSource laserAudioSource;

    [SerializeField]
    private AudioClip laserAudioClip;

    [SerializeField]
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find(SpawnManager.NAME).GetComponent<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("Could not find " + SpawnManager.NAME);
        }

        laserAudioSource = GetComponent<AudioSource>();
        laserAudioSource.clip = laserAudioClip;

        print("Created player positioned at origin");
        transform.position = Vector3.zero;
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
        print("Firing laser at " + fireAt);
        Instantiate(laserPrefab, fireAt, Quaternion.identity);

        // Play audio
        laserAudioSource.Play();

    }

    internal void Damage()
    {
        print("Damage, remaining lives: " + lives);
        if (--lives == 0)
        {
            print("Game over");
            spawnManager.Stop();
            Destroy(this.gameObject);
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
