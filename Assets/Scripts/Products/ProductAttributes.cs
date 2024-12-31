using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductAttributes : MonoBehaviour
{
    private Dictionary<Type, EnumAttribute> attributes = new();

    private ProductDrag productDrag;

    private void Start()
    {
        productDrag=GetComponent<ProductDrag>();
    }

    internal void SetProductCollected()
    {
        productDrag.IsCollected=true;
    }

    // Add an attribute to the product
    public void AddAttribute(EnumAttribute attribute)
    {
        var type = attribute.GetType();
        if (!attributes.ContainsKey(type))
        {
            attributes[type] = attribute;
        }
        else
        {
            Debug.LogWarning($"Attribute of type {type} already exists!");
        }
    }

    // Get an attribute of a specific type
    public T GetAttribute<T>() where T : EnumAttribute
    {
        var type = typeof(T);
        return attributes.ContainsKey(type) ? (T)attributes[type] : null;
    }

    // Get an attribute by its type
    public EnumAttribute GetAttributeByType(Type type)
    {
        attributes.TryGetValue(type, out var attribute);
        return attribute;
    }

    // Get all types of attributes this product has
    public IEnumerable<Type> GetTypes()
    {
        return attributes.Keys;
    }

    // Check if a specific type of attribute exists
    public bool HasAttribute<T>() where T : EnumAttribute
    {
        return attributes.ContainsKey(typeof(T));
    }
}
