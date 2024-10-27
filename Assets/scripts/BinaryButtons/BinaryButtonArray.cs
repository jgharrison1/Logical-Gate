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

    private uint binaryOutput; // Assuming this holds your binary output
    public string arrayID; // Unique identifier for each BinaryButtonArray instance

    private BinaryArrayAdder binaryAdder; // Reference to the associated BinaryArrayAdder

    private void Start()
    {
        // Initialize the binary array for this specific instance
        binaryArray = new int[arraySize];
        SetButtonColors();
        UpdateDecimalDisplay();
    }

    // Method to set the binary adder reference
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

            // Update sum if BinaryArrayAdder is assigned
            binaryAdder?.UpdateSumOutput();
        }
    }

    private void UpdateButtonColor(int index)
    {
        if (index >= 0 && index < buttonObjects.Length)
        {
            Renderer buttonRenderer = buttonObjects[index].GetComponent<Renderer>();
            if (buttonRenderer != null)
            {
                buttonRenderer.material.color = (binaryArray[index] == 1) ? Color.green : Color.red;
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
        return (int)binaryOutput; // Return the output value as an integer
    }
}

