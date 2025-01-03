using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AudioClipsGameManagement
{
    public AudioClip SuccessSound;
    public AudioClip SuccessUISound;
    public AudioClip RestartSound;
    public AudioClip NextLevelSound;
    public AudioClip StartSound;
    public AudioClip FailUISound;
    public AudioClip IncreaseScoreSound;
    public AudioClip HelperBuyButtonTapSound;
    public AudioClip HelperUseButtonTapSound;
    
}

[Serializable]
public class AudioClipsPlayer
{
    public AudioClip PlayersStartMoveSound;
    public AudioClip PlayerWaitTooMuchSound;
    public AudioClip MatchSound;
    public AudioClip DismatchSound;
    public AudioClip ProductDragSound;
    public AudioClip ProductDropSound;
    public AudioClip ProductResetSound;
    public AudioClip CompletedMatchSound;
    public AudioClip ProductPlacedSound;
    public AudioClip PlayerThanksSound;
}

[Serializable]
public class AudioClipsHelpers
{
    public AudioClip CollectorMoveSound;
    
}

[Serializable]
public class AudioClipsVIP
{
    public AudioClip VipSummonSound;
    public AudioClip VipArrivedSound;
    public AudioClip VipProductHitSound;
    public AudioClip VipProductPlacedSound;
    public AudioClip VipLeaveSound;
    public AudioClip VipSuccessManSound;
    public AudioClip VipSuccessFemaleSound;
    public AudioClip VipFailManSound;
    public AudioClip VipFailFemaleSound;

    
}
public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;

    [Header("Game Management")]
    public AudioClipsGameManagement audioClipsGameManagement;
    [Header("Player")]
    public AudioClipsPlayer audioClipsPlayer;

    [Header("Helpers")]
    public AudioClipsHelpers audioClipsHelpers;

    [Header("VIP")]
    public AudioClipsVIP audioClipsVIP;

    private AudioSource musicSource,effectSource;
    


    private void Start() 
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = GameLoop;
        //musicSource.Play();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        #region GameManagement Events
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.AddHandler(GameEvent.OnCoinIncreaseSound,OnCoinIncreaseSound);
        EventManager.AddHandler(GameEvent.OnBuyButtonTap,OnBuyButtonTap);
        EventManager.AddHandler(GameEvent.OnUseButtonTap,OnUseButtonTap);
        #endregion

        //Player
        EventManager.AddHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
        EventManager.AddHandler(GameEvent.OnMatchFound,OnMatchFound);
        EventManager.AddHandler(GameEvent.OnDismatch,OnDismatch);
        EventManager.AddHandler(GameEvent.OnMatchFullPlayer,OnMatchFullPlayer);
        EventManager.AddHandler(GameEvent.OnPlayerWaitTooMuch,OnPlayerWaitTooMuch);
        EventManager.AddHandler(GameEvent.OnPlayerThanks,OnPlayerThanks);

        //Product
        EventManager.AddHandler(GameEvent.OnProductDragStart,OnProductDragStart);
        EventManager.AddHandler(GameEvent.OnProductDragStop,OnProductDragStop);
        EventManager.AddHandler(GameEvent.OnProductReset,OnProductReset);
        EventManager.AddHandler(GameEvent.OnProductPlaced,OnProductPlaced);

        //Helpers
        EventManager.AddHandler(GameEvent.OnCollectorMove,OnCollectorMove);

        //VIP
        EventManager.AddHandler(GameEvent.OnVipSummoned,OnVipSummoned);
        EventManager.AddHandler(GameEvent.OnVipLeave,OnVipLeave);
        EventManager.AddHandler(GameEvent.OnVipProductCreated,OnVipProductCreated);
        EventManager.AddHandler(GameEvent.OnVipProductPlaced,OnVipProductPlaced);
        EventManager.AddHandler(GameEvent.OnVipProductTouched,OnVipProductTouched);
        EventManager.AddHandler(GameEvent.OnVipSuccess,OnVipSuccess);
        EventManager.AddHandler(GameEvent.OnVipFail,OnVipFail);
        
    }
    private void OnDisable() 
    {
        #region GameManagement Events
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnCoinIncreaseSound,OnCoinIncreaseSound);
        EventManager.RemoveHandler(GameEvent.OnBuyButtonTap,OnBuyButtonTap);
        EventManager.RemoveHandler(GameEvent.OnUseButtonTap,OnUseButtonTap);
        #endregion

        //Player
        EventManager.RemoveHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
        EventManager.RemoveHandler(GameEvent.OnMatchFound,OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnDismatch,OnDismatch);
        EventManager.RemoveHandler(GameEvent.OnMatchFullPlayer,OnMatchFullPlayer);
        EventManager.RemoveHandler(GameEvent.OnPlayerWaitTooMuch,OnPlayerWaitTooMuch);
        EventManager.RemoveHandler(GameEvent.OnPlayerThanks,OnPlayerThanks);

        //Product
        EventManager.RemoveHandler(GameEvent.OnProductDragStart,OnProductDragStart);
        EventManager.RemoveHandler(GameEvent.OnProductDragStop,OnProductDragStop);
        EventManager.RemoveHandler(GameEvent.OnProductReset,OnProductReset);
        EventManager.RemoveHandler(GameEvent.OnProductPlaced,OnProductPlaced);

        //Helpers
        EventManager.RemoveHandler(GameEvent.OnCollectorMove,OnCollectorMove);

        //VIP
        EventManager.RemoveHandler(GameEvent.OnVipSummoned,OnVipSummoned);
        EventManager.RemoveHandler(GameEvent.OnVipLeave,OnVipLeave);
        EventManager.RemoveHandler(GameEvent.OnVipProductCreated,OnVipProductCreated);
        EventManager.RemoveHandler(GameEvent.OnVipProductPlaced,OnVipProductPlaced);
        EventManager.RemoveHandler(GameEvent.OnVipProductTouched,OnVipProductTouched);
        EventManager.RemoveHandler(GameEvent.OnVipSuccess,OnVipSuccess);
        EventManager.RemoveHandler(GameEvent.OnVipFail,OnVipFail);

    }

    

    #region GameManagement
    private void OnSuccess()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.SuccessSound);
    }

    private void OnSuccessUI()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.SuccessUISound);
    }


    private void OnRestartLevel()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.RestartSound);
    }

    private void OnNextLevel()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.NextLevelSound);
    }

    private void OnGameStart()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.StartSound);
    }

    private void OnFailUI()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.FailUISound);
    }

    private void OnCoinIncreaseSound()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsGameManagement.IncreaseScoreSound);
    }

    private void OnBuyButtonTap()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.HelperBuyButtonTapSound);
    }

    private void OnUseButtonTap()
    {
        effectSource.PlayOneShot(audioClipsGameManagement.HelperUseButtonTapSound);
    }

    #endregion
    
    #region Player
    private void OnPlayerStartMove()
    {
        effectSource.PlayOneShot(audioClipsPlayer.PlayersStartMoveSound);
    }
    
    private void OnMatchFound()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.MatchSound);
    }

    private void OnDismatch()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.DismatchSound);
    }

    private void OnMatchFullPlayer()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.CompletedMatchSound);
    }

    private void OnPlayerWaitTooMuch()
    {
        effectSource.PlayOneShot(audioClipsPlayer.PlayerWaitTooMuchSound);
    }

    private void OnPlayerThanks()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.PlayerThanksSound);
    }

    private void OnProductDragStart()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.ProductDragSound);
    }

    private void OnProductDragStop()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.ProductDropSound);
    }

    private void OnProductReset()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.ProductResetSound);
    }

    private void OnProductPlaced()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsPlayer.ProductPlacedSound);
    }

    #endregion

    #region Helpers

    private void OnCollectorMove()
    {
        effectSource.pitch=Random.Range(1,1.5f);
        effectSource.PlayOneShot(audioClipsHelpers.CollectorMoveSound);
    }

    #endregion

    #region VIP

    private void OnVipSummoned()
    {
        effectSource.PlayOneShot(audioClipsVIP.VipSummonSound);
    }

    private void OnVipLeave()
    {
        effectSource.PlayOneShot(audioClipsVIP.VipLeaveSound);
    }

    private void OnVipProductCreated()
    {
        effectSource.PlayOneShot(audioClipsVIP.VipArrivedSound);
    }   

    private void OnVipProductTouched()
    {
        effectSource.PlayOneShot(audioClipsVIP.VipProductHitSound);
    }

    private void OnVipProductPlaced()
    {
        effectSource.PlayOneShot(audioClipsVIP.VipProductPlacedSound);
    }

    private void OnVipSuccess()
    {
        int randomNumber=Random.Range(0,2);
        Debug.Log(randomNumber+"RN");
        if(randomNumber==0)
            effectSource.PlayOneShot(audioClipsVIP.VipSuccessManSound);
        else        
            effectSource.PlayOneShot(audioClipsVIP.VipSuccessFemaleSound);
    
    }

    private void OnVipFail()
    {
        int randomNumber=Random.Range(0,2);
        
        if(randomNumber==0)
            effectSource.PlayOneShot(audioClipsVIP.VipFailManSound);
        else        
            effectSource.PlayOneShot(audioClipsVIP.VipFailFemaleSound);
    }

    #endregion

}
