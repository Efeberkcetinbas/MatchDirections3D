using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Product>())
        {
            Debug.Log("SUMMOSEN");
            other.GetComponent<ProductDrag>().Reset();
        }
    }
}
