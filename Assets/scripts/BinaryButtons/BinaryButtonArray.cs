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

    public string arrayID; // Unique identifier for each BinaryButtonArray instance

    private void Start()
    {
        // Initialize the binary array for this specific instance
        binaryArray = new int[arraySize];
        SetButtonColors();
        UpdateDecimalDisplay();
    }

    public void ToggleBinaryValue(int index, string callerID)
    {
        if (callerID != arrayID) return;  // Ensure only matching arrayID can toggle this instance
        Debug.Log("Instance: " + gameObject.name + " - Toggling index: " + index);
   
        if (index >= 0 && index < binaryArray.Length)
        {
            binaryArray[index] = 1 - binaryArray[index];
            UpdateButtonColor(index);
            UpdateDecimalDisplay();
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
            // In UnsignedMagnitude, include all bits, including the MSB
            for (int i = 0; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }
        }
        else if (representationType == BinaryRepresentation.SignedMagnitude)
        {
            // In SignedMagnitude, start from the second bit (index 1) for value calculation
            for (int i = 1; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }

            // If MSB is 1, negate the decimal value for SignedMagnitude
            if (binaryArray[0] == 1)
            {
                decimalValue = -decimalValue;
            }
        }
        else if (representationType == BinaryRepresentation.TwosComplement)
        {
            // In Two's Complement, treat MSB as negative if it's 1
            bool isNegative = (binaryArray[0] == 1);
            for (int i = 1; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }
            if (isNegative)
            {
                // Calculate two's complement by inverting bits and adding 1
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
}
