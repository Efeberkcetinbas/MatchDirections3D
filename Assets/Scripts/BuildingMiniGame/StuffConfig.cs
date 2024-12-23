using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStuff", menuName = "Building/StuffConfig")]
public class StuffConfig : ScriptableObject
{
    public string stuffName;
    public int requiredGameObjects;
    public Sprite stuffIcon; // Optional: for UI representation
}