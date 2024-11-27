using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects; // List of game objects in the scene
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
    }
}
