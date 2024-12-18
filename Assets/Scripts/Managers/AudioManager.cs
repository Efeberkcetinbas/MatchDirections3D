using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioClipsGameManagement
{
    public AudioClip SuccessSound;
    public AudioClip SuccessUISound;
    public AudioClip RestartSound;
    public AudioClip NextLevelSound;
    public AudioClip StartSound;
    public AudioClip FailUISound;
    
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
}
public class AudioManager : MonoBehaviour
{
    public AudioClip GameLoop,BuffMusic;

    [Header("Game Management")]
    public AudioClipsGameManagement audioClipsGameManagement;
    [Header("Player")]
    public AudioClipsPlayer audioClipsPlayer;
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
        #endregion

        //Player
        EventManager.AddHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
        EventManager.AddHandler(GameEvent.OnMatchFound,OnMatchFound);
        EventManager.AddHandler(GameEvent.OnDismatch,OnDismatch);
        EventManager.AddHandler(GameEvent.OnMatchFullPlayer,OnMatchFullPlayer);
        EventManager.AddHandler(GameEvent.OnPlayerWaitTooMuch,OnPlayerWaitTooMuch);

        //Product
        EventManager.AddHandler(GameEvent.OnProductDragStart,OnProductDragStart);
        EventManager.AddHandler(GameEvent.OnProductDragStop,OnProductDragStop);
        EventManager.AddHandler(GameEvent.OnProductReset,OnProductReset);
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
        #endregion

        //Player
        EventManager.RemoveHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
        EventManager.RemoveHandler(GameEvent.OnMatchFound,OnMatchFound);
        EventManager.RemoveHandler(GameEvent.OnDismatch,OnDismatch);
        EventManager.RemoveHandler(GameEvent.OnMatchFullPlayer,OnMatchFullPlayer);
        EventManager.RemoveHandler(GameEvent.OnPlayerWaitTooMuch,OnPlayerWaitTooMuch);

        //Product
        EventManager.RemoveHandler(GameEvent.OnProductDragStart,OnProductDragStart);
        EventManager.RemoveHandler(GameEvent.OnProductDragStop,OnProductDragStop);
        EventManager.RemoveHandler(GameEvent.OnProductReset,OnProductReset);

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

    #endregion
  
    private void OnPlayerStartMove()
    {
        effectSource.PlayOneShot(audioClipsPlayer.PlayersStartMoveSound);
    }
    
    private void OnMatchFound()
    {
        effectSource.PlayOneShot(audioClipsPlayer.MatchSound);
    }

    private void OnDismatch()
    {
        effectSource.PlayOneShot(audioClipsPlayer.DismatchSound);
    }

    private void OnMatchFullPlayer()
    {
        effectSource.PlayOneShot(audioClipsPlayer.CompletedMatchSound);
    }

    private void OnPlayerWaitTooMuch()
    {
        effectSource.PlayOneShot(audioClipsPlayer.PlayerWaitTooMuchSound);
    }

    private void OnProductDragStart()
    {
        effectSource.PlayOneShot(audioClipsPlayer.ProductDragSound);
    }

    private void OnProductDragStop()
    {
        effectSource.PlayOneShot(audioClipsPlayer.ProductDropSound);
    }

    private void OnProductReset()
    {
        effectSource.PlayOneShot(audioClipsPlayer.ProductResetSound);
    }

}
