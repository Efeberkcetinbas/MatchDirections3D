using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewToysAttribute", menuName = "Game/Attributes/Toys")]
public class ToysEnumAttribute : EnumAttribute
{
    public Toys value;

    public override string DisplayName => $"Toys: {value}";
}

