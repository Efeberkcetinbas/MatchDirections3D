using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private ObjectDatabase objectDatabase; // Reference to the ObjectDatabase
    [SerializeField] private Transform spawnPoint;          // Where to spawn bricks
    [SerializeField] private int defaultBrickIndex = 0;     // Index of the default brick to spawn on Start

    private void Start()
    {
        // Automatically spawn the default brick when the scene starts
        SpawnBrick(defaultBrickIndex);
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
}
