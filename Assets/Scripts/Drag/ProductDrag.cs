using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDrag : MonoBehaviour
{
    public bool IsBeingDragged{get;set;}
    public Collider Collider{get; set;}
    public Rigidbody rb{get; set;}



    private void Start()
    {
        Collider = GetComponent<Collider>();
        rb=GetComponent<Rigidbody>();
    }

   
    
}
