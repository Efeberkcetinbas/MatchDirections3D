using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AchievementCategory
{
    Combat,
    Exploration,
    Collection,
    Social
}


[CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievements/AchievementData")]
public class AchievementData : ScriptableObject
{
    public string achievementName;
    public Sprite imageSprite;
    public string successText;
    public bool isCompleted;
    public int progress;
    public int targetProgress; // Required progress to complete the achievement
    public AchievementCategory category; // New field
}