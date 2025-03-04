using UnityEngine;

public class BlockColorChanger : MonoBehaviour
{
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        SetColorToBlue(); // Set the initial color to blue when the game starts
    }

    public void SetTargetValue(int actualValue, int targetValue)
    {
        if (objectRenderer == null) return;

        // Compare the sum of actualValue and targetValue
        if (actualValue == targetValue)
        {
            objectRenderer.material.color = Color.yellow; // Correct match
        }
        else
        {
            SetColorToBlue(); // Set to blue when values don't match
        }
    }

    // Helper method to set the color to blue
    private void SetColorToBlue()
    {
        objectRenderer.material.color = Color.blue; // Default to blue
    }
}
