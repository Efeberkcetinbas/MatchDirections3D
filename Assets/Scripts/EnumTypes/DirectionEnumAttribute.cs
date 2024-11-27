using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectionAttribute", menuName = "Game/Attributes/Direction")]
public class DirectionEnumAttribute : EnumAttribute
{
    public DirectionEnum value;

    public override string DisplayName => $"Direction: {value}";
}
