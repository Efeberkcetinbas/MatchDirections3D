using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombo : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    [Header("Combo Settings")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float intervalDecrement = 1f;

   
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
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFound, OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnDismatch, OnDismatch);
    }

    private void OnMatchFound()
    {
        gameData.comboCount++;
        gameData.elapsedTime = 0;
        isComboActive = true;

        EventManager.Broadcast(GameEvent.OnComboUIUpdate);

        // Adjust interval for increasing difficulty
        if (gameData.currentInterval > minInterval)
        {
            gameData.currentInterval -= intervalDecrement;
        }
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
        
    }
}
