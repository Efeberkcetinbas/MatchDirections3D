using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DragManager : MonoBehaviour
{
    private Camera _mainCamera;
    private Vector3 _offset;
    private Plane _dragPlane;
    private bool canDrag=true;


    [SerializeField] private LayerMask productLayerMask;
    [SerializeField] private LayerMask dropAreaLayerMask;
    [SerializeField] private LayerMask vipLayerMask;
    [SerializeField] private GameData gameData;


    public ProductDrag CurrentProduct;
    public List<ProductDrag> productDrags= new List<ProductDrag>();


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(!gameData.isGameEnd && canDrag && !gameData.isUIIntheScene)
            HandleDrag();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.AddHandler(GameEvent.OnCollector,OnCollector);
        EventManager.AddHandler(GameEvent.OnCollectorEnd,OnCollectorEnd);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnCollector,OnCollector);
        EventManager.RemoveHandler(GameEvent.OnCollectorEnd,OnCollectorEnd);
    }

    private void OnCollector()
    {
        canDrag=false;
    }

    private void OnCollectorEnd()
    {
        canDrag=true;
    }

    private void OnRestartLevel()
    {
        for (int i = 0; i < productDrags.Count; i++)
        {
            productDrags[i].Reset();
            productDrags[i].IsPlaced=false;
            productDrags[i].IsCollected=false;
            productDrags[i].GetComponent<ProductTrigger>().OnRestart();
        }
    }

    private void OnGameStart()
    {
        productDrags.Clear();
        canDrag=true;
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
                    if (CurrentProduct != null && !CurrentProduct.IsPlaced && !CurrentProduct.IsCollected)
                    {
                        CurrentProduct.IsBeingDragged = true;
                        CurrentProduct.Collider.isTrigger=true;
                        CurrentProduct.rb.useGravity=false;
                        CurrentProduct.rb.isKinematic=true;
                        //CurrentProduct.transform.position.y+1
                        CurrentProduct.transform.DOMoveY(6,0.5f);


                        CurrentProduct.OutlineMesh.enabled=true;
                        CurrentProduct.OutlineMesh.OpenOutline();
                        CurrentProduct.OutlineMesh.OutlineMode = Outline.Mode.OutlineAll;
                        CurrentProduct.OutlineMesh.OutlineColor = Color.yellow;
                        CurrentProduct.OutlineMesh.OutlineWidth = 5f;
                        CurrentProduct.OutlineMesh.UpdateMaterialProperties();

                        EventManager.Broadcast(GameEvent.OnProductDragStart);
                        
                        
                        _dragPlane = new Plane(Vector3.up, CurrentProduct.transform.position);
                        _dragPlane.Raycast(ray, out float enter);
                        _offset = CurrentProduct.transform.position - ray.GetPoint(enter);

                        Debug.Log("Started Drag");
                    }
                }

                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, vipLayerMask))
                {
                    if(gameData.isVipHere)
                    {
                        Debug.Log("HIT VIP");
                        gameData.isVipHere=false;
                        EventManager.Broadcast(GameEvent.OnVipProductTouched);
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

                else
                {
                    CurrentProduct.Reset();
                    CurrentProduct=null;
                    EventManager.Broadcast(GameEvent.OnProductReset);
                    return;
                }

                

                CurrentProduct.OutlineMesh.RemoveOutline();
                CurrentProduct.OutlineMesh.enabled=false;
                CurrentProduct.IsBeingDragged = false;
                CurrentProduct = null;

                EventManager.Broadcast(GameEvent.OnProductDragStop);
            }
        }
    }
}
