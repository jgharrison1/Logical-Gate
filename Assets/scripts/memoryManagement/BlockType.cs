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

    private TMP_Text textMesh;
    private Transform textTransform;

    private void Start()
    {
        ConvertBinaryToInt(); 
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

    public void ConvertBinaryToInt()
    {
        if (!string.IsNullOrEmpty(binaryAddressValue))
        {
            if (binaryAddressValue.Length > bitSize)
            {
                Debug.LogError($"Binary address '{binaryAddressValue}' exceeds bit size of {bitSize}. Truncating.");
                binaryAddressValue = binaryAddressValue.Substring(0, bitSize);
            }

            addressValue = Mathf.Clamp(System.Convert.ToInt32(binaryAddressValue, 2), 0, (1 << bitSize) - 1);
        }
        else
        {
            addressValue = 0;
        }
    }

    public void SetBlockType(Type newType)
    {
        blockType = newType;
    }

    public int GetAddress()
    {
        return addressValue;
    }

    private void UpdateAddressDisplay()
    {
        if (textMesh != null)
            textMesh.text = Convert.ToString(addressValue, 2).PadLeft(bitSize, '0'); 
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
        ConvertBinaryToInt();
        UpdateAddressDisplay(); 
    }
}
