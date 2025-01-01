using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class VipManager : MonoBehaviour
{
    [Header("VIP Configuration")]
    [SerializeField] private GameObject vipPool; // Pool of VIPs in the scene
    [SerializeField] private GameObject[] vipMeshPool; // Pool of VIPs in the scene
    [SerializeField] private Transform vipDestination; // Destination point for the VIP
    [SerializeField] private Transform startDestination; // Start point for the VIP
    [SerializeField] private float movementDuration = 2f; // Time for the VIP to reach the destination
    [SerializeField] private Ease ease;
    [SerializeField] private Ease productEase;

    [Header("Product Configuration")]
    [SerializeField] private GameObject[] productList; // List of possible products
    [SerializeField] private Transform productSpawnPosition; // Position where the product is spawned
    [SerializeField] private float interactionTimeLimit = 10f; // Time limit for player interaction
    [SerializeField] private float productMoveDuration = 1f; // Duration to move the product to the preview position

    [Header("Money")]
    [SerializeField] private CollectCoin collectCoin;
    
    
    [Header("Game Data")]
    [SerializeField] private GameData gameData;

    [Header("Particles VIP")]
    [SerializeField] private ParticleSystem vipLeaveParticle;

    

    
    

    private GameObject activeVIP;
    private GameObject activeProduct;
    private bool playerInteracted = false;
    private bool isHitProduct=false;

    private int tempIncreaseScore;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnVipProductTouched, OnVipProductTouched);
        EventManager.AddHandler(GameEvent.OnVipOnTheWay, OnVipOnTheWay);
        
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnVipProductTouched, OnVipProductTouched);
        EventManager.RemoveHandler(GameEvent.OnVipOnTheWay, OnVipOnTheWay);
    }

    

    /// <summary>
    /// Summons a VIP and assigns a product.
    /// </summary>
    public void OnVipOnTheWay()
    {
        if (activeVIP != null)
        {
            Debug.LogWarning("A VIP is already active!");
            return;
        }
        GetRandomVIP();
        activeVIP = vipPool;
        activeVIP.GetComponent<VipPlayer>().meshRenderer.enabled=false;
        
        EventManager.Broadcast(GameEvent.OnVipSummoned);
        MoveVIPToDestination();
    }

    /// <summary>
    /// Handles VIP success event when the product is found.
    /// </summary>
    private void OnVipProductTouched()
    {
        if (activeProduct == null) return;

        //playerInteracted = true;
        isHitProduct=true;
        activeProduct.GetComponent<VipProduct>().Reset();
        activeProduct.transform.DOPunchScale(Vector3.one,0.25f);
        // Move product to the VIP preview position
        MoveProductToPreview();
    }

    private void GetRandomVIP()
    {
        int randomIndex = Random.Range(0, vipMeshPool.Length);
        vipPool.SetActive(true);
        for (int i = 0; i < vipMeshPool.Length; i++)
        {
            vipMeshPool[i].gameObject.SetActive(false);    
        }
        vipMeshPool[randomIndex].gameObject.SetActive(true);
    }

    private GameObject InstantiateRandomProduct()
    {
        int randomIndex = Random.Range(0, productList.Length);
        return Instantiate(productList[randomIndex], productSpawnPosition.position, Quaternion.identity);
    }

    private void SetVIPPreview(GameObject product)
    {
        activeVIP.GetComponent<VipPlayer>().meshRenderer.enabled=true;
        var vipPreview = activeVIP.GetComponent<VipPlayer>().meshFilter;
        var vipRenderer = activeVIP.GetComponent<VipPlayer>().meshRenderer;

        if (vipPreview == null || vipRenderer == null)
        {
            Debug.LogError("VIP is missing a MeshFilter or MeshRenderer in its child object!");
            return;
        }

        var productMeshFilter = product.GetComponent<VipProduct>().meshFilter;
        var productMeshRenderer = product.GetComponent<VipProduct>().meshRenderer;

        if (productMeshFilter != null && productMeshRenderer != null)
        {
            vipPreview.mesh = productMeshFilter.mesh;
            vipRenderer.materials = productMeshRenderer.materials;
        }
        else
        {
            Debug.LogError("Product is missing a MeshFilter or MeshRenderer!");
        }
    }

    private void MoveVIPToDestination()
    {
        activeVIP.transform.DORotate(Vector3.zero,0.1f);
        activeVIP.transform.DOMove(vipDestination.position, movementDuration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                activeProduct = InstantiateRandomProduct();
                activeVIP.transform.DOPunchScale(Vector3.one,0.25f);
                EventManager.Broadcast(GameEvent.OnVipProductCreated);
                SetVIPPreview(activeProduct);
                gameData.isVipHere = true;
                StartCoroutine(StartInteractionTimer());
            });
    }

    private IEnumerator StartInteractionTimer()
    {
        Debug.LogWarning("CALLED");
        float elapsedTime = 0f;

        while (elapsedTime < interactionTimeLimit)
        {
            if (playerInteracted)
            {
                HandleVIPSuccess();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HandleVIPFailure();
    }

    private void MoveProductToPreview()
    {
        var previewPosition = activeVIP.GetComponent<VipPlayer>().meshRenderer.transform.position;
        
        activeVIP.GetComponent<VipPlayer>().meshRenderer.enabled=false;
        //transform.DORotate(player.GetComponent<PlayerTrigger>().ProductEnter.rotation.eulerAngles,.25f)
        activeProduct.transform.DORotate(Vector3.zero,0.25f);
        activeProduct.transform.DOJump(previewPosition,1,1, productMoveDuration).SetEase(productEase).OnComplete(() =>
        {
            //Particle, Sound
            //ResetVIP();
            EventManager.Broadcast(GameEvent.OnVipProductPlaced);
            
            activeVIP.transform.DOPunchScale(Vector3.one,0.2f);
            activeProduct.transform.DOScale(new Vector3(0.1f,0.1f,0.1f),0.25f).OnComplete(()=>activeProduct.SetActive(false));
            activeProduct.SetActive(false);
            playerInteracted = true;
            Debug.Log("Product moved to preview position.");
        });
    }

    private void HandleVIPSuccess()
    {
        Debug.Log("VIP request fulfilled successfully!");
        tempIncreaseScore=gameData.increaseScore;
        gameData.increaseScore=50;
        collectCoin.StartCollectCoin(50);
        EventManager.Broadcast(GameEvent.OnVipSuccess);
        activeVIP.transform.DORotate(new Vector3(0,180,0),1f).OnComplete(()=>{
            gameData.increaseScore=tempIncreaseScore;
            EventManager.Broadcast(GameEvent.OnVipLeave);
            vipLeaveParticle.gameObject.SetActive(true);
            vipLeaveParticle.Play();
            activeVIP.transform.DOMove(startDestination.position, movementDuration)
            .SetEase(ease)
            .OnComplete(()=>{
                ResetVIP();
            });
        });
    }

    private void HandleVIPFailure()
    {
        if(!isHitProduct)
        {
            EventManager.Broadcast(GameEvent.OnVipFail);
            activeVIP.transform.DORotate(new Vector3(0,180,0),1f).OnComplete(()=>{
                gameData.increaseScore=tempIncreaseScore;
                EventManager.Broadcast(GameEvent.OnVipLeave);
                vipLeaveParticle.gameObject.SetActive(true);
                vipLeaveParticle.Play();
                activeVIP.transform.DOMove(startDestination.position, movementDuration)
                .SetEase(ease)
                .OnComplete(()=>{
                    Debug.Log("Time's up! The VIP is leaving.");
                    gameData.isVipHere = false;
                    ResetVIP();
                });
            });
        }
        
    }

    private void ResetVIP()
    {
        if (activeProduct != null)
        {
            Destroy(activeProduct);
        }

        if (activeVIP != null)
        {
            activeVIP.SetActive(false);
        }

        vipLeaveParticle.Stop();
        vipLeaveParticle.gameObject.SetActive(false);

        activeVIP = null;
        activeProduct = null;
        playerInteracted = false;
        isHitProduct=false;
    }
}
