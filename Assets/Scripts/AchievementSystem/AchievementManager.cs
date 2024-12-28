using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private List<AchievementData> achievements;
    [SerializeField] private List<AchievementUI> achievementUIElements;
    [SerializeField] private AchievementPopUpUI popUpUI;

    private string saveFilePath;
    private AchievementCategory currentCategory = AchievementCategory.Combat;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "achievementData.json");
        LoadAchievements();
        UpdateAchievementUI(0);
    }

    public void UpdateAchievementUI(int page)
    {
        int itemsPerPage = achievementUIElements.Count;
        int startIndex = page * itemsPerPage;

        List<AchievementData> filteredAchievements = achievements.FindAll(a => a.category == currentCategory);

        for (int i = 0; i < achievementUIElements.Count; i++)
        {
            int achievementIndex = startIndex + i;

            if (achievementIndex < filteredAchievements.Count)
            {
                achievementUIElements[i].gameObject.SetActive(true);
                achievementUIElements[i].Setup(filteredAchievements[achievementIndex]);
            }
            else
            {
                achievementUIElements[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectCategory(int categoryIndex)
    {
        currentCategory = (AchievementCategory)categoryIndex;
        UpdateAchievementUI(0); // Reset to the first page of the selected category
    }

    public void UpdateProgress(AchievementData achievement, int amount)
    {
        if (achievement.isCompleted) return;

        achievement.progress += amount;
        if (achievement.progress >= achievement.targetProgress)
        {
            achievement.progress = achievement.targetProgress;
            CompleteAchievement(achievement);
        }

        SaveAchievements();
        RefreshUI();
    }

    private void CompleteAchievement(AchievementData achievement)
    {
        if (achievement.isCompleted) return;

        achievement.isCompleted = true;
        SaveAchievements();
        ShowAchievementPopUp(achievement);
    }

    private void ShowAchievementPopUp(AchievementData achievement)
    {
        if (popUpUI != null)
        {
            popUpUI.Show(achievement);
        }
    }

    private void SaveAchievements()
    {
        List<AchievementSaveData> saveData = new List<AchievementSaveData>();

        foreach (var achievement in achievements)
        {
            saveData.Add(new AchievementSaveData(
                achievement.achievementName, 
                achievement.isCompleted, 
                achievement.progress)
            );
        }

        string json = JsonUtility.ToJson(new SerializationWrapper<AchievementSaveData>(saveData), true);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadAchievements()
    {
        if (!File.Exists(saveFilePath)) return;

        string json = File.ReadAllText(saveFilePath);
        var saveData = JsonUtility.FromJson<SerializationWrapper<AchievementSaveData>>(json);

        foreach (var data in saveData.items)
        {
            var achievement = achievements.Find(a => a.achievementName == data.achievementName);
            if (achievement != null)
            {
                achievement.isCompleted = data.isCompleted;
                achievement.progress = data.progress;
            }
        }
    }

    private void RefreshUI()
    {
        // Refresh all achievement UI elements based on updated data
        for (int i = 0; i < achievementUIElements.Count; i++)
        {
            if (i < achievements.Count)
            {
                achievementUIElements[i].UpdateUI();
            }
        }
    }

    public void ClearAchievements()
    {
        // Reset achievements in memory
        foreach (var achievement in achievements)
        {
            achievement.isCompleted = false;
            achievement.progress = 0;
        }

        // Delete the saved file
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        Debug.Log("Achievements cleared and reset.");
        
        // Update the UI
        UpdateAchievementUI(0);
    }
}
