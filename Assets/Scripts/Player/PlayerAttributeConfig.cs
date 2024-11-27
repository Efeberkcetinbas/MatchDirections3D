using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "Game/Player Attribute Configuration")]
public class PlayerAttributeConfig : ScriptableObject
{
    public EnumAttribute[] attributes; // Array of dynamic attributes
}
