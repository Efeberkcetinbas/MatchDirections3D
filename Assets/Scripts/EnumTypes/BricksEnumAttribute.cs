using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBricksAttribute", menuName = "Game/Attributes/Bricks")]
public class BricksEnumAttribute : EnumAttribute
{
    public Bricks value;

    public override string DisplayName => $"Brick: {value}";
}

