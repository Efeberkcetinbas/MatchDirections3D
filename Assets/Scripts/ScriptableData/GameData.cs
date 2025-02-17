using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData", order = 0)]
public class GameData : ScriptableObject 
{

    public int score;
    public int starAmount;
    public int increaseScore;
    public int decreaseScore;
    public int levelIndex;
    public int levelNumber;
    public int dissatisfyPeople;

    //vip counter
    public int vipArriveNumber;
    public int getVipRandomNumber;
    
    //Game ending values
    public int earnedAmount;
    public int totalCustomerNumber;

    //Combo
    public int comboCount;
    public float currentInterval;
    public float elapsedTime;

    //Life
    public int lifeTime;

    public bool isGameEnd=false;
    public bool dissatisfy=false;
    public bool isFreezer=false;
    public bool isVipHere=false;
    public bool isUIIntheScene=false;

}
