using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBuildingManager : MonoBehaviour
{
    [SerializeField] private BuildingManager[] buildingManagers;
    [SerializeField] private List<GameObject> products = new List<GameObject>();
    private int currentBuildingIndex = 0;

    private const string PlayerPrefsKey = "GeneralBuilding_CurrentIndex";

    

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUpdateMiniMap, OnUpdateMiniMap);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUpdateMiniMap, OnUpdateMiniMap);
    }

    private void OnUpdateMiniMap()
    {
        LoadProgress();
        ActivateCurrentBuilding();
    }

    //Reset when new level or restart
    public void ResetProducts()
    {
        products.Clear();
    }
    

    

    public void AddProduct(GameObject product)
    {
        product.SetActive(true);
        products.Add(product);
    }

    public void TransferProductsToBuildings()
    {
        if (currentBuildingIndex >= buildingManagers.Length)
        {
            Debug.LogWarning("No more buildings to unlock.");
            return;
        }

        int activeProductCount = products.FindAll(e => e.activeSelf).Count;

        if (activeProductCount > 0)
        {
            buildingManagers[currentBuildingIndex].AddGameObjects(
                products.ToArray(),
                0.3f // Delay interval for animation
            );

            if (buildingManagers[currentBuildingIndex].IsCompleted)
            {
                currentBuildingIndex++;
                SaveProgress();
                ActivateCurrentBuilding();
            }
        }
        else
        {
            Debug.Log("No active emojis available to transfer.");
        }
    }

    private void ActivateCurrentBuilding()
    {
        for (int i = 0; i < buildingManagers.Length; i++)
        {
            buildingManagers[i].gameObject.SetActive(i == currentBuildingIndex);
        }

        if (currentBuildingIndex < buildingManagers.Length)
        {
            Debug.Log($"Activated: {buildingManagers[currentBuildingIndex].gameObject.name}");
        }

        EventManager.Broadcast(GameEvent.OnUpdateBuilding);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, currentBuildingIndex);
    }

    private void LoadProgress()
    {
        currentBuildingIndex = PlayerPrefs.GetInt(PlayerPrefsKey, 0);
    }
}
