using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
public class ProductTrigger : Obstacleable
{
    private ProductAttributes productAttributes;
    private DragManager dragManager;
    private ProductDrag productDrag;

    private bool matched=false;

    private void Start()
    {
        productAttributes = GetComponent<ProductAttributes>();
        dragManager=FindAnyObjectByType<DragManager>();
        productDrag=GetComponent<ProductDrag>();
    }

    public ProductTrigger()
    {
        canStay = false;
    }

    internal override void DoAction(PlayerAttributes player)
    {
        if (productAttributes == null || player == null)
        {
            Debug.LogWarning("Missing ProductAttributes or PlayerAttributes.");
            return;
        }

        bool isMatch = true;  // Assume match unless proven otherwise

        // Get all the types of attributes in the product and player
        var productTypes = productAttributes.GetTypes();  // Get as IEnumerable<System.Type>
        var playerTypes = player.GetTypes();  // Get as IEnumerable<System.Type>

        // Compare if the sets of attribute types are the same
        if (!HaveSameAttributeTypes(productTypes, playerTypes))
        {
            Debug.Log("Attribute type mismatch.");
            isMatch = false;
        }

        // Loop through all attribute types in product and player
        foreach (var productType in productTypes)
        {
            // Get the product attribute of this type
            var productAttribute = productAttributes.GetAttributeByType(productType);

            // Check if the player has the same type of attribute
            var playerAttribute = player.GetAttributeByType(productType);

            // If player doesn't have the same type of attribute, it's a mismatch
            if (playerAttribute == null)
            {
                Debug.LogWarning($"Player does not have an attribute of type {productType.Name}");
                isMatch = false;
                continue;  // Skip this type if player doesn't have it
            }

            // If both product and player have the attribute, compare their values
            if (productAttribute != null && playerAttribute != null)
            {
                if (productAttribute.GetType() == playerAttribute.GetType())
                {
                    // Compare their values dynamically using reflection
                    if (!AreAttributesEqual(productAttribute, playerAttribute))
                    {
                        Debug.Log($"Attribute value mismatch for {productType.Name}: Product = {productAttribute.name}, Player = {playerAttribute.name}");
                        isMatch = false;
                    }
                }
            }
        }

        if (isMatch && !matched)
        {
            Debug.Log("Match found.");
            
            //player.GetComponent<PlayerTrigger>().ProductParticle.Play();
            player.GetComponent<PeopleSelect>().peoples[player.GetComponent<PeopleSelect>().index].GetComponent<Animator>().SetTrigger("TrueProduct");
            player.GetComponent<PlayerTrigger>().ProductEnter.transform.DOPunchScale(Vector3.one,0.1f);
            transform.DORotate(player.GetComponent<PlayerTrigger>().ProductEnter.rotation.eulerAngles,.25f).OnComplete(()=>{
                //Add event for sound
            });
            transform.DOJump(player.GetComponent<PlayerTrigger>().ProductEnter.position,1,1,.5f).OnComplete(()=>{
                player.GetComponent<Player>().CoinUp();
                player.GetComponent<Player>().IncreaseProductNumber();
                EventManager.Broadcast(GameEvent.OnMatchFound);
                transform.gameObject.SetActive(false);
            });
            productDrag.IsPlaced=true;
            //Increase Satisfaction Bar
            dragManager.CurrentProduct=null;
            matched=true;
        }
        if(!isMatch)
        {
            Debug.Log("No match found.");
            player.GetComponent<PeopleSelect>().peoples[player.GetComponent<PeopleSelect>().index].GetComponent<Animator>().SetTrigger("FalseProduct");
            productDrag.Reset();
            dragManager.CurrentProduct=null;
            //Decrease Satisfaction Bar
            EventManager.Broadcast(GameEvent.OnDismatch);
        }
    }

    // Compare the sets of attribute types between the product and player
    private bool HaveSameAttributeTypes(IEnumerable<System.Type> productTypes, IEnumerable<System.Type> playerTypes)
    {
        // If the number of attributes is different, return false
        if (productTypes.Count() != playerTypes.Count())
            return false;

        // Compare each type to ensure both product and player have the same types
        foreach (var productType in productTypes)
        {
            if (!playerTypes.Contains(productType))
            {
                return false;  // If player doesn't have this type, return false
            }
        }

        return true;  // If no mismatches, return true
    }

    private bool AreAttributesEqual(EnumAttribute productAttribute, EnumAttribute playerAttribute)
    {
        // Use reflection to compare the values of the two attributes (works for any attribute type)
        var productValue = productAttribute.GetType().GetField("value").GetValue(productAttribute);
        var playerValue = playerAttribute.GetType().GetField("value").GetValue(playerAttribute);

        return productValue.Equals(playerValue);
    }

    private void OnRestart()
    {
        matched=false;
    }
}
