using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private Toggle checkbox;
    [SerializeField] private TextMeshProUGUI progressText;

    private AchievementData achievement;

    public void Setup(AchievementData achievementData)
    {
        achievement = achievementData;
        icon.sprite = achievement.imageSprite;
        successText.text = achievement.successText;
        checkbox.isOn = achievement.isCompleted;
        checkbox.interactable = false; // Prevent manual interaction
        UpdateProgressUI();
    }

    public void UpdateUI()
    {
        if (achievement != null)
        {
            checkbox.isOn = achievement.isCompleted;
            UpdateProgressUI();
        }
    }

    private void UpdateProgressUI()
    {
        progressText.text = $"{achievement.progress}/{achievement.targetProgress}";
    }
}
