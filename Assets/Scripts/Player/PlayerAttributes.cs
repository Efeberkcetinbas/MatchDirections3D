using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    private Dictionary<Type, EnumAttribute> attributes = new();

    // Add an attribute to the player
    public void AddAttribute(EnumAttribute attribute)
    {
        var type = attribute.GetType();
    
        // Clean any existing attributes of the same type
        if (attributes.ContainsKey(type))
        {
            attributes.Remove(type);
            Debug.Log($"Removed existing attribute of type {type} before adding the new one.");
        }
        
        // Add the new attribute
        attributes[type] = attribute;
        Debug.Log($"Added attribute of type {type}.");

    }

    internal void CleanAttributes()
    {
        attributes.Clear();
    }

    

    // Get an attribute by its type
    public EnumAttribute GetAttributeByType(Type type)
    {
        attributes.TryGetValue(type, out var attribute);
        return attribute;
    }

    // Get all types of attributes this player has
    public IEnumerable<Type> GetTypes()
    {
        return attributes.Keys;
    }

    
}