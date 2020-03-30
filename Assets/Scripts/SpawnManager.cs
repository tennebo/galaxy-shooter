using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    internal static readonly string NAME = "Spawn_Manager";

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject enemyContainer;

    [SerializeField]
    private int intervalSeconds = 5;

    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
    }

    internal void Stop()
    {
        stop = true;
    }

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
