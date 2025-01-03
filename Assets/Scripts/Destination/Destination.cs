using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Destination : MonoBehaviour
{
    public TextMeshPro CounterText;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Transform ProductEnter;
    public ParticleSystem ProductParticle;
    
    public Vector3 initialScale;

    [SerializeField] private Transform placeholderkeeper;
    private void Awake()
    {
        ResetText();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnRestartLevel()
    {
        ResetDestination();
    }

    internal void ResetDestination()
    {
        ResetText();
        meshRenderer.material=null;
        meshFilter.mesh=null;
        transform.localScale=initialScale;
    }

    internal void SetScaleEffect()
    {
        placeholderkeeper.localScale=Vector3.zero;
        placeholderkeeper.DOScale(Vector3.one,0.25f).SetEase(Ease.OutQuart);
    }

    private void ResetText()
    {
        CounterText.SetText("0/0");
        initialScale=transform.localScale;
    }
    
}
