using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AchievementPopUpUI : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI successText;

    public void Show(AchievementData achievement)
    {
        popUp.SetActive(true);
        icon.sprite = achievement.imageSprite;
        successText.text = achievement.successText;

        // Automatically hide the popup after 3 seconds
        Invoke(nameof(Hide), 3f);
    }

    private void Hide()
    {
        popUp.SetActive(false);
    }
}
