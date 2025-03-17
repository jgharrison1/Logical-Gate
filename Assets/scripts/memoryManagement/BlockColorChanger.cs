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

    if (actualValue == targetValue)
    {
        objectRenderer.material.color = Color.yellow; // Correct match
        Debug.Log($"Block color changed to YELLOW (Match: {actualValue} == {targetValue})");
    }
    else
    {
        SetColorToBlue();
        Debug.Log($"Block color changed to BLUE (Mismatch: {actualValue} != {targetValue})");
    }
}


    // Helper method to set the color to blue
    private void SetColorToBlue()
    {
        objectRenderer.material.color = Color.blue; // Default to blue
    }
}
