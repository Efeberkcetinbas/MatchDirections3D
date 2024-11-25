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
    private HashSet<Vector3Int> _occupiedCells = new HashSet<Vector3Int>();

    public void StartPlacingBrick(Brick brick, ObjectData objectData)
    {
        _currentBrick = brick;
        _currentObjectData = objectData;

        // Set the brick material to transparent for preview
        SetBrickMaterial(_currentBrick.gameObject, validMaterial, 0.25f);
    }

    public void UpdatePreview(Vector3 worldPosition)
    {
        if (_currentBrick == null || _currentObjectData == null) return;

        // Get the dynamically snapped position using offsets from ObjectData
        Vector3 snappedPosition = GetSnappedPosition(worldPosition);

        // Update brick's position
        _currentBrick.transform.position = snappedPosition;

        // Check if placement is valid and update material
        Vector3Int gridPosition = grid.WorldToCell(snappedPosition);
        bool isPlacementValid = CheckGridAvailability(gridPosition, _currentObjectData.Size);
        SetBrickMaterial(_currentBrick.gameObject, isPlacementValid ? validMaterial : invalidMaterial, 0.25f);
    }

    public void FinalizePlacement()
    {
        if (_currentBrick == null || _currentObjectData == null) return;

        // Get the snapped position for final placement
        Vector3 snappedPosition = GetSnappedPosition(_currentBrick.transform.position);
        Vector3Int gridPosition = grid.WorldToCell(snappedPosition);

        if (CheckGridAvailability(gridPosition, _currentObjectData.Size))
        {
            // Lock the brick in place
            MarkGridCells(gridPosition, _currentObjectData.Size);
            _currentBrick.transform.position = snappedPosition;
            _currentBrick.transform.rotation = Quaternion.identity;

            // Reset material to fully opaque
            SetBrickMaterial(_currentBrick.gameObject, validMaterial, 1f);

            Debug.Log("Brick placed successfully!");
        }
        else
        {
            Debug.Log("Invalid placement!");
        }

        _currentBrick = null;
        _currentObjectData = null;
    }

    private Vector3 GetSnappedPosition(Vector3 worldPosition)
    {
        // Convert the world position to the nearest grid cell
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);

        // Calculate the snapped position
        Vector3 snappedPosition = grid.CellToWorld(gridPosition);

        // Apply the custom offsets from ObjectData
        snappedPosition.x += _currentObjectData.XOffset;
        snappedPosition.z += _currentObjectData.ZOffset;

        // Set Y to 0 (or keep the current height if needed)
        snappedPosition.y = 0;

        return snappedPosition;
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
    

