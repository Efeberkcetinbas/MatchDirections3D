using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StuffManager : MonoBehaviour
{
    [SerializeField] private StuffConfig stuffConfig; // The configuration for this stuff
    [SerializeField] private GameObject stuffVisual; // The visual representation of the stuff

    private int collectedGameObjects = 0;
    private const string PlayerPrefsKey = "Stuff_";

    public bool IsUnlocked { get; private set; } = false;

    private void Start()
    {
        LoadProgress();
        UpdateStuffVisual();
    }

   

    

    public void AddGameObjects(GameObject[] products, float delayInterval)
    {
        if (IsUnlocked) return;

        for (int i = 0; i < products.Length; i++)
        {
            GameObject product = products[i];
            float delay = i * delayInterval;
            MoveProducts(product, delay);
            
        }

        DOTween.Sequence()
            .AppendInterval(products.Length * delayInterval) // Wait for all animations
            .AppendCallback(() =>
            {
                collectedGameObjects += products.Length;

                if (collectedGameObjects >= stuffConfig.requiredGameObjects)
                {
                    UnlockStuff();
                }
                SaveProgress();
            });
    }

    private void MoveProducts(GameObject product, float delay)
    {
        product.transform.DOMove(stuffVisual.transform.position, 0.5f)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                product.SetActive(false); // Deactivate emoji after use
            });
    }

    private void UnlockStuff()
    {
        IsUnlocked = true;
        Debug.Log($"{stuffConfig.stuffName} unlocked!");
        UpdateStuffVisual();
    }

    private void UpdateStuffVisual()
    {
        stuffVisual.SetActive(IsUnlocked);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey + stuffConfig.name, IsUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(PlayerPrefsKey + stuffConfig.name + "_collected", collectedGameObjects);
    }

    private void LoadProgress()
    {
        IsUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey + stuffConfig.name, 0) == 1;
        collectedGameObjects = PlayerPrefs.GetInt(PlayerPrefsKey + stuffConfig.name + "_collected", 0);
        UpdateStuffVisual();
    }
}
