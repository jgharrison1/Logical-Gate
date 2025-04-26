// // using UnityEngine;
// // using TMPro;

// // public class BlockType : MonoBehaviour
// // {
// //     public enum Type { PageNumber, Offset, FrameNumber }
// //     public Type blockType;

// //     public int addressValue;

// //     [Header("Interaction Settings")]
// //     public bool isGrabbable = true; 

// //     private TMP_Text textMesh;
// //     private Transform textTransform;

// //     private void Start()
// //     {
// //         CreateTextObject();
// //         UpdateAddressDisplay();
// //     }

// //     private void CreateTextObject()
// //     {
// //         GameObject textObj = new GameObject("AddressText");
// //         textObj.transform.SetParent(transform);
// //         textObj.transform.localPosition = new Vector3(1.2f, 0, 0);

// //         textMesh = textObj.AddComponent<TextMeshPro>();
// //         textMesh.fontSize = 5;
// //         textMesh.alignment = TextAlignmentOptions.Center;
// //         textMesh.color = Color.green;

// //         Renderer renderer = textMesh.GetComponent<Renderer>();
// //         if (renderer != null)
// //             renderer.sortingOrder = 10;

// //         textTransform = textObj.transform;
// //     }

// //     public void SetBlockType(Type newType)
// //     {
// //         blockType = newType;
// //     }

// //     public int GetAddress()
// //     {
// //         return addressValue;
// //     }

// //     private void UpdateAddressDisplay()
// //     {
// //         if (textMesh != null)
// //             textMesh.text = addressValue.ToString();
// //     }

// //     public void ShowAddressText()
// //     {
// //         if (textMesh != null)
// //             textMesh.enabled = true;
// //     }

// //     public void HideAddressText()
// //     {
// //         if (textMesh != null)
// //             textMesh.enabled = false;
// //     }

// //     private void OnValidate()
// //     {
// //         if (textMesh == null && Application.isPlaying)
// //         {
// //             CreateTextObject();
// //         }
// //         UpdateAddressDisplay();
// //     }
// // }

// using UnityEngine;
// using TMPro;
// using System; // Needed for binary conversion

// public class BlockType : MonoBehaviour
// {
//     public enum Type { PageNumber, Offset, FrameNumber }
//     public Type blockType;

//     [Header("Address Settings")]
//     public int addressValue;
//     [Tooltip("Number of bits for binary representation (e.g., 4 bits = 0-15).")]
//     public int bitSize = 4; // Default to 4 bits

//     [Header("Interaction Settings")]
//     public bool isGrabbable = true;

//     private TMP_Text textMesh;
//     private Transform textTransform;

//     private void Start()
//     {
//         ClampAddressValueToBitSize();
//         CreateTextObject();
//         UpdateAddressDisplay();
//     }

//     private void CreateTextObject()
//     {
//         GameObject textObj = new GameObject("AddressText");
//         textObj.transform.SetParent(transform);
//         textObj.transform.localPosition = new Vector3(1.2f, 0, 0);

//         textMesh = textObj.AddComponent<TextMeshPro>();
//         textMesh.fontSize = 5;
//         textMesh.alignment = TextAlignmentOptions.Center;
//         textMesh.color = Color.green;

//         Renderer renderer = textMesh.GetComponent<Renderer>();
//         if (renderer != null)
//             renderer.sortingOrder = 10;

//         textTransform = textObj.transform;
//     }

//     public void SetBlockType(Type newType)
//     {
//         blockType = newType;
//     }

//     public int GetAddress()
//     {
//         return addressValue;
//     }

//     public void ShowAddressText()
//     {
//         if (textMesh != null)
//             textMesh.enabled = true;
//     }

//     public void HideAddressText()
//     {
//         if (textMesh != null)
//             textMesh.enabled = false;
//     }

//     private void UpdateAddressDisplay()
//     {
//         if (textMesh != null)
//             textMesh.text = GetBinaryString(); // <-- Show binary, not decimal
//     }

//     public void ClampAddressValueToBitSize()
//     {
//         int maxValue = (1 << bitSize) - 1;
//         addressValue = Mathf.Clamp(addressValue, 0, maxValue);
//     }

//     public string GetBinaryString()
//     {
//         return Convert.ToString(addressValue, 2).PadLeft(bitSize, '0');
//     }

//     // private void OnValidate()
//     // {
//     //     ClampAddressValueToBitSize();

//     //     if (!Application.isPlaying)
//     //         return;

//     //     if (textMesh == null)
//     //     {
//     //         CreateTextObject();
//     //     }
//     //     UpdateAddressDisplay();
//     // }
// private void OnValidate()
// {
//     // Interpret addressValue as binary
//     string valueStr = addressValue.ToString();
//     if (int.TryParse(valueStr, out int parsedDecimal))
//     {
//         // Try to parse the entered number as binary
//         try
//         {
//             addressValue = System.Convert.ToInt32(valueStr, 2);
//         }
//         catch
//         {
//             Debug.LogWarning($"Invalid binary number entered for Block '{gameObject.name}'");
//         }
//     }

//     if (textMesh == null && Application.isPlaying)
//     {
//         CreateTextObject();
//     }
//     UpdateAddressDisplay();
// }

// }

using System; // Ensure we are using the System namespace
using UnityEngine;
using TMPro;

public class BlockType : MonoBehaviour
{
    public enum Type { PageNumber, Offset, FrameNumber }
    public Type blockType;

    // Store the address value as a binary string.
    [Header("Address Settings")]
    public string binaryAddressValue = "0"; // Default to "0" (binary string).

    // Internal integer that stores the converted binary value.
    [HideInInspector]
    public int addressValue;

    // Add bitSize to control the number of bits
    [Header("Bit Size Settings")]
    public int bitSize = 4; // Default to 4 bits unless specified otherwise.

    [Header("Interaction Settings")]
    public bool isGrabbable = true;

    private TMP_Text textMesh;
    private Transform textTransform;

    private void Start()
    {
        ConvertBinaryToInt(); // Convert the binary string to an integer at the start.
        CreateTextObject();
        UpdateAddressDisplay();
    }

    private void CreateTextObject()
    {
        GameObject textObj = new GameObject("AddressText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(1.2f, 0, 0);

        textMesh = textObj.AddComponent<TextMeshPro>();
        textMesh.fontSize = 5;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = Color.green;

        Renderer renderer = textMesh.GetComponent<Renderer>();
        if (renderer != null)
            renderer.sortingOrder = 10;

        textTransform = textObj.transform;
    }

    // Converts the binary string to an integer.
    public void ConvertBinaryToInt()
    {
        if (!string.IsNullOrEmpty(binaryAddressValue))
        {
            // Parse the binary string to an integer, but ensure it's within the bitSize limit.
            addressValue = Mathf.Clamp(System.Convert.ToInt32(binaryAddressValue, 2), 0, (1 << bitSize) - 1);
        }
        else
        {
            addressValue = 0;
        }
    }

    // Sets the block type (e.g., Page Number, Offset, etc.)
    public void SetBlockType(Type newType)
    {
        blockType = newType;
    }

    // Returns the address value in its integer form.
    public int GetAddress()
    {
        return addressValue;
    }

    // Updates the display to show the current address value in binary.
    private void UpdateAddressDisplay()
    {
        if (textMesh != null)
            textMesh.text = Convert.ToString(addressValue, 2).PadLeft(bitSize, '0'); // Display as binary with leading zeros
    }

    public void ShowAddressText()
    {
        if (textMesh != null)
            textMesh.enabled = true;
    }

    public void HideAddressText()
    {
        if (textMesh != null)
            textMesh.enabled = false;
    }

    private void OnValidate()
    {
        ConvertBinaryToInt(); // Update address value on change in the inspector.
        UpdateAddressDisplay(); // Update text display to reflect changes.
    }
}
