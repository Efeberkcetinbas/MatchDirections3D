using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;

    private Brick _currentBrick;
    private ObjectData _currentObjectData;
    private Vector3 _precomputedOffset; // Precomputed offset for current brick
    private HashSet<Vector3Int> _occupiedCells = new HashSet<Vector3Int>();

    public void StartPlacingBrick(Brick brick, ObjectData objectData)
    {
        _currentBrick = brick;
        _currentObjectData = objectData;

        // Precompute the offset for smoother snapping
        _precomputedOffset = new Vector3(objectData.XOffset, 0, objectData.ZOffset);

        // Set the brick material to transparent for preview
        SetBrickMaterial(_currentBrick.gameObject, validMaterial, 0.25f);
    }

    public void UpdatePreview(Vector3 worldPosition)
    {
        if (_currentBrick == null || _currentObjectData == null) return;

        // Step 1: Convert touch position to the nearest grid center
        Vector3 snappedPosition = GetSnappedPositionToGridCenter(worldPosition);

        // Step 2: Apply the precomputed offset to simulate proper positioning during drag
        snappedPosition += _precomputedOffset;

        // Step 3: Smoothly move the brick to the snapped position
        _currentBrick.transform.position = snappedPosition;

        // Step 4: Validate placement and update materials
        Vector3Int gridPosition = grid.WorldToCell(snappedPosition);
        bool isPlacementValid = CheckGridAvailability(gridPosition, _currentObjectData.Size);
        SetBrickMaterial(_currentBrick.gameObject, isPlacementValid ? validMaterial : invalidMaterial, 0.25f);
    }

    public void FinalizePlacement()
    {
        if (_currentBrick == null || _currentObjectData == null) return;

        // Step 1: Snap to the grid center
        Vector3 snappedPosition = GetSnappedPositionToGridCenter(_currentBrick.transform.position);

        // Step 2: Subtract the precomputed offset to match final position
        snappedPosition -= _precomputedOffset; // Subtract offset to align with grid

        // Step 3: Lock the brick to the final snapped position
        Vector3Int gridPosition = grid.WorldToCell(snappedPosition);

        if (CheckGridAvailability(gridPosition, _currentObjectData.Size))
        {
            // Step 4: Lock the brick in place and finalize the placement
            _currentBrick.transform.position = snappedPosition;

            // Reset material to fully opaque
            SetBrickMaterial(_currentBrick.gameObject, validMaterial, 1f);

            // Mark cells as occupied
            MarkGridCells(gridPosition, _currentObjectData.Size);

            Debug.Log("Brick placed successfully!");
        }
        else
        {
            Debug.Log("Invalid placement!");
        }

        // Reset states
        _currentBrick = null;
        _currentObjectData = null;
    }

    private Vector3 GetSnappedPositionToGridCenter(Vector3 worldPosition)
    {
        // Convert the world position to the nearest grid cell
        Vector3Int gridCell = grid.WorldToCell(worldPosition);

        // Calculate the center of the cell
        Vector3 gridCenter = grid.CellToWorld(gridCell);
        gridCenter.x += grid.cellSize.x / 2f; // Center X
        gridCenter.z += grid.cellSize.z / 2f; // Center Z
        gridCenter.y = 0; // Keep height consistent

        return gridCenter;
    }

    private bool CheckGridAvailability(Vector3Int gridPosition, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cellPosition = gridPosition + new Vector3Int(x, y, 0);
                if (_occupiedCells.Contains(cellPosition)) return false;
            }
        }
        return true;
    }

    private void MarkGridCells(Vector3Int gridPosition, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int cellPosition = gridPosition + new Vector3Int(x, y, 0);
                _occupiedCells.Add(cellPosition);
            }
        }
    }

    private void SetBrickMaterial(GameObject brick, Material material, float alpha)
    {
        foreach (Renderer renderer in brick.GetComponentsInChildren<Renderer>())
        {
            Material newMaterial = new Material(material);
            Color color = newMaterial.color;
            color.a = alpha;
            newMaterial.color = color;
            renderer.material = newMaterial;
        }
    }
}
    

