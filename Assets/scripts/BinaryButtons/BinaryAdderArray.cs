using UnityEngine;
using TMPro;

public class BinaryArrayAdder : MonoBehaviour
{
    public BinaryButtonArray array1; 
    public BinaryButtonArray array2; 
    public TMP_Text outputText; 
    private int binarySum; 
    public ParentGate parentGate; 

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
