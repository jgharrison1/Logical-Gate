// using UnityEngine;
// using System;

// public class BlockColorChanger : MonoBehaviour
// {
//     private Renderer objectRenderer;
//     private Material blockMaterial;

//     public event Action<BlockColorChanger, bool> OnColorChange; 

//     private void Awake()
//     {
//         objectRenderer = GetComponent<Renderer>();
//         if (objectRenderer != null)
//         {
//             blockMaterial = objectRenderer.material;
//         }
//         SetColorToBlue();
//     }

//     public void SetTargetValue(int actualValue, int targetValue)
//     {
//         if (blockMaterial == null) return;

//         bool isYellow = actualValue == targetValue;
//         blockMaterial.color = isYellow ? Color.yellow : Color.blue;

//         OnColorChange?.Invoke(this, isYellow);
//     }

//     private void SetColorToBlue()
//     {
//         if (blockMaterial != null)
//         {
//             blockMaterial.color = Color.blue;
//         }
//     }

//     public void TurnOn()
//     {
//         if (blockMaterial != null)
//         {
//             blockMaterial.color = Color.yellow; 
//         }
//     }

//     public void TurnOff()
//     {
//         SetColorToBlue();
//     }
// }

using UnityEngine;
using System;

public class BlockColorChanger : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material blockMaterial;

    public event Action<BlockColorChanger, bool> OnColorChange;

    private Color currentColor = Color.blue;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            blockMaterial = objectRenderer.material;
        }
        SetColorToBlue();
    }

public void SetTargetValue(int actualValue, int targetValue, bool forceUpdate = false)
{
    if (blockMaterial == null) return;

    bool isYellow = actualValue == targetValue;
    Color newColor = isYellow ? Color.yellow : Color.blue;
    Color currentColor = blockMaterial.color;

    if (blockMaterial.color != newColor)
    {
        blockMaterial.color = newColor;
    }

    // ✅ Trigger the event even if color is the same, when forceUpdate is true
    if (forceUpdate || blockMaterial.color != currentColor)
    {
        OnColorChange?.Invoke(this, isYellow);
    }
}

    private void SetColorToBlue()
    {
        if (blockMaterial != null)
        {
            blockMaterial.color = Color.blue;
            currentColor = Color.blue;
        }
    }

    public void TurnOn()
    {
        if (blockMaterial != null)
        {
            blockMaterial.color = Color.yellow;
            currentColor = Color.yellow;
        }
    }

    public void TurnOff()
    {
        SetColorToBlue();
    }
}
