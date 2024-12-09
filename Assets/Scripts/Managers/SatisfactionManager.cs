using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatisfactionManager : MonoBehaviour
{

    [Range(0, 100)] public float Satisfaction = 100f;
    public float DecreaseRate = 0.1f; // Automatic decrease rate per second
    public float CorrectProductBoost = 10f;
    public float WrongProductPenalty = 15f;
    public float TimeoutPenalty = 20f;
    
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.AddHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.AddHandler(GameEvent.OnPlayerWaitTooMuch, OnPlayerWaitTooMuch);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnDismatch, OnDismatch);
        EventManager.RemoveHandler(GameEvent.OnPlayerWaitTooMuch, OnPlayerWaitTooMuch);
    }

    
    private void Update()
    {
        DecreaseSatisfaction(DecreaseRate * Time.deltaTime);
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
        DecreaseSatisfaction(TimeoutPenalty);
    }
}
