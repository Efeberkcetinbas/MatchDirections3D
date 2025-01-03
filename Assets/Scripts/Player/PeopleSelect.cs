using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PeopleSelect : MonoBehaviour
{
    public List<GameObject> peoples=new List<GameObject>();


    internal int index;

    [SerializeField] private GameObject snowman;
    [SerializeField] private ParticleSystem snowParticle;

    private void Start()
    {
        SelectPeople();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFreezerIn,OnFreezerIn);
        EventManager.AddHandler(GameEvent.OnFreezerOut,OnFreezerOut);
    }

    private void OnDisable()
    {   
        EventManager.RemoveHandler(GameEvent.OnFreezerIn,OnFreezerIn);
        EventManager.RemoveHandler(GameEvent.OnFreezerOut,OnFreezerOut);
    }

    private void OnFreezerIn()
    {
        for(int i = 0; i < peoples[index].transform.childCount; ++i)
        {
            peoples[index].transform.GetChild(i).gameObject.SetActive(false); // or false
        }

        
        snowParticle.Play();
        snowman.transform.localScale=Vector3.zero;
        snowman.SetActive(true);
        snowman.transform.DOScale(Vector3.one*12,0.5f).SetEase(Ease.OutQuint);
    }

    private void OnFreezerOut()
    {
        for(int i = 0; i < peoples[index].transform.childCount; ++i)
        {
            peoples[index].transform.GetChild(i).gameObject.SetActive(true); // or false
        }
        snowParticle.Play();
        snowman.SetActive(false);
    }

    private void RandomizePeople()
    {
        index = Random.Range(0,peoples.Count);
    }

    internal void SelectPeople()
    {
        snowman.SetActive(false);
        RandomizePeople();
        for (int i = 0; i < peoples.Count; i++)
        {
            peoples[i].SetActive(false);
        }
        peoples[index].SetActive(true);
        for(int i = 0; i < peoples[index].transform.childCount; ++i)
        {
            peoples[index].transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
