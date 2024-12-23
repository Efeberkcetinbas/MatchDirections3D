using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private StuffManager[] stuffManagers; // All stuff in the building
    public bool IsCompleted { get; private set; } = false;

    private void Start()
    {
        CheckStuffs();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUpdateBuilding, OnUpdateBuilding);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUpdateBuilding, OnUpdateBuilding);
    }

    private void OnUpdateBuilding()
    {
        CheckStuffs();
    }

    private void CheckStuffs()
    {
        foreach (var stuffManager in stuffManagers)
        {
            stuffManager.gameObject.SetActive(stuffManager.IsUnlocked); // Activate only unlocked stuff
        }
    }

    public void AddGameObjects(GameObject[] emojis, float delayInterval)
    {
        if (IsCompleted) return;

        foreach (var stuffManager in stuffManagers)
        {
            if (!stuffManager.IsUnlocked)
            {
                stuffManager.AddGameObjects(emojis, delayInterval);
                if (AllStuffUnlocked())
                {
                    CompleteBuilding();
                }
                return;
            }
        }
    }

    private bool AllStuffUnlocked()
    {
        foreach (var stuffManager in stuffManagers)
        {
            if (!stuffManager.IsUnlocked) return false;
        }
        return true;
    }

    private void CompleteBuilding()
    {
        IsCompleted = true;
        Debug.Log("Building completed!");
    }
}
