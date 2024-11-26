using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCheck : MonoBehaviour
{
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private Vector3 gridSize = new Vector3(10, 0, 10); // Grid size (adjust accordingly)
    [SerializeField] private float cellSize = 1f; // Size of each grid cell (adjust according to your grid)

    private Dictionary<Vector3Int, GridCell> gridCells = new Dictionary<Vector3Int, GridCell>();

    private void Start()
    {
        GenerateGridCells();
    }

    private void GenerateGridCells()
    {
        Vector3 gridCenter = transform.position;

        // Generate grid cells (e.g., for a 10x10 grid)
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.z; z++)
            {
                Vector3 position = new Vector3(gridCenter.x + x * cellSize, gridCenter.y, gridCenter.z + z * cellSize);
                GameObject gridCellObject = Instantiate(gridCellPrefab, position, Quaternion.identity);
                gridCellObject.transform.SetParent(transform); // Make grid cells children of the GridCheck object
                GridCell gridCell = gridCellObject.GetComponent<GridCell>();

                // Set the correct grid position
                gridCell.cellPosition = new Vector3Int(x, 0, z);

                // Add the grid cell to the dictionary
                gridCells[gridCell.cellPosition] = gridCell;
            }
        }
    }

    // Method to check if a grid cell is occupied
    public bool IsCellOccupied(Vector3Int gridPosition)
    {
        if (gridCells.ContainsKey(gridPosition))
        {
            return gridCells[gridPosition].isOccupied;
        }

        return false; // Cell does not exist
    }

    // Mark a cell as occupied (used by PlacementSystem)
    public void MarkCellAsOccupied(Vector3Int gridPosition)
    {
        if (gridCells.ContainsKey(gridPosition))
        {
            gridCells[gridPosition].isOccupied = true;
        }
    }

    // Optionally add methods to mark cells as unoccupied if necessary
    public void MarkCellAsUnoccupied(Vector3Int gridPosition)
    {
        if (gridCells.ContainsKey(gridPosition))
        {
            gridCells[gridPosition].isOccupied = false;
        }
    }

   
}
