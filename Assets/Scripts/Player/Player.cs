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


}
