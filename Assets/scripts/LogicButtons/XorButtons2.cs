using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XorButtons2 : MonoBehaviour
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
        CheckXor2Gate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < 0)
            {
                isActivated = !isActivated;
                UpdateButtonColor();
                CheckXor2Gate();
            }
        }
    }

    void CheckXor2Gate()
    {
       XorButtons2[] buttons = FindObjectsOfType<XorButtons2>();
        bool Activated = false;
        bool allActivated = true;

        foreach (XorButtons2 button in buttons)
        {
            if (!button.isActivated)
            {
                allActivated = false;
            }
            else
            {
                Activated = true;
            }
        }

        if ((Activated == true) && (allActivated == false))
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