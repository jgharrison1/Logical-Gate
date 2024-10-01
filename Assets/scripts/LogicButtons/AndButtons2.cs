using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndButtons2 : MonoBehaviour
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
        CheckAnd2Gate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < 0)
            {
                isActivated = !isActivated;
                UpdateButtonColor();
                CheckAnd2Gate();
            }
        }
    }

    void CheckAnd2Gate()
    {
        AndButtons2[] buttons = FindObjectsOfType<AndButtons2>();
        bool allActivated = true;

        foreach (AndButtons2 button in buttons)
        {
            if (!button.isActivated)
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated)
        {
            door.OpenDoor();
        }
        else
        {
            door.ClosedDoor();
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
