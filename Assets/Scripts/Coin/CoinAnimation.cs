using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CoinAnimation : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject CoinPrefab;
    [Min(1)] public int MaxCoinsCount = 30;
    [Range(0, 50)] public float SpawnCircleRadius = 30;

    [Header("Animation Settings")]
    /*[Range(0, 2)] public float CoinFlyDuration = 0.5f;
    [Range(0, 1)] public float CoinFlyDelay = 0.5f;*/
    public Ease CoinFlyEase = Ease.OutCubic;
    [SerializeField] private Animator animator;

    private Camera cam;
    private ObjectPool<GameObject> coinPool;

    private void Awake()
    {
        cam = Camera.main;
        coinPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(CoinPrefab, transform),
            actionOnGet: coin => coin.SetActive(true),
            actionOnRelease: coin => coin.SetActive(false),
            maxSize: MaxCoinsCount
        );
    }

    public void PlayAnim(int count, Vector3 worldPosition)
    {
        count = Mathf.Min(count, MaxCoinsCount);

        Vector3 startPosition = cam.WorldToScreenPoint(worldPosition);
        Vector3 endPosition = transform.position;

        // Calculate dynamic delays and durations to fit within 1 second
        float totalAnimationTime = 1f; // Total time in seconds
        float delayPerCoin = totalAnimationTime / count;
        float adjustedFlyDuration = totalAnimationTime - delayPerCoin * (count - 1);

        int completedCount = 0; // Counter for completed animations

        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * SpawnCircleRadius;
            GameObject coin = coinPool.Get();
            coin.transform.position = startPosition + randomOffset;
            coin.transform.SetAsFirstSibling();

            coin.transform.DOMove(endPosition, adjustedFlyDuration)
                .SetEase(CoinFlyEase)
                .SetDelay(delayPerCoin * i)
                .OnComplete(() =>
                {
                    coinPool.Release(coin);
                    completedCount++;
                    EventManager.Broadcast(GameEvent.OnCoinIncreaseSound);
                    // Check if all coins are done
                    if (completedCount == count)
                    {
                        EventManager.Broadcast(GameEvent.OnIncreaseScore);
                        animator.SetBool("ScaleUp", false);
                    }
                })
                .Play();
            // Start the ScaleUp animation when the coins start moving
            animator.SetBool("ScaleUp", true);
        }

        
    }
    
    
}

