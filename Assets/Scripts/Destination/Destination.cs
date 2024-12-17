using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Destination : MonoBehaviour
{
    public TextMeshPro CounterText;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Transform ProductEnter;

    private void Awake()
    {
        CounterText.SetText("0/0");
    }
    
}
