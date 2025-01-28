using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class fps : MonoBehaviour
{
    public TextMeshProUGUI display; // UI Text element to display the FPS
    private float deltaTime = 0.0f;

    void Update()
    {
        //float frame_dur = Time.unscaledDeltaTime;
        // Smooth deltaTime for FPS calculation
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / deltaTime;

        // Update the text
        display.text = $"{Mathf.Ceil(fps)} FPS";
    }
}
