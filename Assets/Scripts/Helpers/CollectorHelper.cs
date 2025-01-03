using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CollectorHelper : MonoBehaviour
{
    [SerializeField]
    private float totalMoveTime = 10f; // Total time for all products to move, adjustable in Inspector

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCollector,OnCollector);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCollector,OnCollector);
    }

    private void OnCollector()
    {
        // Start the Coroutine to move the products
        StartCoroutine(MoveProductsCoroutine());
    }

    
    private IEnumerator MoveProductsCoroutine()
    {
        // Find all active products in the scene
        var products = FindObjectsOfType<ProductAttributes>();

        if (products.Length == 0)
        {
            Debug.LogWarning("No active products found in the scene.");
            yield break;
        }

        // Find all active players in the scene
        var players = FindObjectsOfType<PlayerAttributes>();
        PlayerAttributes selectedPlayer = null;

        // Loop through players and find one with valid attributes
        foreach (var player in players)
        {
            if (player.GetTypes().Count() > 0) // Check if player has attributes
            {
                selectedPlayer = player;
                selectedPlayer.SetCollectAmount(); // Set the collection amount
                break;
            }
        }

        if (selectedPlayer == null)
        {
            Debug.LogWarning("No player with valid attributes found.");
            yield break;
        }

        // Debug: Log selected player's attributes to verify
        Debug.Log($"Selected Player Attributes:");
        foreach (var type in selectedPlayer.GetTypes())
        {
            var playerAttribute = selectedPlayer.GetAttributeByType(type);
            //Debug.Log($"Type: {type.Name}, Value: {playerAttribute}");
        }

        // Get the player's position to move products to
        Vector3 targetPosition = selectedPlayer.TargetPos.position;

        // Calculate the total number of products and the time for each product to move
        float timePerProduct = totalMoveTime / selectedPlayer.CollectAmount; // Time for each product to move

        // Now, move the specified number of products that have matching attributes as the selected player
        int productsMoved = 0;
        foreach (var product in products)
        {
            bool isMatch = true;

            // Check if all attributes of the selected player match with the product
            foreach (var type in selectedPlayer.GetTypes())
            {
                var playerAttribute = selectedPlayer.GetAttributeByType(type);
                var productAttribute = product.GetAttributeByType(type);

                // If the product doesn't have the same attribute or the values don't match, break
                if (playerAttribute == null || productAttribute == null || !playerAttribute.Equals(productAttribute))
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                // Only move the products up to the player's CollectAmount
                if (productsMoved >= selectedPlayer.CollectAmount)
                    break;

                // Debug: Log where the product starts moving from
                product.SetProductCollected();

                // Move product to the target position using the jump
                yield return StartCoroutine(MoveToTargetPositionWithJump(product, targetPosition, timePerProduct));

                productsMoved++;
            }
        }

        EventManager.Broadcast(GameEvent.OnCollectorEnd);
        Debug.LogWarning("DURAN EMMI GELDI DIYIN");

        // If not enough products match the player's attributes
        if (productsMoved < selectedPlayer.CollectAmount)
        {
            Debug.LogWarning("Not enough products found to match the player's attributes.");
        }
    }

    // Coroutine to move the product to the target position using a jump
    private IEnumerator MoveToTargetPositionWithJump(ProductAttributes product, Vector3 targetPosition, float duration)
    {
        // Calculate the height of the jump for the Y movement (you can adjust this value)
        float jumpHeight = 5f;
        product.GetComponent<ProductDrag>().CollectedByCollector();
        // Move product using DOJump: target position, jump height, number of jumps, and duration
        product.transform.DOJump(targetPosition, jumpHeight, 1, duration / 2);

        //Debug.Log($"Product {product.name} started moving to target position with jump");
        EventManager.Broadcast(GameEvent.OnCollectorMove);

        // Wait for the jump to complete before finishing
        yield return new WaitForSeconds(duration / 2); // Duration of jump movement

        //Debug.Log($"Product {product.name} reached the target with jump");
    }
}
