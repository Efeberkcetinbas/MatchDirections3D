using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    

    private WaitForSeconds waitForSeconds,failWaitforseconds;
    private WaitForSeconds freezerWaitForSeconds;


    private void Awake() 
    {
        ClearData();
        gameData.vipArriveNumber=0;
        gameData.starAmount=0;
        OnVipLeave();
        
    }

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(2);
        failWaitforseconds=new WaitForSeconds(0.5f);
        freezerWaitForSeconds=new WaitForSeconds(10);
        GetCustomerNumber();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnFail,OnFail);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnFreezerIn,OnFreezerIn);
        EventManager.AddHandler(GameEvent.OnVipLeave,OnVipLeave);

    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnFreezerIn,OnFreezerIn);
        EventManager.RemoveHandler(GameEvent.OnVipLeave,OnVipLeave);
    }

    private void OnVipLeave()
    {
        gameData.getVipRandomNumber=Random.Range(5,15);
    }
    

    private void OnNextLevel()
    {
        ClearData();
        
    }

    private void OnRestartLevel()
    {
        ClearData();
    }

   
    #region Helpers
    private void OnFreezerIn()
    {
        gameData.isFreezer=true;
        StartCoroutine(SetFreezeOut());
    }

    private IEnumerator SetFreezeOut()
    {
        yield return freezerWaitForSeconds;
        gameData.isFreezer=false;
        EventManager.Broadcast(GameEvent.OnFreezerOut);
    }

    

   
    #endregion

    private void GetCustomerNumber()
    {
        gameData.totalCustomerNumber=PlayerPrefs.GetInt("CustomerNumber",0);
    }
    private void ClearData()
    {
        gameData.isGameEnd=true;
        gameData.dissatisfy=false;
        gameData.isFreezer=false;
        gameData.isUIIntheScene=false;

        gameData.dissatisfyPeople=0;
        gameData.comboCount=0;
        gameData.elapsedTime=0;
        gameData.earnedAmount=0;

        
    }

    private void OnSuccess()
    {
        gameData.isGameEnd=true;
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
        yield return failWaitforseconds;
        OpenFailPanel();
    }


    private void OpenFailPanel()
    {
       
        EventManager.Broadcast(GameEvent.OnFailUI);
    }



    
}
