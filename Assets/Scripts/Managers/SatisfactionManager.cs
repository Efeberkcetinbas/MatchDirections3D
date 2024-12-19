using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{   
    [Header("Satisfaction Settings")]
    [SerializeField] private Image satisfactionProgress;
    [Range(0, 100)] public float Satisfaction = 100f;
    public float DecreaseRate = 0.1f; // Automatic decrease rate per second
    public float CorrectProductBoost = 10f;
    public float WrongProductPenalty = 15f;
    public float TimeoutPenalty = 20f;

    [SerializeField] private GameData gameData;
    
    private Gradient progressGradient;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.AddHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
        EventManager.AddHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.AddHandler(GameEvent.OnPlayerWaitTooMuch, OnPlayerWaitTooMuch);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
        EventManager.RemoveHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.RemoveHandler(GameEvent.OnPlayerWaitTooMuch, OnPlayerWaitTooMuch);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
    }

    private void Start()
    {
        InitializeColorGradientList();
    }
    
    private void Update()
    {
        if(!gameData.isGameEnd)
        {
            DecreaseSatisfaction(DecreaseRate * Time.deltaTime);
            
            if(gameData.dissatisfy)
            {
                DecreaseSatisfaction(TimeoutPenalty*Time.deltaTime * gameData.dissatisfyPeople);
            }

            float val=Satisfaction/100;
            satisfactionProgress.fillAmount=val;
            satisfactionProgress.color=GetColorForProgress(val);
        }
    }

    private void OnNextLevel()
    {
        Satisfaction=100;
    }

    private void OnMatchFound()
    {
        OnCorrectProduct();
    }

    private void OnDismatch()
    {
        OnWrongProduct();
    }
    
    private void OnPlayerWaitTooMuch()
    {
        OnTimeout();
    }

    private void OnMatchFullPlayer()
    {
        if(gameData.dissatisfy)
        {
            gameData.dissatisfyPeople--;
            if(gameData.dissatisfyPeople<=0)
            {
                gameData.dissatisfy=false;
            }
        }
    }
    private void IncreaseSatisfaction(float amount)
    {
        Satisfaction = Mathf.Clamp(Satisfaction + amount, 0, 100);
    }

    private void DecreaseSatisfaction(float amount = 1f)
    {
        Satisfaction = Mathf.Clamp(Satisfaction - amount, 0, 100);
    }

    private void OnCorrectProduct()
    {
        IncreaseSatisfaction(CorrectProductBoost);
    }

    private void OnWrongProduct()
    {
        DecreaseSatisfaction(WrongProductPenalty);
    }

    private void OnTimeout()
    {
        gameData.dissatisfy=true;
    }

    private void InitializeColorGradientList()
    {
        progressGradient = new Gradient
        {
            colorKeys = new[]
            {
                new GradientColorKey(Color.red, 0f),   // Start at 0
                new GradientColorKey(new Color(1f, 0.5f, 0f), 0.33f), // Orange
                new GradientColorKey(Color.yellow, 0.66f), // Yellow
                new GradientColorKey(Color.green, 1f)  // End at 1
            }
        };
    }        
    private Color GetColorForProgress(float progress)
    {
        return progressGradient.Evaluate(progress);
    }
}
