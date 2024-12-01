using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    
    public LevelConfiguration currentLevelConfig; // Assign via the LevelManager

    private Queue<GameObject> productPool = new Queue<GameObject>();

    private void Start()
    {
        if (currentLevelConfig == null)
        {
            Debug.LogError("LevelConfig is not assigned!");
            return;
        }

        InitializePool(currentLevelConfig.productSpawnConfigs);
        SpawnProducts();
    }

    private void InitializePool(List<ProductSpawnConfig> spawnConfigs)
    {
        foreach (var config in spawnConfigs)
        {
            if (config.productPrefab == null)
            {
                Debug.LogWarning("A product prefab is missing in the spawn configuration.");
                continue;
            }

            for (int i = 0; i < config.spawnCount; i++)
            {
                var product = Instantiate(config.productPrefab);
                product.SetActive(false);
                productPool.Enqueue(product);
            }
        }
    }

    private void SpawnProducts()
    {
        foreach (var config in currentLevelConfig.productSpawnConfigs)
        {
            for (int i = 0; i < config.spawnCount; i++)
            {
                if (productPool.Count > 0)
                {
                    var product = productPool.Dequeue();
                    product.transform.position = GetRandomSpawnPosition();
                    product.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Not enough products in the pool!");
                }
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var center = currentLevelConfig.spawnAreaCenter;
        var size = currentLevelConfig.spawnAreaSize;

        return center + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );
    }
}
