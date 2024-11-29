using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributeInitializer : MonoBehaviour
{
    [SerializeField]
    private EnumAttribute[] attributes; // Assign in the player prefab inspector

    private PlayerAttributes playerAttributes;

    private void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();

        if (playerAttributes == null)
        {
            Debug.LogError("PlayerAttributes component is missing!");
            return;
        }

        // Assign attributes to the PlayerAttributes component
        foreach (var attribute in attributes)
        {
            if (attribute != null)
            {
                playerAttributes.AddAttribute(attribute);
            }
        }
    }
}
