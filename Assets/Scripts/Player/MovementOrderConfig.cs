using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMovementOrderConfig", menuName = "Game/MovementOrderConfig")]
public class MovementOrderConfig : ScriptableObject
{
    public List<int> movementOrder; // Order of players to move (e.g., 1-1-2-1)
    public PlayerAttributeConfig[] playerAttributes; // Attributes for each player
}
