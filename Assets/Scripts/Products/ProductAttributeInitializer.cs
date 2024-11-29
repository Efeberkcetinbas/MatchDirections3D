using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductAttributeInitializer : MonoBehaviour
{
    [SerializeField]
    private EnumAttribute[] attributes; // Assign in the prefab inspector

    private ProductAttributes productAttributes;

    private void Awake()
    {
        productAttributes = GetComponent<ProductAttributes>();

        if (productAttributes == null)
        {
            Debug.LogError("ProductAttributes component is missing!");
            return;
        }

        // Assign attributes to the ProductAttributes component
        foreach (var attribute in attributes)
        {
            if (attribute != null)
            {
                productAttributes.AddAttribute(attribute);
            }
        }
    }
}
