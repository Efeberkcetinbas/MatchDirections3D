using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Scene Texts")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Combo Part")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Image progressBar;

    [Header("DATA'S")]
    public GameData gameData;
    public PlayerData playerData;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.AddHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.AddHandler(GameEvent.OnComboProgress,OnComboProgress);
        EventManager.AddHandler(GameEvent.OnComboUIUpdate,OnComboUIUpdate);
        
        
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUIUpdate, OnUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnLevelUIUpdate,OnLevelUIUpdate);
        EventManager.RemoveHandler(GameEvent.OnComboProgress,OnComboProgress);
        EventManager.RemoveHandler(GameEvent.OnComboUIUpdate,OnComboUIUpdate);
    }

    
    private void OnUIUpdate()
    {
        score.SetText(gameData.score.ToString());
        score.transform.DOScale(new Vector3(1.5f,1.5f,1.5f),0.2f).OnComplete(()=>score.transform.DOScale(new Vector3(1,1f,1f),0.2f));
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
