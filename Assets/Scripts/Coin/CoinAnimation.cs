using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CoinAnimation : MonoBehaviour
{
    public GameObject CoinPrefab;

    [Min(1)]
    public int MaxCoinsCount=30;
    [Range(0,50)]
    public float SpawnCircleRadius=30;

    [Range(0,2)]
    public float CoinFlyDuration=0.5f;
    [Range(0,1)]
    public float CoinFlyDelay=0.5f;
    public Ease CoinFlyEase=Ease.OutCubic;
    private Camera cam;

    private void Awake()
    {
        cam=Camera.main;
    }

    public void PlayAnim(int count,Vector3 worldPosition)
    {
        count=Mathf.Min(count,MaxCoinsCount);

        Vector3 startPosition=cam.WorldToScreenPoint(worldPosition);
        Vector3 endPosition=transform.position;

        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset=Random.insideUnitCircle*SpawnCircleRadius;
            GameObject coin=Instantiate(CoinPrefab,startPosition+randomOffset,Quaternion.identity,transform);
            coin.transform.SetAsFirstSibling();

            coin.transform.DOMove(endPosition,CoinFlyDuration)
                .SetEase(CoinFlyEase)
                .SetDelay(CoinFlyDelay*i)
                .OnComplete(()=>Destroy(coin))
                .Play();
        }
    }
    
    
}
