using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private ObjectDatabase objectDatabase;   // Reference to the ObjectDatabase
    [SerializeField] private Transform spawnPoint;            // Where to spawn bricks

    // Structure to hold information about the type and quantity of bricks to spawn
    [System.Serializable]
    public class BrickSpawnData
    {
        public int brickIndex;  // Index of the brick in the ObjectDatabase
        public int quantity;    // Quantity of this brick to spawn
        public float spawnInterval; // Interval between spawns for this brick type
    }

    [SerializeField] private BrickSpawnData[] bricksToSpawn;  // Array of bricks to spawn with quantities and intervals

    private void Start()
    {
        // Automatically start spawning the specified bricks with intervals
        foreach (var brickData in bricksToSpawn)
        {
            StartCoroutine(SpawnMultipleBricksWithInterval(brickData));
        }
    }

    /// <summary>
    /// Spawns a brick based on the provided index in the ObjectDatabase.
    /// </summary>
    /// <param name="brickIndex">Index of the brick in the ObjectDatabase.</param>
    public void SpawnBrick(int brickIndex)
    {
        if (brickIndex >= 0 && brickIndex < objectDatabase.objectsData.Count)
        {
            ObjectData objectData = objectDatabase.objectsData[brickIndex];

            // Instantiate the brick and assign its prefab reference
            Brick brickInstance = Instantiate(objectData.Prefab, spawnPoint.position, Quaternion.identity).GetComponent<Brick>();
            brickInstance.AssociatedObjectData = objectData;
            if (brickInstance != null)
            {
                brickInstance.PrefabReference = objectData.Prefab;
                Debug.Log($"Spawned brick: {objectData.Name}");
            }
            else
            {
                Debug.LogError("Failed to instantiate the brick prefab.");
            }
        }
        else
        {
            Debug.LogError("Invalid brick index provided for spawning.");
        }
    }

    /// <summary>
    /// Spawns multiple bricks of the same type at the spawn point with an interval.
    /// </summary>
    /// <param name="brickData">Data about the brick type, quantity, and spawn interval.</param>
    public IEnumerator SpawnMultipleBricksWithInterval(BrickSpawnData brickData)
    {
        if (brickData.brickIndex >= 0 && brickData.brickIndex < objectDatabase.objectsData.Count)
        {
            ObjectData objectData = objectDatabase.objectsData[brickData.brickIndex];

            for (int i = 0; i < brickData.quantity; i++)
            {
                // Instantiate the brick and assign its prefab reference
                Brick brickInstance = Instantiate(objectData.Prefab, spawnPoint.position, Quaternion.identity).GetComponent<Brick>();
                brickInstance.AssociatedObjectData = objectData;

                if (brickInstance != null)
                {
                    brickInstance.PrefabReference = objectData.Prefab;
                    Debug.Log($"Spawned brick {i + 1}: {objectData.Name}");
                }
                else
                {
                    Debug.LogError("Failed to instantiate the brick prefab.");
                }

                // Wait for the specified interval before spawning the next brick
                yield return new WaitForSeconds(brickData.spawnInterval);
            }
        }
        else
        {
            Debug.LogError("Invalid brick index provided for spawning.");
        }
    }
}

