using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacleable : MonoBehaviour
{
    float st = 0;
    internal float interval = 3;
    internal bool canStay=true;
    internal bool canInteract = true;
    internal bool canDamageToPlayer=true;
    internal string interactionTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (!canInteract) return;
        if (other.tag == interactionTag)
        {
            StartInteractWithEnemy(other.GetComponent<PlayerTrigger>());
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (!canInteract) return;
        if (other.tag == interactionTag)
        {
            InteractWithEnemy(other.GetComponent<PlayerTrigger>());
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == interactionTag)
        {
            StopInteractWithEnemy(other.GetComponent<PlayerTrigger>());
        }
    }

    void StartInteractWithEnemy(PlayerTrigger player)
    {
        DoAction(player);
    }

    void StopInteractWithEnemy(PlayerTrigger player)
    {
        StopAction(player);
    }

    void InteractWithEnemy(PlayerTrigger player)
    {
        st += Time.deltaTime;
        if (st > interval && canStay)
        {
            ResetProgress();
            DoAction(player);
        }
    }
    internal virtual void ResetProgress()
    {
        st = 0;
    }
    
    internal virtual void DoAction(PlayerTrigger player)
    {
        throw new System.NotImplementedException();
    }

    internal virtual void StopAction(PlayerTrigger player)
    {
        st = 0;
    }
    internal void StopInteract()
    {
        canInteract = false;
    }
    internal void StartInteract()
    {
        canInteract = true;
    }
}
