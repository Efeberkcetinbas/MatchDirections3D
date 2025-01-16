using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameEvent
{
    //Player
    OnPlayerStartMove,
    OnMatchFullPlayer,
    OnPlayerThanks,
    OnPlayerLeaving,

    //VIP
    OnVipProductTouched,
    OnVipOnTheWay,
    OnVipProductCreated,
    OnVipProductPlaced,
    OnVipSummoned,
    OnVipSuccess,
    OnVipFail,
    OnVipLeave,

    

    //Player-Product
    
    OnProductPlaced,
    OnMatchFound,
    OnDismatch,
    OnPlayerWaitTooMuch,
    OnProductDragStart,
    OnProductDragStop,
    OnProductReset,

    //Environment
    OnDestinationProduct,

    //Mini Game
    OnUpdateMiniMap,
    OnUpdateBuilding,
    OnUpdateStuff,

    //Helpers
    OnCheckHelpers,
    OnFreezerIn,
    OnCollector,
    OnCollectorMove,
    OnCollectorEnd,
    OnSetSameProductsUp,
    OnFreezerOut,
    OnSetMaxSatisfaction,
    OnOpenPurchasePanel,
    OnClosePurchasePanel,
    
    //Game Management
    OnGameStart,
    OnStartGameFromHomescreen,
    OnProductMakerEnd,
    OnProductMakerExplode,
    OnIncreaseScore,
    OnDecreaseScore,

    //Life Manager
    OnLifeIncrease,
    OnLifeDecrease,
    OnLifeFullUI,
    OnLifeFull,
   
    //UI
    OnScoreUIUpdate,
    OnComboUIUpdate,
    OnLevelUpdateUI,
    OnComboProgress,
    OnCoinIncreaseSound,
    OnBuyButtonTap,
    OnUseButtonTap,



    
    OnNextLevel,
    OnSuccess,
    OnFail,
    OnSuccessUI,
    OnFailUI,
    OnRestartLevel,

}
public class EventManager
{
    private static Dictionary<GameEvent,Action> eventTable = new Dictionary<GameEvent, Action>();
    
    private static Dictionary<GameEvent,Action<int>> IdEventTable=new Dictionary<GameEvent, Action<int>>();
    //2 parametre baglayacagimiz ile bagladigimiz

    
    public static void AddHandler(GameEvent gameEvent,Action action)
    {
        if(!eventTable.ContainsKey(gameEvent))
            eventTable[gameEvent]=action;
        else eventTable[gameEvent]+=action;
    }

    public static void RemoveHandler(GameEvent gameEvent,Action action)
    {
        if(eventTable[gameEvent]!=null)
            eventTable[gameEvent]-=action;
        if(eventTable[gameEvent]==null)
            eventTable.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent gameEvent)
    {
        if(eventTable[gameEvent]!=null)
            eventTable[gameEvent]();
    }
    
}
