using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class mainMemory : MonoBehaviour
{
    [Header("Frame Number Slots")]
    public List<GameObject> frameSlots = new List<GameObject>();

    [Header("Offset Slots")]
    public List<GameObject> offsetSlots = new List<GameObject>(); 

    [Header("Target Values (Binary String)")]
    public List<string> targetValuesBinary = new List<string>(); 

    [Header("Block Color Changers")]
    public List<BlockColorChanger> blockColorChangers = new List<BlockColorChanger>();

    [Header("Runtime Assignment")]
    public List<GameObject> blocksToAssign = new List<GameObject>();
    public List<int> slotIndicesToAssign = new List<int>(); 
    public List<BlockType.Type> slotTypesToAssign = new List<BlockType.Type>(); 

    [Header("Sequence Validation")]
    public List<int> expectedSequence = new List<int>(); 
    private List<int> currentSequence = new List<int>();
    public bool sequenceCompleted = false; 

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
    private playerMovement playerMovementScript;

    [Header("Block Placement Tracking")]
    private List<bool> blockPlacementStatus = new List<bool>();

    [Header("Sequence Display")]
    public Canvas targetCanvas;
    public Vector3 expectedTextPosition;
    public Vector3 currentTextPosition;
    public TMP_FontAsset fontAsset;

    private TextMeshProUGUI expectedTextUI;
    private TextMeshProUGUI currentTextUI;

    private void Start()
    {
        blockPlacementStatus = new List<bool>(new bool[blocksToAssign.Count]);

        if (blocksToAssign.Count > 0)
        {
            for (int i = 0; i < blocksToAssign.Count; i++)
            {
                if (blocksToAssign[i] != null) { AssignBlockToSlot(i); }
            }
            ValidateFrameOffsetPairs();
        }

        CreateSequenceTextObjects();
        UpdateSequenceDisplay();

        UpdateBlockColorsOnStart();

        foreach (var changer in blockColorChangers)
        {
            changer.OnColorChange += HandleBlockColorChange;
        }
    }

    public void RegisterSlot(GameObject slot, BlockType.Type slotType)
    {
        if (slot == null) return;

        if (slotType == BlockType.Type.FrameNumber && !frameSlots.Contains(slot))
        {
            frameSlots.Add(slot);
        }
        else if (slotType == BlockType.Type.Offset && !offsetSlots.Contains(slot))
        {
            offsetSlots.Add(slot);
        }
    }

    public void UnregisterSlot(GameObject slot)
    {
        if (slot == null) return;
    }

    public GameObject GetBlockInSlot(GameObject slot)
    {
        return slotToBlockMap.ContainsKey(slot) ? slotToBlockMap[slot] : null;
    }

    public GameObject RemoveBlockFromSlot(GameObject slot)
    {
        if (!IsValidSlot(slot) || !slotToBlockMap.ContainsKey(slot))
        {
            return null;
        }

        GameObject block = slotToBlockMap[slot];
        slotToBlockMap.Remove(slot);
        block.transform.SetParent(null);
        block.SetActive(false);

        if (blockColorChangers.Count > 0)
        {
            blockColorChangers[0].SetTargetValue(0, GetTargetValueBinary(0)); 
        }

        return block;
    }

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (slot == null || block == null || !IsValidSlot(slot))
            return false;

        BlockType blockType = block.GetComponent<BlockType>();
        if (blockType == null)
            return false;

        if ((frameSlots.Contains(slot) && blockType.blockType != BlockType.Type.FrameNumber) ||
            (offsetSlots.Contains(slot) && blockType.blockType != BlockType.Type.Offset))
        {
            block.transform.SetParent(playerMovementScript.transform);
            block.transform.position = playerMovementScript.holdPosition.position;
            block.SetActive(true);
            return false;
        }

        slotToBlockMap[slot] = block;
        block.transform.SetParent(slot.transform);
        block.transform.localPosition = Vector3.zero;

        ValidateFrameOffsetPairs();

        if (frameSlots.Contains(slot))
        {
            int index = frameSlots.IndexOf(slot);
            if (index != -1 && index < frameSlots.Count && index < offsetSlots.Count && index < targetValuesBinary.Count)
            {
                GameObject frameBlock = GetBlockInSlot(frameSlots[index]);
                GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

                if (frameBlock != null && offsetBlock != null)
                {
                    string frameBinary = frameBlock.GetComponent<BlockType>()?.binaryAddressValue ?? "";
                    string offsetBinary = offsetBlock.GetComponent<BlockType>()?.binaryAddressValue ?? "";
                    string combined = frameBinary + offsetBinary;

                    if (combined == targetValuesBinary[index])
                    {
                        HandleBlockColorChange(blockColorChangers[index], true);
                    }
                }
            }
        }
        
        return true;
    }

    public bool IsSlotOccupied(GameObject slot, GameObject block)
    {
        return slotToBlockMap.ContainsKey(slot) && slotToBlockMap[slot] != block;
    }

    private bool IsValidSlot(GameObject slot)
    {
        return slot != null && (frameSlots.Contains(slot) || offsetSlots.Contains(slot));
    }

    public void ValidateFrameOffsetPairs()
    {
        for (int i = 0; i < frameSlots.Count && i < offsetSlots.Count; i++)
        {
            ValidateFrameOffsetPair(i);
        }
    }

    public void ValidateFrameOffsetPair(int index)
    {
        if (index < 0 || index >= targetValuesBinary.Count) return;

        GameObject frameBlock = GetBlockInSlot(frameSlots[index]);
        GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

        if (frameBlock == null || offsetBlock == null)
        {
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(0, 1); 
            }
            return;
        }

        BlockType frameBlockType = frameBlock.GetComponent<BlockType>();
        BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

        if (frameBlockType == null || offsetBlockType == null)
        {
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(0, 1); 
            }
            return;
        }

        string frameBinary = frameBlockType.binaryAddressValue;
        string offsetBinary = offsetBlockType.binaryAddressValue;

        if (string.IsNullOrEmpty(frameBinary) || string.IsNullOrEmpty(offsetBinary))
        {
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(0, 1); 
            }
            return;
        }

        string combinedBinary = frameBinary + offsetBinary;

        if (blockColorChangers.Count > index)
        {
            bool isMatch = combinedBinary == targetValuesBinary[index];
            blockColorChangers[index].SetTargetValue(isMatch ? 1 : 0, 1);
        }
    }

    public void AssignAllBlocksFromInspector()
    {
        for (int i = 0; i < blocksToAssign.Count; i++)
        {
            if (blocksToAssign[i] != null) { AssignBlockToSlot(i); }
        }
        ValidateFrameOffsetPairs();
    }

    private void AssignBlockToSlot(int index)
    {
        if (index < 0 || index >= blocksToAssign.Count) { return; }

        GameObject block = blocksToAssign[index];
        int slotIndex = slotIndicesToAssign[index];
        BlockType.Type slotType = slotTypesToAssign[index];

        if (block == null) { return; }

        List<GameObject> slotList = slotType == BlockType.Type.FrameNumber ? frameSlots : offsetSlots;

        if (slotIndex >= slotList.Count) { return; }

        GameObject slot = slotList[slotIndex];
        bool success = TryAddBlockToSlot(slot, block);

        if (success)
        {
            block.transform.SetParent(slot.transform);
            block.transform.position = slot.transform.position;
            block.transform.localRotation = Quaternion.identity;
            ValidateFrameOffsetPairs();
        }
        else
        {
            Debug.LogWarning($"Failed to assign {block.name} to {slot.name}.");
        }
    }

    private void UpdateBlockColorsOnStart()
    {
        for (int i = 0; i < blockColorChangers.Count; i++)
        {
            string frameBinary = "0";
            string offsetBinary = "0";

            if (i < frameSlots.Count)
            {
                GameObject frameBlock = GetBlockInSlot(frameSlots[i]);
                if (frameBlock != null)
                {
                    BlockType frameBlockType = frameBlock.GetComponent<BlockType>();
                    frameBinary = frameBlockType != null ? frameBlockType.binaryAddressValue : "0";
                }
            }

            if (i < offsetSlots.Count)
            {
                GameObject offsetBlock = GetBlockInSlot(offsetSlots[i]);
                if (offsetBlock != null)
                {
                    BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();
                    offsetBinary = offsetBlockType != null ? offsetBlockType.binaryAddressValue : "0";
                }
            }

            string combinedBinary = frameBinary + offsetBinary;
            bool isMatch = (i < targetValuesBinary.Count) && (combinedBinary == targetValuesBinary[i]);
            blockColorChangers[i].SetTargetValue(isMatch ? 1 : 0, 1);
        }
    }

    public int GetTargetValueBinary(int index)
    {
        if (index >= 0 && index < targetValuesBinary.Count)
        {
            string binaryValue = targetValuesBinary[index];
            int targetValue = 0;
            if (!string.IsNullOrEmpty(binaryValue))
            {
                try
                {
                    targetValue = System.Convert.ToInt32(binaryValue, 2);
                }
                catch (System.FormatException)
                {
                    Debug.LogError($"Invalid binary format: {binaryValue} at index {index}");
                }
            }
            return targetValue;
        }
        return 0;
    }

    public void HandleBlockColorChange(BlockColorChanger changer, bool isYellow)
    {
        int index = blockColorChangers.IndexOf(changer);
        if (index == -1) return;

        if (isYellow)
        {
            if (!blockPlacementStatus[index])
            {
                blockPlacementStatus[index] = true;
            }
            else
            {
                currentSequence.Add(index);
                Debug.Log($"Added index {index} to current sequence. Sequence: [{string.Join(", ", currentSequence)}]");

                UpdateSequenceDisplay();

                if (currentSequence.Count == expectedSequence.Count)
                {
                    bool correct = true;
                    for (int i = 0; i < expectedSequence.Count; i++)
                    {
                        if (currentSequence[i] != expectedSequence[i])
                        {
                            correct = false;
                            break;
                        }
                    }

                    sequenceCompleted = correct;

                    if (correct)
                    {
                        Debug.Log("Correct sequence achieved!");
                    }
                    else
                    {
                        Debug.Log("Incorrect sequence. Resetting.");
                        ResetSequence();
                    }
                }
                else if(currentSequence.Count > expectedSequence.Count)
                {
                    sequenceCompleted = false;
                    ResetSequence();
                }
                else
                {
                    sequenceCompleted = false;
                }
            }
        }
        else
        {
            Debug.Log($"Block removed or deactivated at index {index}. Resetting sequence.");
            ResetSequence();
        }
    }

    private void CreateSequenceTextObjects()
    {
        // Try to auto-assign canvas if not set
        if (targetCanvas == null)
        {
            targetCanvas = FindObjectOfType<Canvas>();

            if (targetCanvas == null)
            {
                Debug.LogError("No Canvas found in the scene. Please add one.");
                return;
            }
        }

        // Expected Sequence Text
        GameObject expectedGO = new GameObject("ExpectedSequenceText");
        expectedGO.transform.SetParent(targetCanvas.transform, false);
        expectedTextUI = expectedGO.AddComponent<TextMeshProUGUI>();
        expectedTextUI.fontSize = 48;
        expectedTextUI.alignment = TextAlignmentOptions.Left;
        expectedTextUI.color = Color.cyan;
        if (fontAsset != null) expectedTextUI.font = fontAsset;
        expectedGO.GetComponent<RectTransform>().anchoredPosition3D = expectedTextPosition;
        expectedGO.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 50);

        // Current Sequence Text
        GameObject currentGO = new GameObject("CurrentSequenceText");
        currentGO.transform.SetParent(targetCanvas.transform, false);
        currentTextUI = currentGO.AddComponent<TextMeshProUGUI>();
        currentTextUI.fontSize = 48;
        currentTextUI.alignment = TextAlignmentOptions.Left;
        currentTextUI.color = Color.green;
        if (fontAsset != null) currentTextUI.font = fontAsset;
        currentGO.GetComponent<RectTransform>().anchoredPosition3D = currentTextPosition;
        currentGO.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 50);
    }


    private void UpdateSequenceDisplay()
    {
        if (expectedTextUI != null)
            expectedTextUI.text = "Expected: [" + string.Join(", ", expectedSequence) + "]";

        if (currentTextUI != null)
            currentTextUI.text = "Current: [" + string.Join(", ", currentSequence) + "]";
    }


    private void ResetSequence()
    {
        currentSequence.Clear();
        sequenceCompleted = false;
        UpdateSequenceDisplay();
    }

}
