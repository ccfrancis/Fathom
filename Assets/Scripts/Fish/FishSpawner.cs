using UnityEngine;

/// <summary>
/// Simple fish spawner that maintains a population of fish in the area
/// </summary>
public class FishSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private int maxFishCount = 5;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float initialSpawnDelay = 1f;

    [Header("Spawn Area")]
    [SerializeField] private float spawnMinX = -15f;
    [SerializeField] private float spawnMaxX = 15f;
    [SerializeField] private float spawnMinY = -10f;
    [SerializeField] private float spawnMaxY = 0f;

    [Header("Debug")]
    [SerializeField] private int currentFishCount = 0;

    private float nextSpawnTime;

    void Start()
    {
        // Initial spawn
        nextSpawnTime = Time.time + initialSpawnDelay;
    }

    void Update()
    {
        UpdateFishCount();

        // Spawn new fish if below max count
        if (Time.time >= nextSpawnTime && currentFishCount < maxFishCount)
        {
            SpawnFish();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void UpdateFishCount()
    {
        // Count active fish in scene
        currentFishCount = GameObject.FindGameObjectsWithTag("Fish").Length;
    }

    void SpawnFish()
    {
        if (fishPrefab == null)
        {
            Debug.LogError("FishSpawner: Fish prefab not assigned!");
            return;
        }

        // Random position within spawn area
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Spawn the fish
        GameObject fishObj = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
        fishObj.transform.parent = transform; // Organize under spawner

        currentFishCount++;
        Debug.Log($"Spawned fish at {spawnPosition}. Total fish: {currentFishCount}");
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = Random.Range(spawnMinY, spawnMaxY);
        return new Vector3(x, y, 0);
    }

    void OnDrawGizmos()
    {
        // Draw spawn area
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((spawnMinX + spawnMaxX) / 2f, (spawnMinY + spawnMaxY) / 2f, 0);
        Vector3 size = new Vector3(spawnMaxX - spawnMinX, spawnMaxY - spawnMinY, 0);
        Gizmos.DrawWireCube(center, size);

        // Draw spawn area label
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(center, $"Fish Spawn Area\n{currentFishCount}/{maxFishCount} Fish");
        #endif
    }
}
