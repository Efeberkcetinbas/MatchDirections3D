using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    private Dictionary<Type, EnumAttribute> attributes = new();

    public Transform TargetPos;

    // Add an attribute to the player
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

        foreach (var kvp in attributes)
        {
            Debug.Log($"Type: {kvp.Key}, Attribute: {kvp.Value},Player: {transform.name} ");
        }
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