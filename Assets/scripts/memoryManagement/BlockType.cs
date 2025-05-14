using System;
using UnityEngine;
using TMPro;

public class BlockType : MonoBehaviour
{
    public enum Type { PageNumber, Offset, FrameNumber }
    public Type blockType;

    [Header("Address Settings")]
    public string binaryAddressValue = "0";

    [HideInInspector]
    public int addressValue;

    [Header("Bit Size Settings")]
    public int bitSize = 4;

    [Header("Interaction Settings")]
    public bool isGrabbable = true;

    [Header("Visibility Settings")]
    public bool isVisible = true; // Toggle visibility in Inspector

    private TMP_Text addressTextMesh;
    private TMP_Text typeTextMesh;
    private Transform textTransform;

    private void Start()
    {
        ConvertBinaryToInt();
        CreateTextObject();
        UpdateAddressDisplay();
    }

    private void CreateTextObject()
    {
        // Create address text (lower)
        GameObject addressTextObj = new GameObject("AddressText");
        addressTextObj.transform.SetParent(transform);
        addressTextObj.transform.localPosition = new Vector3(0, 0, 0);
        addressTextMesh = addressTextObj.AddComponent<TextMeshPro>();
        addressTextMesh.fontSize = 5;
        addressTextMesh.alignment = TextAlignmentOptions.Center;
        addressTextMesh.color = Color.green;
        Renderer r1 = addressTextMesh.GetComponent<Renderer>();
        if (r1 != null) r1.sortingOrder = 10;

        // Create type text (upper)
        GameObject typeTextObj = new GameObject("TypeText");
        typeTextObj.transform.SetParent(transform);
        typeTextObj.transform.localPosition = new Vector3(0, .65f, 0);
        typeTextMesh = typeTextObj.AddComponent<TextMeshPro>();
        typeTextMesh.fontSize = 4;
        typeTextMesh.alignment = TextAlignmentOptions.Center;
        typeTextMesh.color = Color.green;
        Renderer r2 = typeTextMesh.GetComponent<Renderer>();
        if (r2 != null) r2.sortingOrder = 10;

        textTransform = addressTextObj.transform;
    }

    public void ConvertBinaryToInt()
    {
        if (!string.IsNullOrEmpty(binaryAddressValue))
        {
            if (binaryAddressValue.Length > bitSize)
            {
                Debug.LogError($"Binary address '{binaryAddressValue}' exceeds bit size of {bitSize}. Truncating.");
                binaryAddressValue = binaryAddressValue.Substring(0, bitSize);
            }

            addressValue = Mathf.Clamp(Convert.ToInt32(binaryAddressValue, 2), 0, (1 << bitSize) - 1);
        }
        else
        {
            addressValue = 0;
        }
    }

    public void SetBlockType(Type newType)
    {
        blockType = newType;
        UpdateAddressDisplay();
    }

    public int GetAddress()
    {
        return isVisible ? addressValue : -1;
    }

    private void UpdateAddressDisplay()
    {
        // Lower address text shows "--" if not visible
        if (addressTextMesh != null)
        {
            addressTextMesh.text = isVisible
                ? Convert.ToString(addressValue, 2).PadLeft(bitSize, '0')
                : "--";
        }

        // Upper type text always shown
        if (typeTextMesh != null)
        {
            typeTextMesh.text = blockType switch
            {
                Type.PageNumber => "P",
                Type.Offset => "D",
                Type.FrameNumber => "F",
                _ => ""
            };
        }
    }


    public void ShowAddressText()
    {
        if (addressTextMesh != null) addressTextMesh.enabled = true;
        if (typeTextMesh != null) typeTextMesh.enabled = true;
    }

    public void HideAddressText()
    {
        if (addressTextMesh != null) addressTextMesh.enabled = false;
        if (typeTextMesh != null) typeTextMesh.enabled = false;
    }

    private void OnValidate()
    {
        ConvertBinaryToInt();
        UpdateAddressDisplay();
    }
}
