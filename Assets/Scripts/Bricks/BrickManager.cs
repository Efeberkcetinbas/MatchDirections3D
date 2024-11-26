using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementManager; // Reference to PlacementManager
    private Grid _grid;
    private Brick _currentBrick; // Currently active brick
    [SerializeField] private ObjectDatabase objectDatabase; // Reference to the ObjectDatabase

     public void Initialize(Grid grid)
    {
        _grid = grid;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleTouchStart(touch);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (_currentBrick != null)
                        HandleDragging(touch);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (_currentBrick != null)
                        HandleTouchEnd();
                    break;
            }
        }
    }

    private void HandleTouchStart(Touch touch)
    {
        // Raycast to detect a brick under the touch
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Brick brick = hit.collider.GetComponent<Brick>();
            if (brick != null)
            {
                _currentBrick = brick;
                _currentBrick.OnTouched();

                // Notify PlacementManager to start placing
                ObjectData objectData = GetObjectDataForBrick(brick); // Fetch ObjectData
                placementManager.StartPlacingBrick(brick, objectData);
            }
        }
    }

    private void HandleDragging(Touch touch)
    {
        // Raycast to update position during drag
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Notify PlacementManager to update the preview position
            placementManager.UpdatePreview(hit.point);
        }
    }

    private void HandleTouchEnd()
    {
        // Notify PlacementManager to finalize placement
        placementManager.FinalizePlacement();

        _currentBrick.OnReleased();
        _currentBrick = null; // Clear active brick
    }

    private ObjectData GetObjectDataForBrick(Brick brick)
    {
        // Check if the brick's prefab matches one in the ObjectDatabase
        foreach (ObjectData objectData in objectDatabase.objectsData)
        {
            if (objectData.Prefab == brick.PrefabReference) // Compare prefab references
            {
                return objectData;
            }
        }

        return null;
    }
}
