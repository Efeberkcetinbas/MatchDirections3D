using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class Player : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    private PlayerWait playerWait;
    
    


    [SerializeField] private GameObject xP;
    [SerializeField] private Transform xPSpawnPos;
    [SerializeField] private int productNumber;
    [SerializeField] private Vector3 startPos;

    internal PeopleSelect peopleSelect;
    internal Destination destination;
    
    public int requirementProduct;
    public Mesh placeholderMesh;
    public Material mat;
    public TextMeshPro counterText;

    public bool UnRegister=false;

    private void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
        peopleSelect = GetComponent<PeopleSelect>();
        playerWait=GetComponent<PlayerWait>();

        
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
            EventManager.Broadcast(GameEvent.OnMatchFullPlayer);
            destination.ResetDestination();
            destination=null;
            if(!UnRegister)
                PlayerWaitManager.Instance.UnRegisterWaiter(GetComponent<PlayerWait>());
            //Turn back 
            transform.Rotate(0, 180, 0);
            EventManager.Broadcast(GameEvent.OnPlayerLeaving);
            transform.DOMove(startPos,2).OnComplete(()=>{
                gameObject.SetActive(false);
            });
            playerWait.SetActivityProgress(false);
            peopleSelect.peoples[peopleSelect.index].GetComponent<Animator>().SetTrigger("Completed");
            
        }
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

    private void OnRestart()
    {
        productNumber=0;
        counterText.SetText(productNumber.ToString() + " / " + requirementProduct.ToString());
    }


    




}
