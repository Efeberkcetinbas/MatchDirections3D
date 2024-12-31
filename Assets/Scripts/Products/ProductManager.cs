using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    /*[SerializeField] private GameObject[] gameObjects; // List of game objects in the scene
    [SerializeField] private ProductAttributeConfig[] objectConfigurations; // Configuration for each game object

    private void Start()
    {
        AssignAttributes();
    }

    private void AssignAttributes()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (i < objectConfigurations.Length)
            {
                var objectAttributes = gameObjects[i].GetComponent<ProductAttributes>();
                var configuration = objectConfigurations[i];

                foreach (var attribute in configuration.attributes)
                {
                    objectAttributes.AddAttribute(attribute);
                    Debug.Log((attribute) + "" + objectAttributes + " / " +  attribute.name);
                }
            }
            else
            {
                Debug.LogWarning($"No configuration assigned for GameObject {gameObjects[i].name}");
            }
        }
    }*/

    
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSetSameProductsUp, OnSetSameProductsUp);
        EventManager.AddHandler(GameEvent.OnVipProductCreated,OnVipProductCreated);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSetSameProductsUp, OnSetSameProductsUp);
        EventManager.RemoveHandler(GameEvent.OnVipProductCreated,OnVipProductCreated);
    }
    

    private void OnVipProductCreated()
    {
        // Find all active ProductAttributes in the scene
        var products = FindObjectsOfType<ProductAttributes>();
        foreach (var product in products)
        {
            product.transform.position += new Vector3(0, 5, 0);
        }
    }
    
    //Events Calling
    private void OnSetSameProductsUp()
    {
        // Find all active ProductAttributes in the scene
        var products = FindObjectsOfType<ProductAttributes>();
        if (products.Length == 0)
        {
            Debug.LogWarning("No active products found in the scene.");
            return;
        }

        // Collect all unique sets of (type, value) pairs from active products
        var uniqueAttributeSets = new List<List<(Type type, object value)>>();
        foreach (var product in products)
        {
            var attributeSet = new List<(Type type, object value)>();

            foreach (var type in product.GetTypes())
            {
                var attribute = product.GetAttributeByType(type);
                if (attribute != null)
                {
                    var value = GetEnumValue(attribute);
                    if (value != null)
                    {
                        // Add (type, value) pair to the attribute set for this product
                        attributeSet.Add((type, value));
                    }
                }
            }

            if (attributeSet.Count > 0 && !uniqueAttributeSets.Contains(attributeSet))
            {
                uniqueAttributeSets.Add(attributeSet); // Store unique attribute sets
            }
        }

        // If no unique attribute sets are found, log a warning and exit
        if (uniqueAttributeSets.Count == 0)
        {
            Debug.LogWarning("No attributes with values found in active products.");
            return;
        }

        // Randomly select one attribute set (this set contains type-value pairs)
        var selectedAttributeSet = uniqueAttributeSets[UnityEngine.Random.Range(0, uniqueAttributeSets.Count)];
        Debug.Log($"Selected Attribute Set: {string.Join(", ", selectedAttributeSet)}");

        // Move products that have the exact same set of attributes and values
        bool anyMoved = false;
        foreach (var product in products)
        {
            bool matchesAllAttributes = true;

            // Check if all attributes match the selected set of attributes
            foreach (var (selectedType, selectedValue) in selectedAttributeSet)
            {
                var matchingAttribute = product.GetAttributeByType(selectedType);
                if (matchingAttribute == null || !GetEnumValue(matchingAttribute)?.Equals(selectedValue) == true)
                {
                    matchesAllAttributes = false;
                    break;
                }
            }

            // If all attributes match, move the product
            if (matchesAllAttributes)
            {
                product.transform.position += new Vector3(0, 5, 0);
                Debug.Log($"Moved product: {product.name} with Attributes: {string.Join(", ", selectedAttributeSet)}");
                anyMoved = true;
            }
        }

        // If no products were moved, log a warning
        if (!anyMoved)
        {
            Debug.LogWarning("No matching products found to move.");
        }
    }

    private object GetEnumValue(EnumAttribute attribute)
    {
        // Use reflection to retrieve the 'value' field from the EnumAttribute
        var valueField = attribute.GetType().GetField("value");
        if (valueField != null)
        {
            return valueField.GetValue(attribute);
        }
        else
        {
            Debug.LogError($"Attribute {attribute.GetType().Name} does not have a 'value' field.");
            return null;
        }
    }

}
