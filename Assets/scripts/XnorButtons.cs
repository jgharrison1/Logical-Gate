using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XnorButtons : MonoBehaviour
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < 0)
            {
                isActivated = !isActivated;
                UpdateButtonColor();
                CheckXnorGate();
            }
        }
    }

    void CheckXnorGate()
    {
        XnorButtons[] buttons = FindObjectsOfType<XnorButtons>();
        bool allActivated = true;
        bool Activated = false;

        foreach (XnorButtons button in buttons)
        {
            if (button.isActivated)
            {
                Activated = true;
                break;
            }
        }

        if (!Activated)
        {
            door.OpenDoor();
        }
        if (allActivated)
        {
            door.OpenDoor();
        }
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
