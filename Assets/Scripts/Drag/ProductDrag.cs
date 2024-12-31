using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDrag : MonoBehaviour
{
    public bool IsBeingDragged{get;set;}
    public bool IsPlaced {get;set;}
    public bool IsCollected{get; set;}
    public Collider Collider{get; set;}
    public Rigidbody rb{get; set;}

    public Outline OutlineMesh;

    



    private void Start()
    {
        Collider = GetComponent<Collider>();
        rb=GetComponent<Rigidbody>();
        
    }

   

    internal void Reset()
    {
        transform.position=new Vector3(0,7,-7);
        Collider.isTrigger=false;
        rb.useGravity=true;
        rb.isKinematic=false;
        OutlineMesh.RemoveOutline();
        OutlineMesh.enabled=false;                    
        IsBeingDragged=false;
    }

    

   
    
}
