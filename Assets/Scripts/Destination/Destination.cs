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
    
    private Vector3 initialScale;
    private void Awake()
    {
        ResetText();
    }

    internal void ResetDestination()
    {
        ResetText();
        meshRenderer.material=null;
        meshFilter.mesh=null;
        transform.localScale=initialScale;
    }


    private void ResetText()
    {
        CounterText.SetText("0/0");
        initialScale=transform.localScale;
    }
    
}
