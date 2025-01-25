using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class fps : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay; // UI Text element to display the FPS
    private float deltaTime = 0.0f;

    void Update()
    {
        // Smooth deltaTime for FPS calculation
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / deltaTime;

        // Update the text
        fpsDisplay.text = $"{Mathf.Ceil(fps)} FPS";
    }
}
