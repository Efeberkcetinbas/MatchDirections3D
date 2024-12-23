using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelperData", menuName = "HelperData/Helper", order = 0)]
public class HelperConfig : ScriptableObject
{   
    public Sprite HelperSprite;
    public int RequirementScore;
    public int Amount;
    public int GivenAmount;
    
    
}
