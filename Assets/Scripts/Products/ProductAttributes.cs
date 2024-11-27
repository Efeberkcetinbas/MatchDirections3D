using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductAttributes : MonoBehaviour
{
    private Dictionary<System.Type, EnumAttribute> attributes = new();

    // Add an attribute to the game object
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

    // Check if a specific type of attribute exists
    public bool HasAttribute<T>() where T : EnumAttribute
    {
        return attributes.ContainsKey(typeof(T));
    }
}
