using UnityEngine;

/// <summary>
/// A laser "beam".
/// </summary>
public class Laser : MonoBehaviour
{
    internal static readonly string TAG = "Laser";

    // Upper limit of the visible screen
    private static readonly float uLimit = 8f;

    // Vertical speed of the beam
    [SerializeField]
    private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        // Destroy it if it has left the visible screen
        if (uLimit < transform.position.y)
            Destroy(this.gameObject);
    }

    private void Move()
    {
        Vector3 translation = Vector3.up * speed * Time.deltaTime;
        transform.Translate(translation);
    }
}
