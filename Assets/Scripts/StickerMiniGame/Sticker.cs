using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class Sticker : MonoBehaviour
{
    public Image stickerImage; // Reference to the UI Image component
    public TextMeshProUGUI stickerText; // Reference to the UI Text component
    public int requiredStars; // Stars required to unlock this sticker
    [Range(0, 100)] public float percentOpen; // Unlock progress (0 = locked, 100 = fully unlocked)
    public bool isFullyOpened; // Whether this sticker is fully opened

    // Method to update the UI display based on current progress
    public void UpdateStickerUI()
    {
        float initialPercent = stickerImage.fillAmount * 100;
        float targetPercent = percentOpen;

        DOTween.To(() => initialPercent, value =>
        {
            initialPercent = value;
            stickerImage.fillAmount = value / 100f; // Update the image fill amount
            stickerText.SetText($"{Mathf.FloorToInt(value)} %"); // Update the text percentage
        }, targetPercent, 1f).OnComplete(() =>
        {
            if (percentOpen >= 100 && !isFullyOpened)
            {
                StickerOpened();
            }
        });
    }

    // Called when the sticker reaches 100% fill
    public void StickerOpened()
    {
        isFullyOpened = true;
        Debug.Log($"Sticker {gameObject.name} is fully opened!");
        // Add your desired DOTween effect here
        transform.DOScale(1.2f, 0.3f).SetLoops(2, LoopType.Yoyo); // Example animation
    }

    // Load sticker state
    public void LoadSticker(float percent, bool fullyOpened)
    {
        percentOpen = percent;
        isFullyOpened = fullyOpened;
        UpdateStickerUI();
    }

    // Save sticker state
    public StickerData SaveSticker()
    {
        return new StickerData
        {
            percentOpen = percentOpen,
            isFullyOpened = isFullyOpened
        };
    }
}

[System.Serializable]
public class StickerData
{
    public float percentOpen;
    public bool isFullyOpened;
}
