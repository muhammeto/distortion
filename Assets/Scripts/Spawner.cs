using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector2 minPos = Vector2.zero,maxPos = Vector2.zero;
    [SerializeField] private GameObject trianglePrefab = null;
    [SerializeField] private int startSpawnTime = 1;
    [SerializeField] private int spawnTime = 3;

    private void Start()
    {
        InvokeRepeating("Spawn", startSpawnTime, spawnTime);
    }

    void Spawn()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(minPos.x,maxPos.x), Random.Range(minPos.y, maxPos.y));
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(trianglePrefab, spawnPosition, trianglePrefab.transform.rotation);
    }
}