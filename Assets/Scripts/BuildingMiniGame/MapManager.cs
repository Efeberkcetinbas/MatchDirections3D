using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> Products=new List<GameObject>();

    [SerializeField] private GeneralBuildingManager generalBuildingManager;
    [SerializeField] private DragManager dragManager;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSuccess, OnSuccess);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSuccess, OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
    }



    public void SetProductsToTarget()
    {
        generalBuildingManager.ResetProducts();
        EventManager.Broadcast(GameEvent.OnUpdateMiniMap);
        
        for (int i = 0; i < Products.Count; i++)
        {
            generalBuildingManager.AddProduct(Products[i]);
            Products[i].SetActive(true);
        }

        generalBuildingManager.TransferProductsToBuildings();
    }

    private void OnSuccess()
    {
        for (int i = 0; i < dragManager.productDrags.Count; i++)
        {
            Products.Add(dragManager.productDrags[i].gameObject);
        }
    }

    private void OnGameStart()
    {
        Products.Clear();
    }

}
