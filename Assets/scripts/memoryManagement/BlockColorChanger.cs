// using UnityEngine;

// public class BlockColorChanger : MonoBehaviour
// {
//     private Renderer objectRenderer;
//     private Material blockMaterial; 

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

//         if (actualValue == targetValue)
//         {
//             blockMaterial.color = Color.yellow;
//         }
//         else
//         {
//             SetColorToBlue();
//         }
//     }

//     private void SetColorToBlue()
//     {
//         if (blockMaterial != null)
//         {
//             blockMaterial.color = Color.blue;
//         }
//     }
// }

using UnityEngine;
using System;

public class BlockColorChanger : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material blockMaterial;

    public event Action<BlockColorChanger, bool> OnColorChange; // Event to notify mainMemory

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

        bool isYellow = actualValue == targetValue;
        blockMaterial.color = isYellow ? Color.yellow : Color.blue;

        OnColorChange?.Invoke(this, isYellow);
    }

    private void SetColorToBlue()
    {
        if (blockMaterial != null)
        {
            blockMaterial.color = Color.blue;
        }
    }
}
