using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private PlayerAttributes playerAttributes;

    [SerializeField] private GameObject xP;
    [SerializeField] private Transform xPSpawnPos;
    [SerializeField] private int requirementProduct;
    [SerializeField] private int productNumber;

    private void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
    }

    public void Initialize()
    {
        // Example: Access ColorEnumAttribute
        var colorAttribute = playerAttributes.GetAttribute<ColorEnumAttribute>();
        if (colorAttribute != null)
        {
            Debug.Log($"Player's color is {colorAttribute.value}");
        }

        // Example: Access DirectionEnumAttribute
        var directionAttribute = playerAttributes.GetAttribute<DirectionEnumAttribute>();
        if (directionAttribute != null)
        {
            Debug.Log($"Player's direction is {directionAttribute.value}");
        }
    }

    //Set it to Money UI or Moneycase. Whereever you want!
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
            PlayerWaitManager.Instance.UnRegisterWaiter(GetComponent<PlayerWait>());
            //Turn back
            gameObject.SetActive(false);
        }
    }

    internal void IncreaseProductNumber()
    {
        productNumber++;
        CheckAllMatch();
    }




}
