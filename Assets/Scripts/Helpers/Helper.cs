using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[Serializable]
public class HelperProperties
{
    public Button BuyButton;
    public Button UseButton;
    public GameObject Locked;
    public TextMeshProUGUI UseAmountText;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI UnlockedLevelText;
    public HelperConfig helperConfig;
    public GameEvent gameEvent;
}

public class Helper : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private List<HelperProperties> helperProperties=new List<HelperProperties>();

    private bool isUsingHelper=false;

    private void Start()
    {
        AssignHelperImages();
        CheckIfButtonAvailable();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckHelpers,OnCheckHelpers);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnCollector,OnCollector);
        EventManager.AddHandler(GameEvent.OnCollectorEnd,OnCollectorEnd);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckHelpers,OnCheckHelpers);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnCollector,OnCollector);
        EventManager.RemoveHandler(GameEvent.OnCollectorEnd,OnCollectorEnd);
    }

    private void OnCheckHelpers()
    {
        CheckIfButtonAvailable();
    }

    //Or OnGameStart
    private void OnNextLevel()
    {
        CheckIfButtonAvailable();
    }

    private void OnCollector()
    {
        isUsingHelper=true;
        CheckIfButtonAvailable();
    }

    private void OnCollectorEnd()
    {
        isUsingHelper=false;
        CheckIfButtonAvailable();
    }

    private void AssignHelperImages()
    {
        for (int i = 0; i < helperProperties.Count; i++)
        {
            //helperProperties[i].HelperImage.sprite=helperProperties[i].helperConfig.HelperSprite;
            var helperProperty = helperProperties[i];
            var config = helperProperty.helperConfig;

            // Load the Amount value from PlayerPrefs, defaulting to 0 if not set
            config.Amount = PlayerPrefs.GetInt($"Helper_{i}_Amount", 0);
            helperProperty.UseAmountText.SetText(config.Amount.ToString());
            helperProperty.PriceText.SetText(config.RequirementScore.ToString());

            // Check lock status and set unlock level text
            bool isLocked = (gameData.levelNumber+1) < config.UnlockLevel;
            helperProperty.Locked.SetActive(isLocked);
            helperProperty.UnlockedLevelText.SetText($"Unlocks at Level {config.UnlockLevel}");
            helperProperty.UnlockedLevelText.gameObject.SetActive(isLocked);
        }
    }

    private void CheckIfButtonAvailable()
    {

        for (int i = 0; i < helperProperties.Count; i++)
        {
            var helperProperty = helperProperties[i];
            var config = helperProperty.helperConfig;
            var buyButton = helperProperty.BuyButton;
            var useButton = helperProperty.UseButton;
            var lockedButton=helperProperty.Locked;

             // Check if the helper is unlocked based on the level
            bool isUnlocked = (gameData.levelNumber+1) >= config.UnlockLevel;

            // Check if the helper has been bought
            bool hasAmount = config.Amount > 0;

            // Check if the score requirement is met
            bool canBuy = config.RequirementScore <= gameData.score;

            // Update lock panel visibility
            lockedButton.SetActive(!isUnlocked);

            // Update button visibility
            buyButton.gameObject.SetActive(isUnlocked && !hasAmount);
            useButton.gameObject.SetActive(isUnlocked && hasAmount);



            // Update button interactability
            buyButton.interactable = isUnlocked && !hasAmount && canBuy && !isUsingHelper;
            useButton.interactable = isUnlocked && hasAmount && !isUsingHelper;

            PlayerPrefs.SetInt($"Helper_{i}_Amount", config.Amount);
        }

        PlayerPrefs.Save();
    }

    public void BuyHelper(int index)
    {
        if(gameData.score>=helperProperties[index].helperConfig.RequirementScore)
        {
            gameData.score-=helperProperties[index].helperConfig.RequirementScore;
            gameData.decreaseScore=helperProperties[index].helperConfig.RequirementScore;
            EventManager.Broadcast(GameEvent.OnScoreUIUpdate);
            PlayerPrefs.SetInt("Score",gameData.score);
            helperProperties[index].helperConfig.Amount=helperProperties[index].helperConfig.GivenAmount;
            helperProperties[index].UseAmountText.SetText(helperProperties[index].helperConfig.Amount.ToString());

            EventManager.Broadcast(GameEvent.OnBuyButtonTap);
            
            CheckIfButtonAvailable();
        }

        else
            return;
    }

    public void UseHelper(int index)
    {
        helperProperties[index].helperConfig.Amount--;
        helperProperties[index].UseAmountText.SetText(helperProperties[index].helperConfig.Amount.ToString());
        EventManager.Broadcast(helperProperties[index].gameEvent);
        EventManager.Broadcast(GameEvent.OnUseButtonTap);
        CheckIfButtonAvailable();
    }


    
}
