using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputController : MonoBehaviour
{
    public AndLogicGate andLogicGate;
    public bool buttonInput1;
    public bool buttonInput2;
    public string playerTag = "Player";

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
        if (other.CompareTag(playerTag) && gameObject.name == "button1")
        {
            Debug.Log("in the onTriggerEnter");
            buttonInput1 = !buttonInput1;
            andLogicGate.input1 = buttonInput1;
            color = buttonInput1;
            UpdateButtonColor();
        }
        else if (other.CompareTag(playerTag) && gameObject.name == "button2")
        {
            buttonInput2 = !buttonInput2;
            andLogicGate.input2 = buttonInput2;
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
