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
    public TextMeshProUGUI UseAmountText;
    public TextMeshProUGUI PriceText;
    public HelperConfig helperConfig;
    public GameEvent gameEvent;
}

public class Helper : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private List<HelperProperties> helperProperties=new List<HelperProperties>();



    private void Start()
    {
        AssignHelperImages();
        CheckIfButtonAvailable();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckHelpers,OnCheckHelpers);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckHelpers,OnCheckHelpers);
    }

    private void OnCheckHelpers()
    {
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
            //helperProperty.PriceText.SetText(config.RequirementScore.ToString());
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

            // Check if the helper has been bought
            bool hasAmount = config.Amount > 0;

            // Check if the score requirement is met
            bool canBuy = config.RequirementScore <= gameData.score;

            // Update button visibility
            buyButton.gameObject.SetActive(!hasAmount);
            useButton.gameObject.SetActive(hasAmount);

            // Update button interactability
            /*buyButton.interactable = !hasAmount && canBuy;
            useButton.interactable = hasAmount;*/

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
        CheckIfButtonAvailable();
    }


    
}
