using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProductAttributeConfig", menuName = "Game/Product Attribute Configuration")]
public class ProductAttributeConfig : ScriptableObject
{
    public EnumAttribute[] attributes; // Array of dynamic attributes
}
