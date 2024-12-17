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
    
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.AddHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
        EventManager.AddHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.AddHandler(GameEvent.OnPlayerWaitTooMuch, OnPlayerWaitTooMuch);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
        EventManager.RemoveHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.RemoveHandler(GameEvent.OnPlayerWaitTooMuch, OnPlayerWaitTooMuch);
    }

    
    private void Update()
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

    private Color GetColorForProgress(float progress)
    {
        // Define the colors
        Color green = Color.green;
        Color yellow = Color.yellow;
        Color orange = new Color(1f, 0.5f, 0f); // Orange
        Color red = Color.red;

        if (progress < 0.33f)
        {
            return Color.Lerp(red, orange, progress / 0.33f);
        }
        else if (progress < 0.66f)
        {
            return Color.Lerp(orange, yellow, (progress - 0.33f) / 0.33f);
        }
        else
        {
            return Color.Lerp(yellow, green, (progress - 0.66f) / 0.34f);
        }
    }
}
