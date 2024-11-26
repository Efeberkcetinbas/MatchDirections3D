using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Brick : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private Vector3 _initialScale;
    public GameObject PrefabReference { get; set; } // Assigned when instantiated
    public ObjectData AssociatedObjectData { get; set; }
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _initialScale = transform.localScale;
    }
    

    public void OnTouched()
    {
        _rigidbody.isKinematic = true;
        //transform.DOScale(_initialScale * 1.2f, 0.2f);

        /*Vector3 position = transform.position;
        position.y += 0.5f; // Adjust height
        transform.position = position;*/

        transform.rotation = Quaternion.identity;
    }

    public void OnReleased()
    {
        /*Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;*/

        //transform.DOScale(_initialScale, 0.2f);
        _rigidbody.isKinematic = false;
    }

    public void DragTo(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y;
        transform.position = targetPosition;
    }
}
