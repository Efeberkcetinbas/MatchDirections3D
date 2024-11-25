using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject fingerIndicator,cellIndicator;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;

    private void Update()
    {
        Vector3 fingerPosition=inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition=grid.WorldToCell(fingerPosition);
        fingerIndicator.transform.position=fingerPosition;
        cellIndicator.transform.position=grid.CellToWorld(gridPosition);
    }
}
