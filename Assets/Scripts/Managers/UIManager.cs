using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI gameEndinglevelText;

    [Header("Star")]
    [SerializeField] private TextMeshProUGUI starAmountText;
    [SerializeField] private TextMeshProUGUI startStarAmountText;
    

    [Header("Combo Part")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Image comboProgressBar;

    [Header("Coin Amount")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI startCoinText;

    [Header("DATA'S")]
    public GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnScoreUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnComboProgress,OnComboProgress);
        EventManager.AddHandler(GameEvent.OnComboUIUpdate,OnComboUIUpdate);
        EventManager.AddHandler(GameEvent.OnLevelUpdateUI, OnLevelUpdateUI);
        EventManager.AddHandler(GameEvent.OnIncreaseStar,OnIncreaseStar);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnScoreUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnComboProgress,OnComboProgress);
        EventManager.RemoveHandler(GameEvent.OnComboUIUpdate,OnComboUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnLevelUpdateUI, OnLevelUpdateUI);
        EventManager.RemoveHandler(GameEvent.OnIncreaseStar,OnIncreaseStar);
    }

    private void Start()
    {
        OnUIUpdate();
        OnLevelUpdateUI();
        OnIncreaseStar();
    }

    
    private void OnUIUpdate()
    {
        coinText.SetText(gameData.score.ToString());
        startCoinText.SetText(gameData.score.ToString());
        
    }

    private void OnIncreaseStar()
    {
        starAmountText.SetText(gameData.starAmount.ToString());
        startStarAmountText.SetText(gameData.starAmount.ToString());
    }

    private void OnLevelUpdateUI()
    {
        levelText.SetText("LEVEL " + (gameData.levelNumber+1).ToString());
        gameEndinglevelText.SetText("LEVEL " + (gameData.levelNumber+1).ToString());
    }

    private void OnComboProgress()
    {
        var val=gameData.elapsedTime/gameData.currentInterval;
        comboProgressBar.fillAmount=val;
    }

    private void OnComboUIUpdate()
    {
        comboText.SetText("x " + gameData.comboCount);
    }

    
    

    
    
    
}
