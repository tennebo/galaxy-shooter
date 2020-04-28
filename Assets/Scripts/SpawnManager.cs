using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn new enemies.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    internal static readonly string NAME = "Spawn_Manager";

    [SerializeField]
    private GameObject enemyPrefab = default;

    [SerializeField]
    private GameObject enemyContainer = default;

    // Create a new enemy every few seconds
    [SerializeField]
    private int intervalSeconds = 5;

    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Signal a stop to the enemy creation stream
    internal void OnGameOver()
    {
        stop = true;
    }

    // Infinite stream of new enemies, with a delay between each one
    private IEnumerator SpawnEnemies()
    {
        while (!stop)
        {
            print("Creating new enemy from prefab");
            var enemy = Instantiate(enemyPrefab, Enemy.InitialPos(), Quaternion.identity);
            enemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(intervalSeconds);
        }
    }
}
