using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera cam; // Camera to use for raycasting
    [SerializeField] private LayerMask placementLayerMask; // Layer mask for raycasting
    private Vector3 lastPosition; // Store the last valid position

    public Vector3 GetSelectedMapPosition()
    {
        if (Input.touchCount > 0) // Check if there is at least one touch
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector3 touchPosition = touch.position;
                touchPosition.z = cam.nearClipPlane; // Set z to near clip plane distance
                Ray ray = cam.ScreenPointToRay(touchPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
                {
                    lastPosition = hit.point; // Update the last valid position
                }
            }
        }

        return lastPosition; // Return the last known valid position
    }
}
