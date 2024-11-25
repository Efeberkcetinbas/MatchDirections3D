using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera cam; // Camera for raycasting
    [SerializeField] private LayerMask placementLayerMask; // Layer mask for grid detection
    private Vector3 lastPosition; // Last valid position on the grid

    public Vector3 GetSelectedMapPosition()
    {
        if (Input.touchCount > 0) // Ensure there is at least one touch
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector3 touchPosition = touch.position;
                Ray ray = cam.ScreenPointToRay(touchPosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayerMask))
                {
                    lastPosition = hit.point;
                }
            }
        }
        return lastPosition; // Return the last valid position
    }
}
