using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player[] players; // List of players in the scene
    [SerializeField] private Transform[] destinationTargets; // Predefined destination points (max 3)
    [SerializeField] private MovementOrderConfig movementOrderConfig; // Movement order for this level
    [SerializeField] private Transform playerInitialPos;
    [SerializeField] private GameData gameData;

    private Queue<Player> playerQueue = new Queue<Player>(); // Queue to manage players
    private int currentMovementIndex = 0; // Tracks the current movement step
    private int counter;
    private int index;

   

    
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
        EventManager.AddHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatchFullPlayer, OnMatchFullPlayer);
        EventManager.RemoveHandler(GameEvent.OnGameStart,OnGameStart);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);
    }

    private void OnGameStart()
    {
        InitializePlayerQueue();
        AssignAttributes();
        OnNextPlayer();    
        LogQueueContents();
    }

    

    private void InitializePlayerQueue()
    {
        foreach (var player in players)
        {
            player.Unregister=false;
            player.Full=false;
            player.gameObject.SetActive(false); // Deactivate all players at the start
            playerQueue.Enqueue(player);
        }
    }

    private void OnMatchFullPlayer()
    {
        ControlNextPlayer();
    }

    private void ControlNextPlayer()
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
            players[i].GetComponent<PlayerAttributes>().CleanAttributes();
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
            EventManager.Broadcast(GameEvent.OnSuccess);
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
        EventManager.Broadcast(GameEvent.OnPlayerStartMove);
        List<Transform> availableDestinations = new List<Transform>(destinationTargets);
        
        

        for (int i = 0; i < playersToMove; i++)
        {
            if (playerQueue.Count > 0 && availableDestinations.Count > 0)
            {
                var player = playerQueue.Dequeue();
                player.gameObject.SetActive(true);

                //ResetPlayer
                player.GetComponent<PeopleSelect>().SelectPeople();
                player.GetComponent<PeopleLevel>().OnAssignPeople();
                player.GetComponent<PlayerWait>().ResetTimer();
                player.transform.rotation=Quaternion.Euler(0f, 0f, 0f);

                // Assign a random destination from the available ones
                int randomIndex = Random.Range(0, availableDestinations.Count);
                Transform target = availableDestinations[randomIndex];
                availableDestinations.RemoveAt(randomIndex);

                MovePlayerToDestination(player, target);
            }
            else
            {
                Debug.LogWarning("Not enough players in the queue or destinations available!");
                break;
            }
        }


    }

    private void MovePlayerToDestination(Player player, Transform destination)
    {
        // Replace with your preferred movement logic
        Debug.Log($"{player.name} moving to {destination}");
        player.transform.position=playerInitialPos.position;
        player.transform.DOMove(destination.position,1f).OnComplete(()=>{
            destination.GetComponent<Destination>().meshRenderer.transform.localScale=player.NewScale;
            destination.GetComponent<Destination>().meshFilter.mesh=player.placeholderMesh;
            destination.GetComponent<Destination>().ProductParticle.Play();
            destination.GetComponent<Destination>().meshRenderer.material=player.mat;
            player.counterText=destination.GetComponent<Destination>().CounterText;
            player.GetComponent<Player>().Reset();
            player.GetComponent<PlayerWait>().SetActivityProgress(true);
            player.UpdateCounterText();
            player.GetComponent<PlayerTrigger>().ProductEnter=destination.GetComponent<Destination>().ProductEnter;
            player.peopleSelect.peoples[player.peopleSelect.index].GetComponent<Animator>().SetTrigger("Hold");
            player.destination=destination.GetComponent<Destination>();
            PlayerWaitManager.Instance.RegisterWaiter(player.GetComponent<PlayerWait>());
            EventManager.Broadcast(GameEvent.OnDestinationProduct);
        });
        
        //VIP 
        gameData.vipArriveNumber++;
        if(gameData.vipArriveNumber==gameData.getVipRandomNumber)
        {
            EventManager.Broadcast(GameEvent.OnVipOnTheWay);
            gameData.vipArriveNumber=0;
        }

        index++;
        Debug.Log("INDEX " + index);
    }

    private void OnRestartLevel()
    {
        currentMovementIndex=0;
        counter=0;
        index=0;
        playerQueue.Clear();

        InitializePlayerQueue();
        AssignAttributes();
    }

    private void LogQueueContents()
    {
        Debug.Log($"Queue size: {playerQueue.Count}");
        foreach (var player in playerQueue)
        {
            Debug.Log($"Player: {player.name}");
        }
    }
}
