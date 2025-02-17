using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWait : MonoBehaviour,IPlayerWait
{
    private float waitTime;
    private float timer=0f;
    
    [SerializeField] private Gradient progressGradient;
    [SerializeField] private Image progressImage,normalImage;
    [SerializeField] private GameData gameData;
    [SerializeField] private ParticleSystem warningParticle;
    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();
        InitializeColorGradientList();
    }   

    internal void SetActivityProgress(bool val)
    {
        progressImage.gameObject.SetActive(val);
        normalImage.gameObject.SetActive(val);
    }

    internal void ResetTimer()
    {
        timer=0;
    }
    public void ApplyWaitSettings(PlayerWaitSettings playerWaitSettings)
    {
        waitTime=playerWaitSettings.WaitTime;
    }
    public void UpdateBehavior(float deltaTime)
    {
        timer += deltaTime;

        var val=timer/waitTime;
        progressImage.DOFillAmount(val,0.1f);
        normalImage.color=GetColorForProgress(val);

        if (timer >= waitTime)
        {
            timer = 0f;
            WaitTooMuch();
        }
    }

    private void WaitTooMuch()
    {
        Debug.Log("WAITED SO LONG AND ANGRY");
        player.Unregister=true;
        warningParticle.Play();
        PlayerWaitManager.Instance.UnRegisterWaiter(this);
        EventManager.Broadcast(GameEvent.OnPlayerWaitTooMuch);
        
        
        gameData.dissatisfyPeople++;

        
    }

    private void InitializeColorGradientList()
    {
        progressGradient = new Gradient
        {
            colorKeys = new[]
            {
                new GradientColorKey(Color.green, 0f),   // Start at 0
                new GradientColorKey(Color.yellow, 0.33f), // Yellow
                new GradientColorKey(new Color(1f, 0.5f, 0f), 0.66f), // Orange
                new GradientColorKey(Color.red, 1f)  // End at 1
            }
        };
    }        
    private Color GetColorForProgress(float progress)
    {
        return progressGradient.Evaluate(progress);
    }


    
}
