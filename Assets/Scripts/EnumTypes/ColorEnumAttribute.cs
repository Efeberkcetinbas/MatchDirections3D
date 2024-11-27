using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewColorAttribute", menuName = "Game/Attributes/Color")]
public class ColorEnumAttribute : EnumAttribute
{
    public ColorEnum value;

    public override string DisplayName => $"Color: {value}";
    
}
