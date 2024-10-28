using UnityEngine;
using TMPro;

public class BinaryButtonArray : MonoBehaviour
{
    public enum BinaryRepresentation
    {
        UnsignedMagnitude,
        SignedMagnitude,
        TwosComplement
    }

    public BinaryRepresentation representationType;
    public int arraySize = 10;
    private int[] binaryArray;
    public GameObject[] buttonObjects;
    public TMP_Text decimalDisplayText;

    public Sprite sprite0; 
    public Sprite sprite1; 

    private uint binaryOutput;
    public string arrayID;

    private BinaryArrayAdder binaryAdder;

    private void Start()
    {
        binaryArray = new int[arraySize];
        SetButtonColors();
        UpdateDecimalDisplay();
    }

    public void SetBinaryAdder(BinaryArrayAdder adder)
    {
        binaryAdder = adder;
    }

    public void ToggleBinaryValue(int index, string callerID)
    {
        if (callerID != arrayID) return;

        if (index >= 0 && index < binaryArray.Length)
        {
            binaryArray[index] = 1 - binaryArray[index];
            UpdateButtonColor(index);
            UpdateDecimalDisplay();

            binaryAdder?.UpdateSumOutput();
        }
    }

    private void UpdateButtonColor(int index)
    {
        if (index >= 0 && index < buttonObjects.Length)
        {
            Renderer buttonRenderer = buttonObjects[index].GetComponent<Renderer>();
            SpriteRenderer spriteRenderer = buttonObjects[index].GetComponent<SpriteRenderer>();

            if (buttonRenderer != null)
            {
                buttonRenderer.material.color = (binaryArray[index] == 1) ? Color.green : Color.red;
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = (binaryArray[index] == 1) ? sprite1 : sprite0;
            }
        }
    }

    private void SetButtonColors()
    {
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            UpdateButtonColor(i);
        }
    }

    private int ConvertBinaryArrayToDecimal()
    {
        int decimalValue = 0;

        if (representationType == BinaryRepresentation.UnsignedMagnitude)
        {
            for (int i = 0; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }
        }
        else if (representationType == BinaryRepresentation.SignedMagnitude)
        {
            for (int i = 1; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }

            if (binaryArray[0] == 1)
            {
                decimalValue = -decimalValue;
            }
        }
        else if (representationType == BinaryRepresentation.TwosComplement)
        {
            bool isNegative = (binaryArray[0] == 1);
            for (int i = 1; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }
            if (isNegative)
            {
                decimalValue = -(1 << (binaryArray.Length - 1)) + decimalValue;
            }
        }

        return decimalValue;
    }

    private void UpdateDecimalDisplay()
    {
        if (decimalDisplayText != null)
        {
            int decimalValue = ConvertBinaryArrayToDecimal();
            decimalDisplayText.text = "Decimal Value: " + decimalValue;
        }
    }

    public int GetDecimalValue()
    {
        return ConvertBinaryArrayToDecimal();
    }

    public int GetOutputValue()
    {
        return (int)binaryOutput;
    }
}
