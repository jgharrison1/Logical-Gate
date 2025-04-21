using UnityEngine;
using TMPro;

public class BlockType : MonoBehaviour
{
    public enum Type { PageNumber, Offset, FrameNumber }
    public Type blockType;

    public int addressValue;

    [Header("Interaction Settings")]
    public bool isGrabbable = true; 

    private TMP_Text textMesh;
    private Transform textTransform;

    private void Start()
    {
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
            textMesh.text = addressValue.ToString();
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
        if (textMesh == null && Application.isPlaying)
        {
            CreateTextObject();
        }
        UpdateAddressDisplay();
    }
}
