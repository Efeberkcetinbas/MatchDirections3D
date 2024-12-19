using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpecifiedPeople
{
    public int RequirementProductNumber;
    public int MultiplyPlaceHolder;
    public Mesh placeholderMesh;
    public Material mat;
    public PlayerWaitSettings playerWaitSettings;

}
public class PeopleLevel : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private List<SpecifiedPeople> specifiedPeoples=new List<SpecifiedPeople>();
    [SerializeField] private Player player;
    [SerializeField] private PlayerWait playerWait;

    private void Start()
    {
        OnAssignPeople();
    }

    

    internal void OnAssignPeople()
    {
        if (player != null)
        {
            player.requirementProduct=specifiedPeoples[gameData.levelIndex].RequirementProductNumber;
            player.placeholderMesh=specifiedPeoples[gameData.levelIndex].placeholderMesh;
            player.multiply=specifiedPeoples[gameData.levelIndex].MultiplyPlaceHolder;
            player.mat=specifiedPeoples[gameData.levelIndex].mat;
            playerWait.ApplyWaitSettings(specifiedPeoples[gameData.levelIndex].playerWaitSettings);
        }
    }
}
