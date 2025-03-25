using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BinaryButtonArray : MonoBehaviour
{
    public enum BinaryRepresentation
    {
        UnsignedMagnitude,
        SignedMagnitude,
        TwosComplement
    }

    [Header("Representation Settings")]
    public List<BinaryRepresentation> allowedRepresentations;  // List of allowed representations
    private int currentRepresentationIndex = 0;                // Tracks the current representation index

    public int arraySize = 10;
    private int[] binaryArray;
    public GameObject[] buttonObjects;
    public bool[] canToggle;
    public int[] staticValues; 
    public TMP_Text decimalDisplayText;
    public TMP_Text representationTypeDisplayText;

    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    private uint binaryOutput;
    public string arrayID;
    private BinaryArrayAdder binaryAdder;
    public SpriteRenderer spriteRendererRep;
    public Sprite spriteUnsigned;
    public Sprite spriteSigned;
    public Sprite spriteTwos;

    private void Start()
    {
        binaryArray = new int[arraySize];
        InitializeBinaryArray();
        SetButtonColors();

        if (allowedRepresentations.Count == 0)
        {
            allowedRepresentations.Add(BinaryRepresentation.UnsignedMagnitude);
        }

        currentRepresentationIndex = Mathf.Clamp(currentRepresentationIndex, 0, allowedRepresentations.Count - 1);
        UpdateDecimalDisplay();
        UpdateRepresentationTypeDisplay(); 
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
        if (callerID != arrayID || !canToggle[index]) return;

        if (index >= 0 && index < binaryArray.Length)
        {
            binaryArray[index] = 1 - binaryArray[index];
            UpdateButtonColor(index);
            UpdateDecimalDisplay();

            // Notify the BinaryArrayAdder to update sum
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
                if (canToggle[index])
                {
                    spriteRenderer.sprite = (binaryArray[index] == 1) ? sprite1 : sprite0;
                }
                else
                {
                    spriteRenderer.sprite = (binaryArray[index] == 1) ? sprite3 : sprite2;
                }
                
                //spriteRenderer.color = (binaryArray[index] == 1) ? Color.green : Color.red;
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
        if (allowedRepresentations.Count == 0)
        {
            return 0;
        }

        int decimalValue = 0;
        BinaryRepresentation activeType = allowedRepresentations[currentRepresentationIndex];

        if (activeType == BinaryRepresentation.UnsignedMagnitude)
        {
            spriteRendererRep.sprite = spriteUnsigned;
            for (int i = 0; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }
        }
        else if (activeType == BinaryRepresentation.SignedMagnitude)
        {
            spriteRendererRep.sprite = spriteSigned;
            for (int i = 1; i < binaryArray.Length; i++)
            {
                decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
            }

            if (binaryArray[0] == 1)
            {
                decimalValue = -decimalValue;
            }
        }
        else if (activeType == BinaryRepresentation.TwosComplement)
        {
            spriteRendererRep.sprite = spriteTwos;
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

    private void UpdateRepresentationTypeDisplay()
    {
        if (representationTypeDisplayText != null && allowedRepresentations.Count > 0)
        {
            representationTypeDisplayText.text = "" + allowedRepresentations[currentRepresentationIndex].ToString();
        }
    }

    public void CycleRepresentationType()
    {
        if (allowedRepresentations.Count > 0)
        {
            currentRepresentationIndex = (currentRepresentationIndex + 1) % allowedRepresentations.Count;
            UpdateRepresentationTypeDisplay();
            UpdateDecimalDisplay();
            binaryAdder?.UpdateSumOutput();
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


