using UnityEngine;
using TMPro;

public class BinaryArrayAdder : MonoBehaviour
{
    public BinaryButtonArray array1; 
    public BinaryButtonArray array2; 
    public TMP_Text outputText; 
    private int binarySum; 

    // Reference to the ParentGate
    public ParentGate parentGate; 

    // Enum to decide which target value to compare the binary sum against
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
        if ((array1 != null || array2 != null) && outputText != null)
        {
            int decimalValue1 = array1 != null ? array1.GetDecimalValue() : 0;
            int decimalValue2 = array2 != null ? array2.GetDecimalValue() : 0;
            binarySum = decimalValue1 + decimalValue2;
            outputText.text = "Sum: " + binarySum;

            // Call the ParentGate method to check the binary sum
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
