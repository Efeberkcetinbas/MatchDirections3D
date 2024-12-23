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

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
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
                var product = Instantiate(config.productPrefab,transform);
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
            Random.Range(-5, 5),
            Random.Range(1, 4),
            Random.Range(-12, -1)
        );
    }

    private void ResetPool()
    {
        // Find all products under this manager's transform
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf && !productPool.Contains(child.gameObject))
            {
                productPool.Enqueue(child.gameObject);
            }
        }
    }

    private void OnRestartLevel()
    {
        ResetPool(); // Return all inactive products to the pool
        SpawnProducts();
    }
}
