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

    [Header("Combo Part")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Image progressBar;

    [Header("Coin Amount")]
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("DATA'S")]
    public GameData gameData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnScoreUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.AddHandler(GameEvent.OnComboProgress,OnComboProgress);
        EventManager.AddHandler(GameEvent.OnComboUIUpdate,OnComboUIUpdate);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnScoreUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnComboProgress,OnComboProgress);
        EventManager.RemoveHandler(GameEvent.OnComboUIUpdate,OnComboUIUpdate);
    }

    
    private void OnUIUpdate()
    {
        coinText.SetText(gameData.score.ToString());
    }

    private void OnLevelUIUpdate()
    {
        levelText.SetText("LEVEL " + (gameData.levelNumber+1).ToString());
    }

    private void OnComboProgress()
    {
        var val=gameData.elapsedTime/gameData.currentInterval;
        progressBar.fillAmount=val;
    }

    private void OnComboUIUpdate()
    {
        comboText.SetText("x " + gameData.comboCount);
    }

    
    

    
    
    
}
