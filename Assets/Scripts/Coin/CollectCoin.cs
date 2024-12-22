using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public Transform WorldPoint;
    public CoinAnimation coinAnimation;


    public void StartCollectCoin()
    {
        int randomCoin=Random.Range(0,coinAnimation.MaxCoinsCount);
        coinAnimation.PlayAnim(randomCoin,WorldPoint.position);
    }
}
