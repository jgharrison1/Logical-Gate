using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BinaryArrayAdder : MonoBehaviour
{
    public BinaryButtonArray array1; 
    public BinaryButtonArray array2; 
    public TMP_Text outputText; 
    private int binarySum; 
    public ParentGate parentGate; 
    public Sprite lineOnSprite;
    public Sprite lineOffSprite;
    public List<SpriteRenderer> connectedLines;

    public enum InputToAffect
    {
        Input1,
        Input2
    }

    public InputToAffect inputToAffect; // Dropdown in Inspector

    void Start()
    {
        if (array1 != null)
        {
            array1.SetBinaryAdder(this);
        }
        if (array2 != null)
        {
            array2.SetBinaryAdder(this);
        }
    }

    public void UpdateSumOutput()
    {
        if (array1 != null || array2 != null)
        {
            int decimalValue1 = array1 != null ? array1.GetDecimalValue() : 0;
            int decimalValue2 = array2 != null ? array2.GetDecimalValue() : 0;
            binarySum = decimalValue1 + decimalValue2;

            //when using binary arrays as both inputs of the parent gate, binary adder needs to account for turning lines on and off.
            if(inputToAffect == InputToAffect.Input1){
                if(binarySum == parentGate.targetValueForInput1){
                    foreach(SpriteRenderer r in connectedLines) //change sprites for connected lines
                    {
                        r.sprite = lineOnSprite;
                    }
                }
                else {
                    foreach(SpriteRenderer r in connectedLines) //change sprites for connected lines
                    {
                        r.sprite = lineOffSprite;
                    }
                }
            }
            else {
                if(binarySum == parentGate.targetValueForInput2){
                    foreach(SpriteRenderer r in connectedLines) //change sprites for connected lines
                    {
                        r.sprite = lineOnSprite;
                    }
                }
                else {
                    foreach(SpriteRenderer r in connectedLines) //change sprites for connected lines
                    {
                        r.sprite = lineOffSprite;
                    }
                }
            }

            if (outputText != null)
            {
                outputText.text = "Sum: " + binarySum;
            }

            if (parentGate != null)
            {
                parentGate.CheckBinarySum(binarySum, inputToAffect == InputToAffect.Input1);
            }
        }
    }

    public int GetOutputValue()
    {
        return binarySum; 
    }
}
