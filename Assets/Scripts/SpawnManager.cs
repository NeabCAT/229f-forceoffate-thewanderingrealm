using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Spawn Points (เรียงลำดับจากซ้ายไปขวา)")]
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    private int currentSpawnIndex = -1; 
    private Vector3 defaultSpawnPosition;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            defaultSpawnPosition = player.transform.position;
    }

    public void TryActivate(SpawnPoint triggered)
    {
        int triggeredIndex = spawnPoints.IndexOf(triggered);

        if (triggeredIndex == -1) return;                 
        if (triggered.isActivated) return;                 
        if (triggeredIndex <= currentSpawnIndex) return;   

        currentSpawnIndex = triggeredIndex;
        triggered.Activate();

        Debug.Log($"SpawnPoint [{triggeredIndex}] activated!");
    }

    public Vector3 GetRespawnPosition()
    {
        if (currentSpawnIndex == -1)
            return defaultSpawnPosition;

        return spawnPoints[currentSpawnIndex].transform.position;
    }
}