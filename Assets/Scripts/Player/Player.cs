using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class Player : MonoBehaviour
{
   
    private PlayerWait playerWait;
    private int randomIndex;
    private int responseIndex;
    private int coincounter;
    private WaitForSeconds waitForSeconds;
    private Renderer successRenderer;
    


    [SerializeField] private GameObject xP;
    [SerializeField] private Transform xPSpawnPos;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 doorPos;
    [SerializeField] private GameData gameData;
    [SerializeField] private CollectCoin collectCoin;
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem collectParticle;
    [SerializeField] private List<Material> particleMaterials=new List<Material>();
    [SerializeField] private List<ParticleSystem> matchFullParticles=new List<ParticleSystem>();
    

    internal PeopleSelect peopleSelect;
    internal Destination destination;
    
    public int requirementProduct;
    public int productNumber;

    public Vector3 NewScale=Vector3.one;
    public Mesh placeholderMesh;
    public Material mat;
    public TextMeshPro counterText;

    public bool Unregister=false;
    public bool Full=false;

    private void Awake()
    {
        peopleSelect = GetComponent<PeopleSelect>();
        playerWait=GetComponent<PlayerWait>();
        waitForSeconds=new WaitForSeconds(1f);
        successRenderer=successParticle.GetComponent<Renderer>();
        
    }

    private void Start()
    {
        ResetParticles();
    }

    private void ResetParticles()
    {
        for (int i = 0; i < matchFullParticles.Count; i++)
        {
            matchFullParticles[i].Stop();
            matchFullParticles[i].gameObject.SetActive(false);
        }
    }

   

    
    //Set it to Money UI or Moneycase. 
    internal void CoinUp()
    {
        GameObject xPeffect=Instantiate(xP,xPSpawnPos.position,xP.transform.rotation);
        xPeffect.transform.DOLocalJump(xPSpawnPos.position,1,1,1,false);
        xPeffect.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("+5");
        xPeffect.transform.GetChild(0).GetComponent<TextMeshPro>().DOFade(0,2f).OnComplete(()=>{
            Destroy(xPeffect);
        });

    }



    internal void CheckAllMatch()
    {
        PlaySuccessParticle();

        if(requirementProduct==productNumber)
        {
            Debug.Log("FULL");
            Full=true;
            /*destination.ResetDestination();
            destination=null;*/
            GetRandomFullParticle();
            coincounter=Mathf.Max(1,gameData.comboCount);
            gameData.increaseScore=coincounter;
            playerWait.SetActivityProgress(false);
            peopleSelect.peoples[peopleSelect.index].GetComponent<Animator>().SetTrigger("Thanks");
            collectCoin.StartCollectCoin(coincounter);
            EventManager.Broadcast(GameEvent.OnPlayerThanks);
            StartCoroutine(OnSuccessCompleted());
            
        }
    }

    private void GetRandomFullParticle()
    {
        responseIndex=Random.Range(0,matchFullParticles.Count);
        matchFullParticles[responseIndex].gameObject.SetActive(true);
        matchFullParticles[responseIndex].Play();
    }

    

    private IEnumerator OnSuccessCompleted()
    {
        yield return waitForSeconds;
        destination.ResetDestination();
        destination=null;
        
        EventManager.Broadcast(GameEvent.OnMatchFullPlayer);
        if(!Unregister)
                PlayerWaitManager.Instance.UnRegisterWaiter(GetComponent<PlayerWait>());
        transform.Rotate(0, 180, 0);
        EventManager.Broadcast(GameEvent.OnPlayerLeaving);

        
        
        transform.DOMove(doorPos,1).OnComplete(()=>{
            ResetParticles();
            transform.DOMove(startPos,1).OnComplete(()=>{
                responseIndex=Random.Range(0,matchFullParticles.Count);
                gameObject.SetActive(false);
            });
        });
        
        peopleSelect.peoples[peopleSelect.index].GetComponent<Animator>().SetTrigger("Completed");

            


    }

    internal void Reset()
    {
        coincounter=0;
        productNumber=0;
        counterText.SetText(productNumber.ToString() + " / " + requirementProduct.ToString());
    }

    internal void UpdateCounterText()
    {
        counterText.SetText(productNumber.ToString() + " / " + requirementProduct.ToString());
    }

    internal void IncreaseProductNumber()
    {
        productNumber++;
        counterText.SetText(productNumber.ToString() + " / " + requirementProduct.ToString());
        CheckAllMatch();
    }

    internal void SetCollectParticle()
    {
        collectParticle.Play();
    }
    

    private void GetRandomIndex()
    {
        randomIndex=Random.Range(0,particleMaterials.Count);
    }

    private void PlaySuccessParticle()
    {
        GetRandomIndex();
        successRenderer.material=particleMaterials[randomIndex];
        successParticle.Play();
    }


    
    

    


}
