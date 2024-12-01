using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Game/LevelConfig")]
public class LevelConfiguration:ScriptableObject
{
    public string levelName;
    public List<ProductSpawnConfig> productSpawnConfigs; // List of specific spawn configurations
    public Vector3 spawnAreaCenter;                     // Center of the spawn area
    public Vector3 spawnAreaSize;                       // Size of the spawn area
}

[System.Serializable]
public class ProductSpawnConfig
{
    public GameObject productPrefab; // The prefab to spawn
    public int spawnCount;           // Number of instances to spawn
}
