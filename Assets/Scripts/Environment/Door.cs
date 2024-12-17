using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();   
    }
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerStartMove,OnPlayerStartMove);
    }

    private void OnPlayerStartMove()
    {
        animator.SetTrigger("DoorOpen");
    }
}
