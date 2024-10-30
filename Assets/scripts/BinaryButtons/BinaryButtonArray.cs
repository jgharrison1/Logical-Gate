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
    public bool[] canToggle;
    public int[] staticValues; 
    public TMP_Text decimalDisplayText;

    public Sprite sprite0; // toggleable "off" sprite
    public Sprite sprite1; // toggleable "on" sprite
    public Sprite sprite2; // non-toggleable "off" sprite
    public Sprite sprite3; // non-toggleable "on" sprite

    private uint binaryOutput;
    public string arrayID;

    private BinaryArrayAdder binaryAdder; 

    private void Start()
    {
        binaryArray = new int[arraySize];
        InitializeBinaryArray();
        SetButtonColors();
        UpdateDecimalDisplay();
    }

    private void OnValidate()
    {
        // Ensure arrays match `arraySize` length whenever `arraySize` is changed in the inspector
        if (arraySize < 0) arraySize = 0;

        // Resize `buttonObjects` array
        if (buttonObjects == null || buttonObjects.Length != arraySize)
        {
            var newButtonObjects = new GameObject[arraySize];
            for (int i = 0; i < arraySize && i < buttonObjects?.Length; i++)
            {
                newButtonObjects[i] = buttonObjects[i];
            }
            buttonObjects = newButtonObjects;
        }

        // Resize `canToggle` array
        if (canToggle == null || canToggle.Length != arraySize)
        {
            var newCanToggle = new bool[arraySize];
            for (int i = 0; i < arraySize && i < canToggle?.Length; i++)
            {
                newCanToggle[i] = canToggle[i];
            }
            for (int i = canToggle?.Length ?? 0; i < arraySize; i++)
            {
                newCanToggle[i] = true; // Default to true for new entries
            }
            canToggle = newCanToggle;
        }

        // Resize `staticValues` array
        if (staticValues == null || staticValues.Length != arraySize)
        {
            var newStaticValues = new int[arraySize];
            for (int i = 0; i < arraySize && i < staticValues?.Length; i++)
            {
                newStaticValues[i] = staticValues[i];
            }
            staticValues = newStaticValues;
        }
    }

    private void InitializeBinaryArray()
    {
        for (int i = 0; i < arraySize; i++)
        {
            binaryArray[i] = canToggle[i] ? 0 : Mathf.Clamp(staticValues[i], 0, 1);
        }
    }

    public void SetBinaryAdder(BinaryArrayAdder adder)
    {
        binaryAdder = adder;
    }

    public void ToggleBinaryValue(int index, string callerID)
    {
        if (callerID != arrayID || !canToggle[index]) return; // Check if toggling is allowed for this index

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
            SpriteRenderer spriteRenderer = buttonObjects[index].GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Choose sprite based on toggleability and binary state
                if (canToggle[index])
                {
                    spriteRenderer.sprite = (binaryArray[index] == 1) ? sprite1 : sprite0;
                }
                else
                {
                    spriteRenderer.sprite = (binaryArray[index] == 1) ? sprite3 : sprite2;
                }
                
                // Set color based on state (green for on, red for off)
                spriteRenderer.color = (binaryArray[index] == 1) ? Color.green : Color.red;
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

