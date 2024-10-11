using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndLeaf : MonoBehaviour
{
    public GateDoor door;
    private bool isActivated = false;

    private SpriteRenderer spriteRenderer;
    public Color activatedColor = Color.green;
    public Color deactivatedColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonColor();
        CheckAndLeafGate();
    }

    void Update()
    {
        
    }

    void CheckAndLeafGate()
    {
        // AndButtons[] buttons = FindObjectsOfType<AndButtons>();
        // bool allActivated = true;

        // foreach (AndButtons button in buttons)
        // {
        //     if (!button.isActivated)
        //     {
        //         allActivated = false;
        //         break;
        //     }
        // }

        // if (allActivated)
        // {
        //     door.OpenDoor();
        // }
        // else
        // {
        //     door.ClosedDoor();
        // }
    }

    private void UpdateButtonColor()
    {
        if (spriteRenderer != null)
        {
            if (isActivated)
            {
                spriteRenderer.color = activatedColor;
            }
            else
            {
                spriteRenderer.color = deactivatedColor;
            }
        }
    }
}
