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
    public Sprite lineOnSprite;
    public Sprite lineOffSprite;
    public List<SpriteRenderer> connectedLines;
    public List<SpriteRenderer> binaryLines;
    public int targetValueForInput1;
    public int targetValueForInput2;
    private bool binary = false;

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
        
        if(binary)
        {
            foreach(SpriteRenderer r in binaryLines) //change sprites for connected lines
            {
                r.sprite = output ? lineOnSprite : lineOffSprite;
            }
            binary = false;
        }
        else
        {
            foreach(SpriteRenderer r in connectedLines) //change sprites for connected lines
            {
                r.sprite = output ? lineOnSprite : lineOffSprite;
            }
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
        binary=true;
        UpdateSprite(); 
    }
}

