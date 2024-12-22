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
        EventManager.AddHandler(GameEvent.OnCoinIncreaseSound,OnCoinIncreaseSound);
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

    #endregion
  
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

}
