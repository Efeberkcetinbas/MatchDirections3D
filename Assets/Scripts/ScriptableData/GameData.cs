using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{

    public int score;
    public int increaseScore;
    public int levelIndex;
    public int levelNumber;


    //Combo
    public int comboCount;
    public float currentInterval;
    public float elapsedTime;

    public bool isGameEnd=false;

}
