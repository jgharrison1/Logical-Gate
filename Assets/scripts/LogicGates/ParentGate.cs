using System;
using System.Collections.Generic;
using UnityEngine;

public class ParentGate : MonoBehaviour
{
    public bool input1;
    public bool input2;
    public bool output;
    private GameObject input1ButtonObject;
    private GameObject input2ButtonObject;
    public ParentGate previousGate1;
    public ParentGate previousGate2;
    private string playerTag = "Player";

    // Fields for sprite representation
    public SpriteRenderer spriteRenderer;  // Attach the SpriteRenderer component here
    public Sprite spriteTrue;              // Attach the sprite for output = true here
    public Sprite spriteFalse;             // Attach the sprite for output = false here
    
    // Separate target values for input1 and input2
    public int targetValueForInput1;
    public int targetValueForInput2;

    public ParentGate(bool input1, bool input2)
    {
        this.input1 = input1;
        this.input2 = input2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && other.gameObject == input1ButtonObject)
        {
            input1 = !input1;
        }
        if (other.CompareTag(playerTag) && other.gameObject == input2ButtonObject)
        {
            input2 = !input2;
        }
    }

    // Method to update the sprite based on output value
    protected void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = output ? spriteTrue : spriteFalse;
        }
    }

    // Method to check the binary sum and update inputs based on specific target values
    public void CheckBinarySum(int binarySum, bool affectInput1)
    {
        if (affectInput1)
        {
            input1 = binarySum == targetValueForInput1;
        }
        else
        {
            input2 = binarySum == targetValueForInput2;
        }

        output = input1 || input2; 
        UpdateSprite(); 
    }
}
