using UnityEngine;
using TMPro;

public class BinaryArrayAdder : MonoBehaviour
{
    public BinaryButtonArray array1; // Reference to the first BinaryButtonArray
    public BinaryButtonArray array2; // Reference to the second BinaryButtonArray
    public TMP_Text outputText; // Text field to display the result

    private int binarySum; // Holds the calculated sum of the two arrays

    void Start()
    {
        // Set the binary adder reference in each BinaryButtonArray
        if (array1 != null)
        {
            array1.SetBinaryAdder(this);
        }

        if (array2 != null)
        {
            array2.SetBinaryAdder(this);
        }
    }

    // Method to update the output by adding the two arrays' decimal values
    public void UpdateSumOutput()
    {
        if (array1 != null && array2 != null && outputText != null)
        {
            int decimalValue1 = array1.GetDecimalValue();
            int decimalValue2 = array2.GetDecimalValue();
            binarySum = decimalValue1 + decimalValue2;

            outputText.text = "Sum: " + binarySum;
        }
    }

    public int GetOutputValue()
    {
        return binarySum; // Return the calculated sum
    }
}

