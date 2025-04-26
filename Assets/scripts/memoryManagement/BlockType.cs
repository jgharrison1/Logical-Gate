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

    // Converts the binary string to an integer, considering bit size
    public void ConvertBinaryToInt()
    {
        if (!string.IsNullOrEmpty(binaryAddressValue))
        {
            // Check if the binary string exceeds the bit size
            if (binaryAddressValue.Length > bitSize)
            {
                Debug.LogError($"Binary address '{binaryAddressValue}' exceeds bit size of {bitSize}. Truncating.");
                binaryAddressValue = binaryAddressValue.Substring(0, bitSize); // Truncate to fit bit size
            }

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
