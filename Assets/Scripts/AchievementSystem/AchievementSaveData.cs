using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementSaveData
{
    public string achievementName;
    public bool isCompleted;
    public int progress;

    public AchievementSaveData(string name, bool completed, int progress)
    {
        achievementName = name;
        isCompleted = completed;
        this.progress = progress;
    }
}