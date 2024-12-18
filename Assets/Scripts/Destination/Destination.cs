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
        ResetText();
    }

    internal void ResetDestination()
    {
        ResetText();
        meshRenderer.material=null;
        meshFilter.mesh=null;
    }


    private void ResetText()
    {
        CounterText.SetText("0/0");
    }
    
}
