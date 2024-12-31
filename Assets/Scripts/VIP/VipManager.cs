using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VipManager : MonoBehaviour
{
    [Header("VIP Configuration")]
    [SerializeField] private GameObject[] vipPool; // Pool of VIPs in the scene
    [SerializeField] private Transform vipDestination; // Destination point for the VIP
    [SerializeField] private float movementDuration = 2f; // Time for the VIP to reach the destination
    [SerializeField] private Ease ease;

    [Header("Product Configuration")]
    [SerializeField] private GameObject[] productList; // List of possible products
    [SerializeField] private Transform productSpawnPosition; // Position where the product is spawned
    [SerializeField] private float interactionTimeLimit = 10f; // Time limit for player interaction
    [SerializeField] private float productMoveDuration = 1f; // Duration to move the product to the preview position

    [Header("Game Data")]
    [SerializeField] private GameData gameData;

    private GameObject activeVIP;
    private GameObject activeProduct;
    private bool playerInteracted = false;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnVipProductTouched, OnVipProductTouched);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnVipProductTouched, OnVipProductTouched);
    }

    /// <summary>
    /// Summons a VIP and assigns a product.
    /// </summary>
    public void SummonVIP()
    {
        if (activeVIP != null)
        {
            Debug.LogWarning("A VIP is already active!");
            return;
        }

        activeVIP = GetRandomVIP();
        

        MoveVIPToDestination();
    }

    /// <summary>
    /// Handles VIP success event when the product is found.
    /// </summary>
    private void OnVipProductTouched()
    {
        if (activeProduct == null) return;

        playerInteracted = true;

        // Move product to the VIP preview position
        MoveProductToPreview();
    }

    private GameObject GetRandomVIP()
    {
        int randomIndex = Random.Range(0, vipPool.Length);
        GameObject vip = vipPool[randomIndex];
        vip.SetActive(true);
        return vip;
    }

    private GameObject InstantiateRandomProduct()
    {
        int randomIndex = Random.Range(0, productList.Length);
        return Instantiate(productList[randomIndex], productSpawnPosition.position, Quaternion.identity);
    }

    private void SetVIPPreview(GameObject product)
    {
        var vipPreview = activeVIP.GetComponentInChildren<VipPlayer>().meshFilter;
        var vipRenderer = activeVIP.GetComponentInChildren<VipPlayer>().meshRenderer;

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
        activeVIP.transform.DOMove(vipDestination.position, movementDuration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                activeProduct = InstantiateRandomProduct();
                SetVIPPreview(activeProduct);
                gameData.isVipHere = true;
                StartCoroutine(StartInteractionTimer());
            });
    }

    private IEnumerator StartInteractionTimer()
    {
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
        var previewPosition = activeVIP.GetComponentInChildren<VipPlayer>().transform.position;

        activeProduct.GetComponent<VipProduct>().Reset();
        //transform.DORotate(player.GetComponent<PlayerTrigger>().ProductEnter.rotation.eulerAngles,.25f)
        activeProduct.transform.DORotate(previewPosition,0.25f);
        activeProduct.transform.DOMove(previewPosition, productMoveDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            //Particle, Sound
            ResetVIP();
            Debug.Log("Product moved to preview position.");
        });
    }

    private void HandleVIPSuccess()
    {
        Debug.Log("VIP request fulfilled successfully!");
        //ResetVIP();
    }

    private void HandleVIPFailure()
    {
        Debug.Log("Time's up! The VIP is leaving.");
        gameData.isVipHere = false;
        ResetVIP();
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

        activeVIP = null;
        activeProduct = null;
        playerInteracted = false;
    }
}
