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

    [SerializeField]
    GameObject tripleShotPrefab = default;

    [SerializeField]
    GameObject shieldPrefab = default;

    [SerializeField]
    GameObject speedPrefab = default;

    [Min(1)]
    [SerializeField]
    [Tooltip("How many seconds between creating each new enemy")]
    int intervalSeconds = 5;

    [Min(1)]
    [SerializeField]
    [Tooltip("How many seconds between creating a triple-shot power-up")]
    int tripleShotPowerupSeconds = 50;

    [Min(1)]
    [SerializeField]
    [Tooltip("How many seconds between creating a triple-shot power-up")]
    int shieldPowerupSeconds = 150;

    [Min(1)]
    [SerializeField]
    [Tooltip("How many seconds between creating a triple-shot power-up")]
    int speedPowerupSeconds = 20;

    bool gameOver = false;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnTripleShotPowerUps());
        StartCoroutine(SpawnShieldPowerUps());
        StartCoroutine(SpawnSpeedPowerUps());
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

    // Infinite stream of new triple-shot power-ups
    IEnumerator SpawnTripleShotPowerUps()
    {
        while (!gameOver)
        {
            Instantiate(tripleShotPrefab, Powerup.InitialPos(), Quaternion.identity);
            yield return new WaitForSeconds(WaitForRandom(tripleShotPowerupSeconds));
        }
    }

    // Infinite stream of new shield power-ups
    IEnumerator SpawnShieldPowerUps()
    {
        while (!gameOver)
        {
            Instantiate(shieldPrefab, Powerup.InitialPos(), Quaternion.identity);
            yield return new WaitForSeconds(WaitForRandom(shieldPowerupSeconds));
        }
    }

    // Infinite stream of new speed power-ups
    IEnumerator SpawnSpeedPowerUps()
    {
        while (!gameOver)
        {
            Instantiate(speedPrefab, Powerup.InitialPos(), Quaternion.identity);
            yield return new WaitForSeconds(WaitForRandom(speedPowerupSeconds));
        }
    }

    static float WaitForRandom(float max)
    {
        return Random.Range(max / 10f, max);
    }
}
