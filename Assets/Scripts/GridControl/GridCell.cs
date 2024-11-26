using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector3Int cellPosition; // The position of the grid cell in grid coordinates
    public bool isOccupied = false; // Whether the cell is occupied

    private bool canCheck=false;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlacement,OnPlacement);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlacement,OnPlacement);
    }

    private void OnPlacement()
    {
        canCheck=true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(canCheck)
        {
            if (other.CompareTag("Brick")) // Only interact with bricks
            {
                isOccupied = true; // Mark the cell as occupied when a brick enters
                Debug.Log("HERE");
                canCheck=false;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            isOccupied = false; // Mark the cell as unoccupied when the brick exits
        }
    }
}
