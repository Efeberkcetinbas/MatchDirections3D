using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnerManager : MonoBehaviour
{
    [SerializeField]
    private List<LevelConfiguration> levelConfigs; // Drag and drop LevelConfig assets here

    [SerializeField]
    private LevelSpawner spawner; // Reference to the spawner

    private int currentLevelIndex = 0;

    private void Start()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelConfigs.Count)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        currentLevelIndex = levelIndex;
        spawner.enabled = false; // Stop the current spawner
        spawner.currentLevelConfig = levelConfigs[currentLevelIndex];
        spawner.enabled = true; // Restart with new config
    }

    private void OnRestart()
    {
        LoadLevel(currentLevelIndex);
    }
}
