using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerAttributes playerAttributes;

    private void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
    }

    public void Initialize()
    {
        // Example: Access ColorEnumAttribute
        var colorAttribute = playerAttributes.GetAttribute<ColorEnumAttribute>();
        if (colorAttribute != null)
        {
            Debug.Log($"Player's color is {colorAttribute.value}");
        }

        // Example: Access DirectionEnumAttribute
        var directionAttribute = playerAttributes.GetAttribute<DirectionEnumAttribute>();
        if (directionAttribute != null)
        {
            Debug.Log($"Player's direction is {directionAttribute.value}");
        }
    }
}
