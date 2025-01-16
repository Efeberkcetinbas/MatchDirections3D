using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProductMaker : MonoBehaviour
{
    [SerializeField] private List<GameObject> presentList=new List<GameObject>();
    [SerializeField] private Transform presentParent;
    [SerializeField] private ParticleSystem dustParticle;

    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.25f);
    }


    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGameStart, OnGameStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGameStart, OnGameStart);
    }

    private void OnGameStart()
    {
        presentParent.gameObject.SetActive(true);
        GetPresent();
        presentParent.DOShakeScale(2.5f,1.5f,10).OnComplete(()=>{
            EventManager.Broadcast(GameEvent.OnProductMakerExplode);
            presentParent.gameObject.SetActive(false);
            dustParticle.Play();
            StartCoroutine(SetProducts());
        });
    }

    private IEnumerator SetProducts()
    {
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnProductMakerEnd);
    }


    private void GetPresent()
    {
        int randomIndex=Random.Range(0, presentList.Count);

        for (int i = 0; i < presentList.Count; i++)
        {
            presentList[i].gameObject.SetActive(false);
        }

        presentList[randomIndex].SetActive(true);
    }
}
