using UnityEngine;
using TMPro;

public class BinaryArrayAdder : MonoBehaviour
{
    public BinaryButtonArray array1; 
    public BinaryButtonArray array2; 
    public TMP_Text outputText; 
    private int binarySum; 

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
        return binarySum; 
    }
}

