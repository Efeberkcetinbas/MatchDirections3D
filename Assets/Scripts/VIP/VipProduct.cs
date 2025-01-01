using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipProduct : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;


    private Rigidbody rb;
    private BoxCollider boxCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    internal void Reset()
    {
        boxCollider.enabled=false;
        rb.useGravity=false;
        rb.isKinematic=true;
    }
}
