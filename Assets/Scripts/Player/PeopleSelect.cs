using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSelect : MonoBehaviour
{
    [SerializeField] private List<GameObject> peoples=new List<GameObject>();


    private int index;
    private void Start()
    {
        SelectPeople();
    }


    private void RandomizePeople()
    {
        index = Random.Range(0,peoples.Count);
    }

    private void SelectPeople()
    {
        RandomizePeople();
        for (int i = 0; i < peoples.Count; i++)
        {
            peoples[i].SetActive(false);
        }
        peoples[index].SetActive(true);
    }
}
