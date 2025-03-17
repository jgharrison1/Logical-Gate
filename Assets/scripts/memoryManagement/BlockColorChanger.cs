using UnityEngine;

public class BlockColorChanger : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material blockMaterial; 

    private void Awake() 
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            blockMaterial = objectRenderer.material;
        }
        SetColorToBlue();
    }

    public void SetTargetValue(int actualValue, int targetValue)
    {
        if (blockMaterial == null) return;

        if (actualValue == targetValue)
        {
            blockMaterial.color = Color.yellow;
        }
        else
        {
            SetColorToBlue();
        }
    }

    private void SetColorToBlue()
    {
        if (blockMaterial != null)
        {
            blockMaterial.color = Color.blue;
        }
    }
}
