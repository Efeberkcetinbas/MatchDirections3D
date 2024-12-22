using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] private Transform WorldPoint;
    private CoinAnimation coinAnimation;

    private void Start()
    {
        coinAnimation=FindObjectOfType<CoinAnimation>();
    }



    internal void StartCollectCoin(int coincount)
    {
        coinAnimation.PlayAnim(coincount,WorldPoint.position);
    }

}
