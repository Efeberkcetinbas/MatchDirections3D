using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Game/LevelConfig")]
public class LevelConfiguration:ScriptableObject
{
    public string levelName;
    public int productCount;
    //public EnumAttribute attributeType; // Attribute type for the level (e.g., ColorEnumAttribute)
    public List<GameObject> productPrefabs; // List of prefabs used in this level
}
