using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player[] players; // List of players in the scene
    [SerializeField] private Transform[] destinationTargets; // Predefined destination points (max 3)
    [SerializeField] private MovementOrderConfig movementOrderConfig; // Movement order for this level

    private Queue<Player> playerQueue = new Queue<Player>(); // Queue to manage players
    private int currentMovementIndex = 0; // Tracks the current movement step
    private int counter;
    private int index;

    private void Start()
    {
        InitializePlayerQueue();
        AssignAttributes();
        OnNextPlayer();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
    }

    private void InitializePlayerQueue()
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(false); // Deactivate all players at the start
            playerQueue.Enqueue(player);
        }
    }

    private void OnMatchFullPlayer()
    {
        counter++;
        if(counter==index)
        {
            counter=0;
            index=0;
            OnNextPlayer();
        }
        
    }

    private void AssignAttributes()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i < movementOrderConfig.playerAttributes.Length)
            {
                var playerAttributes = players[i].GetComponent<PlayerAttributes>();
                var configuration = movementOrderConfig.playerAttributes[i];

                foreach (var attribute in configuration.attributes)
                {
                    playerAttributes.AddAttribute(attribute);
                }
            }
            else
            {
                Debug.LogWarning($"No configuration assigned for Player {i + 1}");
            }
        }
    }

    public void OnNextPlayer()
    {
        if (currentMovementIndex >= movementOrderConfig.movementOrder.Count)
        {
            Debug.Log("All players have moved according to the movement order.");
            return;
        }

        int playersToMove = movementOrderConfig.movementOrder[currentMovementIndex];
        currentMovementIndex++;

        MovePlayersToDestinations(playersToMove);
    }

    private void MovePlayersToDestinations(int playersToMove)
    {
        if (playersToMove > destinationTargets.Length)
        {
            Debug.LogWarning($"Cannot assign {playersToMove} players; only {destinationTargets.Length} targets available!");
            playersToMove = destinationTargets.Length;
        }

        List<Transform> availableDestinations = new List<Transform>(destinationTargets);

        for (int i = 0; i < playersToMove; i++)
        {
            if (playerQueue.Count > 0 && availableDestinations.Count > 0)
            {
                var player = playerQueue.Dequeue();
                player.gameObject.SetActive(true);

                // Assign a random destination from the available ones
                int randomIndex = Random.Range(0, availableDestinations.Count);
                Transform target = availableDestinations[randomIndex];
                availableDestinations.RemoveAt(randomIndex);

                MovePlayerToDestination(player, target.position);
            }
            else
            {
                Debug.LogWarning("Not enough players in the queue or destinations available!");
                break;
            }
        }
    }

    private void MovePlayerToDestination(Player player, Vector3 destination)
    {
        // Replace with your preferred movement logic
        Debug.Log($"{player.name} moving to {destination}");
        player.transform.position = destination;
        PlayerWaitManager.Instance.RegisterWaiter(player.GetComponent<PlayerWait>());
        index++;
        Debug.Log("INDEX " + index);
    }
}
