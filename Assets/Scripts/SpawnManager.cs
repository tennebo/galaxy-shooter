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
    [Tooltip("How many seconds between creating a shield power-up")]
    int shieldPowerupSeconds = 100;

    [Min(1)]
    [SerializeField]
    [Tooltip("How many seconds between creating a speed boost power-up")]
    int speedPowerupSeconds = 30;

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
            yield return new WaitForSeconds(WaitForRandom(shieldPowerupSeconds));
            Instantiate(shieldPrefab, Powerup.InitialPos(), Quaternion.identity);
        }
    }

    // Infinite stream of new speed power-ups
    IEnumerator SpawnSpeedPowerUps()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(WaitForRandom(speedPowerupSeconds));
            Instantiate(speedPrefab, Powerup.InitialPos(), Quaternion.identity);
        }
    }

    static float WaitForRandom(float max)
    {
        return Random.Range(max / 10f, max);
    }
}
