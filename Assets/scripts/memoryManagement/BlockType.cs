using UnityEngine;
using TMPro;

public class BlockType : MonoBehaviour
{
    public enum Type { PageNumber, Offset, FrameNumber }
    public Type blockType;

    public int addressValue; // Address value as an integer

    private TMP_Text textMesh; // Reference to the text component

    private void Start()
    {
        // Create a new TextMeshPro object and set it up
        GameObject textObj = new GameObject("AddressText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(1.2f, 0, 0); // Adjust position to the right of the block

        // Add TMP_Text component to the GameObject
        textMesh = textObj.AddComponent<TextMeshPro>();
        textMesh.fontSize = 5;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.color = Color.green; // Set the text color to green

        // Set the sorting layer to ensure it's in front
        var renderer = textMesh.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 10; // Set the sorting layer to be higher to appear in the foreground
        }

        UpdateAddressDisplay();
    }

    public void SetBlockType(Type newType)
    {
        blockType = newType;
    }

    public int GetAddress()
    {
        return addressValue; // Return the address as an int
    }

    private void UpdateAddressDisplay()
    {
        if (textMesh != null)
        {
            textMesh.text = addressValue.ToString(); // Update the text to display the address value
        }
    }
}

