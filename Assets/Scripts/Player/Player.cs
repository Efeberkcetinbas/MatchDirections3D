using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class Player : MonoBehaviour
{
   
    private PlayerWait playerWait;
    private int randomIndex;
    private WaitForSeconds waitForSeconds;
    


    [SerializeField] private GameObject xP;
    [SerializeField] private Transform xPSpawnPos;
    [SerializeField] private int productNumber;
    [SerializeField] private Vector3 startPos;

    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private List<Material> particleMaterials=new List<Material>();
    

    internal PeopleSelect peopleSelect;
    internal Destination destination;
    
    public int requirementProduct;
    public Vector3 NewScale=Vector3.one;
    public Mesh placeholderMesh;
    public Material mat;
    public TextMeshPro counterText;

    public bool Unregister=false;

    private void Awake()
    {
        peopleSelect = GetComponent<PeopleSelect>();
        playerWait=GetComponent<PlayerWait>();
        waitForSeconds=new WaitForSeconds(2f);

        
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
        if(requirementProduct==productNumber)
        {
            Debug.Log("FULL");
            destination.ResetDestination();
            destination=null;
            GetRandomMaterialForParticle();
            playerWait.SetActivityProgress(false);
            peopleSelect.peoples[peopleSelect.index].GetComponent<Animator>().SetTrigger("Thanks");
            StartCoroutine(OnSuccessCompleted());
            
        }
    }

    private IEnumerator OnSuccessCompleted()
    {
        yield return waitForSeconds;
        EventManager.Broadcast(GameEvent.OnMatchFullPlayer);
        if(!Unregister)
                PlayerWaitManager.Instance.UnRegisterWaiter(GetComponent<PlayerWait>());
        transform.Rotate(0, 180, 0);
        EventManager.Broadcast(GameEvent.OnPlayerLeaving);
        transform.DOMove(startPos,2).OnComplete(()=>{
                gameObject.SetActive(false);
            });
        peopleSelect.peoples[peopleSelect.index].GetComponent<Animator>().SetTrigger("Completed");

            


    }

    internal void Reset()
    {
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

    

    private void GetRandomIndex()
    {
        randomIndex=Random.Range(0,particleMaterials.Count);
    }

    private void GetRandomMaterialForParticle()
    {
        GetRandomIndex();
        var Renderer=successParticle.GetComponent<Renderer>();
        Renderer.material=particleMaterials[randomIndex];
        successParticle.Play();
    }


    
    private void OnRestart()
    {
        productNumber=0;
        counterText.SetText(productNumber.ToString() + " / " + requirementProduct.ToString());
    }



}
