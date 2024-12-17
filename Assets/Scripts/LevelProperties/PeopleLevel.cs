using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpecifiedPeople
{
    public int RequirementProductNumber;
    public Mesh placeholderMesh;
    public Material mat;

}
public class PeopleLevel : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private List<SpecifiedPeople> specifiedPeoples=new List<SpecifiedPeople>();
    [SerializeField] private Player player;

    private void Start()
    {
        OnNextLevel();
    }

    private void OnNextLevel()
    {
        if (player != null)
        {
            player.requirementProduct=specifiedPeoples[gameData.levelIndex].RequirementProductNumber;
            player.placeholderMesh=specifiedPeoples[gameData.levelIndex].placeholderMesh;
            player.mat=specifiedPeoples[gameData.levelIndex].mat;
        }
    }
}
