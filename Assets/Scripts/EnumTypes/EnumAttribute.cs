using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnumAttribute : ScriptableObject
{
    public abstract string DisplayName { get; } // Optional: For debugging or UI
    
}