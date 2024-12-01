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
    public SpriteRenderer spriteRenderer;
    public Sprite spriteTrue;
    public Sprite spriteFalse;
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

    protected void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = output ? spriteTrue : spriteFalse;
        }
    }

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
