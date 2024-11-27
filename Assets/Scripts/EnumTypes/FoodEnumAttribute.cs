using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodAttribute", menuName = "Game/Attributes/Food")]
public class FoodEnumAttribute : EnumAttribute
{
    public Food value;

    public override string DisplayName => $"Food: {value}";
}
