using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputController : MonoBehaviour
{
    public ParentGate ConnectedGate;
    public bool buttonInput1;
    public bool buttonInput2;
    public string buttonName1;
    public string buttonName2;
    private string playerTag = "Player";

    private SpriteRenderer spriteRenderer;
    private bool color;
    public Color activatedColor = Color.green;
    public Color deactivatedColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && gameObject.name == buttonName1)
        {
            buttonInput1 = !buttonInput1;
            ConnectedGate.input1 = buttonInput1;
            color = buttonInput1;
            UpdateButtonColor();
        }
        else if (other.CompareTag(playerTag) && gameObject.name == buttonName2)
        {
            buttonInput2 = !buttonInput2;
            ConnectedGate.input2 = buttonInput2;
            color = buttonInput2;
            UpdateButtonColor();
        }
    }

    private void UpdateButtonColor()
    {
        if (spriteRenderer != null)
        {
            if (color)
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
