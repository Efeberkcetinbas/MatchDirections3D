using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
        EventManager.AddHandler(GameEvent.OnPlayerLeaving,OnPlayerLeaving);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
        EventManager.RemoveHandler(GameEvent.OnPlayerLeaving,OnPlayerLeaving);
    }

    private void OnPlayerStartMove()
    {
        animator.SetTrigger("DoorOpen");
    }

    private void OnPlayerLeaving()
    {
        animator.SetTrigger("DoorOpen");
    }
}
