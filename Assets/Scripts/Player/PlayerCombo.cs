using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCombo : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    [Header("Combo Settings")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float intervalDecrement = 1f;
    [SerializeField] private List<GameObject> comboList=new List<GameObject>();
    
    [Header("UI Elements to Randomize")]
    [SerializeField] private List<RectTransform> posUIElements;
    [SerializeField] private List<RectTransform> colorUIElements; // Assign UI Images/Texts RectTransforms
    [SerializeField] private List<Color> colorList;          // Predefined list of colors

    [Header("Position Range")]
    [SerializeField] private float xMin = -400f;
    [SerializeField] private float xMax = 400f;
    [SerializeField] private float yMin = 750f;
    [SerializeField] private float yMax = 850f;


   
    private bool isComboActive;

    

    

    private void Update()
    {
        if (!isComboActive) return;

        gameData.elapsedTime += Time.deltaTime;
        EventManager.Broadcast(GameEvent.OnComboProgress);

        if (gameData.elapsedTime >= gameData.currentInterval)
        {
            ResetCombo();
        }
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.AddHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
    }

    private void OnMatchFound()
    {
        SetActivity(true);

        gameData.comboCount++;
        gameData.elapsedTime = 0;
        isComboActive = true;

        EventManager.Broadcast(GameEvent.OnComboUIUpdate);

        RandomizeUIElements();

        // Adjust interval for increasing difficulty
        if (gameData.currentInterval > minInterval)
        {
            gameData.currentInterval -= intervalDecrement;
        }
    }

    private void OnSuccess()
    {
        ResetCombo();
    }

    private void OnFail()
    {
        ResetCombo();
    }

    private void OnDismatch()
    {
        ResetCombo();
    }

    private void ResetCombo()
    {
        gameData.comboCount = 0;
        gameData.elapsedTime = 0;
        isComboActive = false;
        EventManager.Broadcast(GameEvent.OnComboUIUpdate);
        SetActivity(false);
        
        
    }

    private void SetActivity(bool val)
    {
        for (int i = 0; i < comboList.Count; i++)
        {
            comboList[i].SetActive(val);
        }
    }

    /// <summary>
    /// Sets random positions and same random colors for related UI Images and Texts.
    /// </summary>
    private void RandomizeUIElements()
    {
        /*foreach (RectTransform element in posUIElements)
        {
            // Set Random Position
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);
            element.anchoredPosition = new Vector2(randomX, randomY);
        }*/
        
        Color randomColor = GetRandomColor();

        foreach (RectTransform element in colorUIElements)
        {
            // Set Random Color (Same for Image and Text)
            ApplyColorToElement(element, randomColor);
        }
    }

    /// <summary>
    /// Returns a random color from the predefined list.
    /// </summary>
    /// <returns>Color</returns>
    private Color GetRandomColor()
    {
        if (colorList.Count > 0)
        {
            int randomIndex = Random.Range(0, colorList.Count);
            return colorList[randomIndex];
        }
        return Color.white; // Default to white if no colors are defined
    }

    /// <summary>
    /// Applies the same color to Image and Text components within the UI element.
    /// </summary>
    /// <param name="element">RectTransform of the UI element</param>
    /// <param name="color">Color to apply</param>
    private void ApplyColorToElement(RectTransform element, Color color)
    {
        // Check and set color for Image component
        if (element.TryGetComponent<Image>(out Image imageComponent))
        {
            imageComponent.color = color;
        }

        // Check and set color for Text component
        if (element.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI textComponent))
        {
            textComponent.color = color;
        }
    }
}
