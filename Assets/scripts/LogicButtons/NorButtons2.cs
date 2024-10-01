using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorButtons2 : MonoBehaviour
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
        CheckNor2Gate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < 0)
            {
                isActivated = !isActivated;
                UpdateButtonColor();
                CheckNor2Gate();
            }
        }
    }

    void CheckNor2Gate()
    {
        NorButtons2[] buttons = FindObjectsOfType<NorButtons2>();
        bool Activated = false;

        foreach (NorButtons2 button in buttons)
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
