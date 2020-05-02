using UnityEngine;

/// <summary>
/// A laser "beam".
/// </summary>
public class Laser : MonoBehaviour
{
    internal static readonly string TAG = "Laser";

    // Vertical speed of the beam
    [Min(0)]
    [SerializeField]
    [Tooltip("Laser beam speed")]
    float speed = 10.0f;

    void Update()
    {
        Move();

        // Destroy it if it has left the visible screen
        if (Constants.topY < transform.position.y)
            Destroy(this.gameObject);
    }

    void Move()
    {
        Vector3 translation = Vector3.up * speed * Time.deltaTime;
        transform.Translate(translation);
    }
}
