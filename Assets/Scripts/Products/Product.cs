using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    private ProductAttributes attributes;

    private void Awake()
    {
        attributes = GetComponent<ProductAttributes>();
    }

    private void Initialize()
    {
        // Example: Access ColorEnumAttribute
        var colorAttribute = attributes.GetAttribute<ColorEnumAttribute>();
        if (colorAttribute != null)
        {
            Debug.Log($"GameObject's color is {colorAttribute.value}");
        }

        // Example: Access DirectionEnumAttribute
        var directionAttribute = attributes.GetAttribute<DirectionEnumAttribute>();
        if (directionAttribute != null)
        {
            Debug.Log($"GameObject's direction is {directionAttribute.value}");
        }
    }
}
