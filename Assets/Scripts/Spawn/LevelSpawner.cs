using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    
    public LevelConfiguration currentLevelConfig; // Assign via the LevelManager

    [SerializeField]
    private Transform spawnArea; // Optional: Area for spawning products

    private Queue<GameObject> productPool = new Queue<GameObject>();

    private void Start()
    {
        if (currentLevelConfig == null)
        {
            Debug.LogError("LevelConfig is not assigned!");
            return;
        }

        InitializePool(currentLevelConfig.productPrefabs, currentLevelConfig.productCount);
        SpawnProducts();
    }

    private void InitializePool(List<GameObject> prefabs, int count)
    {
        foreach (var prefab in prefabs)
        {
            for (int i = 0; i < count / prefabs.Count; i++)
            {
                var product = Instantiate(prefab);
                product.SetActive(false);
                productPool.Enqueue(product);
            }
        }
    }

    private void SpawnProducts()
    {
        for (int i = 0; i < currentLevelConfig.productCount; i++)
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

    private Vector3 GetRandomSpawnPosition()
    {
        // Replace with your spawn logic (e.g., random within a spawn area)
        return new Vector3(
            Random.Range(-2f, -3f),
            Random.Range(0f, 1f),
            Random.Range(1f, -1f)
        );
    }
}
