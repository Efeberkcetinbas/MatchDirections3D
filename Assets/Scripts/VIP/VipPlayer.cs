using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipPlayer : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    [SerializeField] private ParticleSystem arrivedParticle,pullParticle;

    internal void PlayParticle()
    {
        arrivedParticle.Play();
    }

    internal void PullParticle()
    {
        pullParticle.Play();
    }
}
