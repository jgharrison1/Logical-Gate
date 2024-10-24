using UnityEngine;
using TMPro; // Ensure you are using TextMeshPro

public class BinaryButtonArray : MonoBehaviour
{
    public enum BinaryRepresentation
    {
        UnsignedMagnitude,
        SignedMagnitude
    }

    public BinaryRepresentation representationType; // Field to choose between signed and unsigned
    public int arraySize = 10;                      // Size of the binary array
    public int[] binaryArray;                       // The binary array to store on/off values
    public GameObject[] buttonObjects;              // Array of GameObjects representing the buttons
    public TMP_Text decimalDisplayText;             // TextMeshPro component to display the decimal value

    private void Start()
    {
        binaryArray = new int[arraySize];
        SetButtonColors();
        UpdateDecimalDisplay(); // Display the initial decimal value
    }

    public void ToggleBinaryValue(int index)
    {
        if (index >= 0 && index < binaryArray.Length)
        {
            binaryArray[index] = 1 - binaryArray[index]; // Toggle between 0 and 1
            UpdateButtonColor(index);
            UpdateDecimalDisplay();  // Update the decimal value after toggling
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

    // Convert the binary array to a decimal number
    private int ConvertBinaryArrayToDecimal()
    {
        int decimalValue = 0;

        // Convert binary array to decimal value (magnitude only)
        for (int i = 0; i < binaryArray.Length; i++)
        {
            decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
        }

        // Check representation type to apply signed magnitude logic
        if (representationType == BinaryRepresentation.SignedMagnitude)
        {
            // If the most significant bit (sign bit) is set, make the value negative
            if (binaryArray[0] == 1) // Check if the sign bit is set
            {
                // Adjust for signed magnitude: subtract 1 from the magnitude to get the correct negative value
                decimalValue = -decimalValue;
                // Ensure that we remove the influence of the sign bit
                decimalValue += (1 << (binaryArray.Length - 1)); // Add back the value of the sign bit
            }
        }

        return decimalValue;
    }

    // Update the UI text with the decimal value
    private void UpdateDecimalDisplay()
    {
        if (decimalDisplayText != null)
        {
            int decimalValue = ConvertBinaryArrayToDecimal();
            decimalDisplayText.text = "Decimal Value: " + decimalValue; // You can customize the display text
        }
    }
}

// using UnityEngine;
// using TMPro; // Ensure you are using TextMeshPro

// public class BinaryButtonArray : MonoBehaviour
// {
//     public enum BinaryRepresentation
//     {
//         UnsignedMagnitude,
//         SignedMagnitude
//     }

//     public BinaryRepresentation representationType; // Field to choose between signed and unsigned
//     public int arraySize = 10;                      // Size of the binary array
//     public int[] binaryArray;                       // The binary array to store on/off values
//     public GameObject[] buttonObjects;              // Array of GameObjects representing the buttons
//     public TMP_Text decimalDisplayText;             // TextMeshPro component to display the decimal value

//     private void Start()
//     {
//         binaryArray = new int[arraySize];
//         buttonObjects = new GameObject[arraySize]; // Ensure buttonObjects is initialized
//         SetButtonColors();
//         UpdateDecimalDisplay(); // Display the initial decimal value
//     }

//     public void ToggleBinaryValue(int index)
//     {
//         if (index >= 0 && index < binaryArray.Length)
//         {
//             binaryArray[index] = 1 - binaryArray[index]; // Toggle between 0 and 1
//             UpdateButtonColor(index);
//             UpdateDecimalDisplay();  // Update the decimal value after toggling
//         }
//     }

//     private void UpdateButtonColor(int index)
//     {
//         if (index >= 0 && index < buttonObjects.Length)
//         {
//             Renderer buttonRenderer = buttonObjects[index].GetComponent<Renderer>();
//             if (buttonRenderer != null)
//             {
//                 buttonRenderer.material.color = (binaryArray[index] == 1) ? Color.green : Color.red;
//             }
//         }
//     }

//     private void SetButtonColors()
//     {
//         for (int i = 0; i < buttonObjects.Length; i++)
//         {
//             UpdateButtonColor(i);
//         }
//     }

//     // Convert the binary array to a decimal number
//     private int ConvertBinaryArrayToDecimal()
//     {
//         int decimalValue = 0;

//         // Convert binary array to decimal value (magnitude only)
//         for (int i = 0; i < binaryArray.Length; i++)
//         {
//             decimalValue += binaryArray[i] * (1 << (binaryArray.Length - 1 - i));
//         }

//         // Check representation type to apply signed magnitude logic
//         if (representationType == BinaryRepresentation.SignedMagnitude)
//         {
//             // If the most significant bit (sign bit) is set, make the value negative
//             if (binaryArray[0] == 1) // Check if the sign bit is set
//             {
//                 decimalValue = -decimalValue;
//                 decimalValue += (1 << (binaryArray.Length - 1)); // Add back the value of the sign bit
//             }
//         }

//         return decimalValue;
//     }

//     // Update the UI text with the decimal value
//     private void UpdateDecimalDisplay()
//     {
//         if (decimalDisplayText != null)
//         {
//             int decimalValue = ConvertBinaryArrayToDecimal();
//             decimalDisplayText.text = "Decimal Value: " + decimalValue; // You can customize the display text
//         }
//     }
// }

