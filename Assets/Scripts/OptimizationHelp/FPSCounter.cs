using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For displaying text with TextMeshPro
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText; // Reference to the TextMeshProUGUI component
    [SerializeField] private float updateInterval = 0.5f; // Time interval for updating the FPS display

    private float elapsedTime = 0f;
    private int frameCount = 0;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        frameCount++;

        // Update FPS display at regular intervals
        if (elapsedTime >= updateInterval)
        {
            float fps = frameCount / elapsedTime;
            fpsText.text = $"FPS: {fps:F2}";

            // Reset counters
            elapsedTime = 0f;
            frameCount = 0;
        }
    }
}
