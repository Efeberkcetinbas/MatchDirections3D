using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWait : MonoBehaviour,IPlayerWait
{
    private float waitTime;
    private float timer=0f;
    
    [SerializeField] private Image progressImage;
    [SerializeField] private PlayerWaitSettings playerWaitSettings1;
    
    [SerializeField] private GameData gameData;

    private Player player;


    private void Start()
    {
        player = GetComponent<Player>();
        ApplyWaitSettings(playerWaitSettings1);
    }   

    public void ApplyWaitSettings(PlayerWaitSettings playerWaitSettings)
    {
        waitTime=playerWaitSettings.WaitTime;
    }
    public void UpdateBehavior(float deltaTime)
    {
        timer += deltaTime;

        var val=timer/waitTime;
        progressImage.fillAmount=val;


        if (timer >= waitTime)
        {
            timer = 0f;
            WaitTooMuch();
        }
    }

    private void WaitTooMuch()
    {
        Debug.Log("WAITED SO LONG AND ANGRY");
        player.UnRegister=true;
        PlayerWaitManager.Instance.UnRegisterWaiter(this);
        EventManager.Broadcast(GameEvent.OnPlayerWaitTooMuch);
        
        
        gameData.dissatisfyPeople++;

        
    }


    
}
