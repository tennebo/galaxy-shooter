using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn new enemies.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    internal static readonly string NAME = "Spawn_Manager";

    [SerializeField]
    GameObject enemyPrefab = default;

    [SerializeField]
    GameObject enemyContainer = default;

    // Create a new enemy every few seconds
    [Min(1)]
    [SerializeField]
    [Tooltip("How many seconds between creating each new enemy")]
    int intervalSeconds = 5;

    bool gameOver = false;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Signal a stop to the enemy creation stream
    internal void OnGameOver()
    {
        gameOver = true;
    }

    // Infinite stream of new enemies, with a delay between each one
    IEnumerator SpawnEnemies()
    {
        while (!gameOver)
        {
            //Debug.Log("Creating new enemy from prefab <i>" + enemyPrefab.name + "</i>");
            var enemy = Instantiate(enemyPrefab, Enemy.InitialPos(), Quaternion.identity);
            enemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(intervalSeconds);
        }
    }
}
