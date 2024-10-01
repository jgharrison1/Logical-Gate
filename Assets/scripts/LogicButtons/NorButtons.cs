using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorButtons : MonoBehaviour
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
        CheckNorGate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < 0)
            {
                isActivated = !isActivated;
                UpdateButtonColor();
                CheckNorGate();
            }
        }
    }

    void CheckNorGate()
    {
        NorButtons[] buttons = FindObjectsOfType<NorButtons>();
        bool Activated = false;

        foreach (NorButtons button in buttons)
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
