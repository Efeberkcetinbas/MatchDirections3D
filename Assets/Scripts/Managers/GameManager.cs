using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;


    private WaitForSeconds waitForSeconds;


    private void Awake() 
    {
        ClearData();
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnFail,OnFail);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);
    }

    
    

    private void OnNextLevel()
    {
        ClearData();
        
    }

    private void OnRestartLevel()
    {
        ClearData();
    }

   

   
    

    private void ClearData()
    {
        gameData.isGameEnd=true;
        gameData.dissatisfy=false;

        gameData.dissatisfyPeople=0;
        gameData.comboCount=0;
        gameData.elapsedTime=0;
    }

    private void OnSuccess()
    {
        StartCoroutine(OpenSuccess());
    }

    private void OnFail()
    {
        gameData.isGameEnd=true;
        Debug.Log("SATISFACTION RUN OUT");
        StartCoroutine(OpenFail());
    }

    private IEnumerator OpenSuccess()
    {
        yield return waitForSeconds;
        OpenSuccessPanel();
    }


    private void OpenSuccessPanel()
    {
        EventManager.Broadcast(GameEvent.OnSuccessUI);
    }

    private IEnumerator OpenFail()
    {
        yield return waitForSeconds;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
       
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}
