using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitManager : MonoBehaviour
{
    public static PlayerWaitManager Instance { get; private set; }

    [SerializeField] private GameData gameData;

    private readonly List<IPlayerWait> players = new();
    private readonly List<IPlayerWait> playersToRemove = new(); // Temporary list to track players to remove

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if(!gameData.isFreezer)
        {
            foreach (var player in players)
            {
                player.UpdateBehavior(Time.deltaTime);
            }
        }
        

        // Remove players after iteration
        if (playersToRemove.Count > 0)
        {
            foreach (var player in playersToRemove)
            {
                players.Remove(player);
            }
            playersToRemove.Clear();
        }
    }

    public void RegisterWaiter(IPlayerWait player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void UnRegisterWaiter(IPlayerWait player)
    {
        if (players.Contains(player))
        {
            playersToRemove.Add(player); // Mark the player for removal
        }
    }

    private void OnRestart()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playersToRemove.Add(players[i]);
        }

        //OnRestartPlayers
    }

    
}
