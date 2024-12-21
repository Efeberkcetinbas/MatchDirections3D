using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private Transform door1,door2;
    [SerializeField] private float door1X,door2X,duration;
    [SerializeField] private Ease ease;
    [SerializeField] private float door1Initial,door2Initial;
    

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
        MoveAndReturnPosition();
        //animator.SetTrigger("DoorOpen");
    }

    private void OnPlayerLeaving()
    {
        MoveAndReturnPosition();
        //animator.SetTrigger("DoorOpen");
    }

    private void MoveAndReturnPosition()
    {
        Sequence sequence=DOTween.Sequence();

        sequence.Append(door1.DOLocalMoveX(door1X,duration).SetEase(ease));
        sequence.Join(door2.DOLocalMoveX(door2X,duration).SetEase(ease));

        sequence.Append(door1.DOLocalMoveX(door1Initial,duration).SetEase(ease));
        sequence.Join(door2.DOLocalMoveX(door2Initial,duration).SetEase(ease));
    }
}
