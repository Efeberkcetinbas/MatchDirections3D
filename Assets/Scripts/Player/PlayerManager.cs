using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player[] players; // List of players in the scene
    [SerializeField] private PlayerAttributeConfig[] levelConfigurations; // Configuration for each level

    private void Start()
    {
        AssignAttributes();
    }

    private void AssignAttributes()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i < levelConfigurations.Length)
            {
                var playerAttributes = players[i].GetComponent<PlayerAttributes>();
                var configuration = levelConfigurations[i];

                foreach (var attribute in configuration.attributes)
                {
                    playerAttributes.AddAttribute(attribute);
                    Debug.Log((attribute) + "" + playerAttributes + " / " +  attribute.name);
                }
            }
            else
            {
                Debug.LogWarning($"No configuration assigned for Player {i + 1}");
            }
        }
    }
}
