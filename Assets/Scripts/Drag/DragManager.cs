using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragManager : MonoBehaviour
{
    private Camera _mainCamera;
    private Vector3 _offset;
    private Plane _dragPlane;

    [SerializeField] private LayerMask productLayerMask;
    [SerializeField] private LayerMask dropAreaLayerMask;


    public ProductDrag CurrentProduct;


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleDrag();
    }

    private void HandleDrag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // Start dragging
            {
                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, productLayerMask))
                {
                    CurrentProduct = hit.collider.GetComponent<ProductDrag>();
                    if (CurrentProduct != null)
                    {
                        CurrentProduct.IsBeingDragged = true;
                        CurrentProduct.Collider.isTrigger=true;
                        CurrentProduct.rb.useGravity=false;
                        CurrentProduct.rb.isKinematic=true;
                        CurrentProduct.transform.DOMoveY(CurrentProduct.transform.position.y+1,0.5f);
                        
                        
                        _dragPlane = new Plane(Vector3.up, CurrentProduct.transform.position);
                        _dragPlane.Raycast(ray, out float enter);
                        _offset = CurrentProduct.transform.position - ray.GetPoint(enter);

                        Debug.Log("Started Drag");
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && CurrentProduct != null) // Dragging
            {
                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                if (_dragPlane.Raycast(ray, out float enter))
                {
                    Vector3 targetPosition = ray.GetPoint(enter) + _offset;
                    CurrentProduct.transform.position = new Vector3(targetPosition.x, CurrentProduct.transform.position.y, targetPosition.z);

                    Debug.Log("Dragging");
                }
            }
            else if (touch.phase == TouchPhase.Ended && CurrentProduct != null) // Drop
            {
                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, dropAreaLayerMask))
                {
                    Debug.Log("Dropped area");
                    CurrentProduct.Collider.isTrigger=false;
                    CurrentProduct.rb.useGravity=true;
                    CurrentProduct.rb.isKinematic=false;

                }

                CurrentProduct.IsBeingDragged = false;
                CurrentProduct = null;
            }
        }
    }
}
