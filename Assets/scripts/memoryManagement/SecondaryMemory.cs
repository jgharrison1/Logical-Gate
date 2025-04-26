using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class secondaryMemory : MonoBehaviour
{
    [Header("Page Number Slots")]
    public List<GameObject> pageSlots = new List<GameObject>();

    [Header("Offset Slots")]
    public List<GameObject> offsetSlots = new List<GameObject>();

    [Header("Target Values (Binary String)")]
    public List<string> targetValuesBinary = new List<string>(); // Now a list of binary strings

    [Header("Block Color Changers")]
    public List<BlockColorChanger> blockColorChangers = new List<BlockColorChanger>();

    [Header("Runtime Assignment")]
    public List<GameObject> blocksToAssign = new List<GameObject>();
    public List<int> slotIndicesToAssign = new List<int>();
    public List<BlockType.Type> slotTypesToAssign = new List<BlockType.Type>();

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
    private playerMovement playerMovementScript;

    private GameObject textPrefab3D; // A prefab with a TextMesh or TextMeshPro component
    private Vector3 labelOffset = new Vector3(-0.5f, 0f, 0f); // Adjust for spacing to the left
    private List<GameObject> valueLabels = new List<GameObject>();

    private void Start()
    {
        if (blocksToAssign.Count > 0)
        {
            for (int i = 0; i < blocksToAssign.Count; i++)
            {
                if (blocksToAssign[i] != null)
                {
                    AssignBlockToSlot(i);
                }
            }
            ValidatePageOffsetPairs();
            UpdateBlockColorsOnStart();

        }
        CreateTargetValueDisplays();
    }

    public void RegisterSlot(GameObject slot, BlockType.Type slotType)
    {
        if (slot == null) return;

        if (slotType == BlockType.Type.PageNumber && !pageSlots.Contains(slot))
        {
            pageSlots.Add(slot);
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

        if (pageSlots[0] == slot || offsetSlots[0] == slot)
        {
            if (blockColorChangers.Count > 0)
            {
                blockColorChangers[0].SetTargetValue(0, GetTargetValue(0)); // Use GetTargetValue to get integer
            }
        }

        return block;
    }

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (slot == null || block == null || !IsValidSlot(slot)) return false;

        BlockType blockType = block.GetComponent<BlockType>();
        if (blockType == null) return false;

        // Check if block type matches slot type
        bool isCorrectSlot = (pageSlots.Contains(slot) && blockType.blockType == BlockType.Type.PageNumber) ||
                            (offsetSlots.Contains(slot) && blockType.blockType == BlockType.Type.Offset);

        if (!isCorrectSlot)
        {
            // If the block is not of the correct type for the slot, reset its color to blue.
            if (blockColorChangers.Count > 0)
            {
                int blockIndex = pageSlots.IndexOf(slot); // Or offsetSlots if it's an offset slot.
                if (blockIndex != -1)
                {
                    blockColorChangers[blockIndex].TurnOff();
                }
            }

            if (playerMovementScript != null)
            {
                block.transform.SetParent(playerMovementScript.transform);
                block.transform.position = playerMovementScript.holdPosition.position;
            }

            block.SetActive(true);
            return false;
        }

        slotToBlockMap[slot] = block;
        block.transform.SetParent(slot.transform);
        block.transform.localPosition = Vector3.zero;

        ValidatePageOffsetPairs();
        return true;
    }

    public bool IsSlotOccupied(GameObject slot, GameObject block)
    {
        return slotToBlockMap.ContainsKey(slot) && slotToBlockMap[slot] != block;
    }

    private bool IsValidSlot(GameObject slot)
    {
        return slot != null && (pageSlots.Contains(slot) || offsetSlots.Contains(slot));
    }

    public void ValidatePageOffsetPairs()
    {
        for (int i = 0; i < pageSlots.Count && i < offsetSlots.Count; i++)
        {
            ValidatePageOffsetPair(i);
        }
    }

    public void ValidatePageOffsetPair(int index)
    {
        if (index < 0 || index >= targetValuesBinary.Count) return;

        GameObject pageBlock = GetBlockInSlot(pageSlots[index]);
        GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

        if (pageBlock != null && offsetBlock != null)
        {
            BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
            BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

            string pageBinaryValue = pageBlockType != null ? pageBlockType.binaryAddressValue : "0";
            string offsetBinaryValue = offsetBlockType != null ? offsetBlockType.binaryAddressValue : "0";

            string combinedBinary = pageBinaryValue + offsetBinaryValue;
            string targetBinaryValue = targetValuesBinary[index];

            if (blockColorChangers.Count > index)
            {
                bool isMatch = combinedBinary == targetBinaryValue;
                if (!isMatch)
                {
                    blockColorChangers[index].TurnOff(); // Set color to blue if mismatch
                }
                else
                {
                    blockColorChangers[index].TurnOn(); // Turn yellow if match
                }
            }
        }
        else
        {
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].TurnOff(); // Default to blue if no blocks
            }
        }
    }

    // Converts the binary string of target value at index to an integer.
    public int GetTargetValue(int index)
    {
        if (index >= 0 && index < targetValuesBinary.Count)
        {
            string binaryValue = targetValuesBinary[index];
            // Ensure the binary string is valid and convert it.
            int targetValue = 0;
            if (!string.IsNullOrEmpty(binaryValue))
            {
                try
                {
                    // Ensure the binary value is properly formatted
                    targetValue = System.Convert.ToInt32(binaryValue, 2);
                }
                catch (System.FormatException)
                {
                    Debug.LogError($"Invalid binary format: {binaryValue} at index {index}");
                }
            }
            return targetValue;
        }
        return 0; // Return 0 if index is invalid
    }

    private void CreateTargetValueDisplays()
    {
        for (int i = 0; i < pageSlots.Count && i < targetValuesBinary.Count; i++)
        {
            GameObject slot = pageSlots[i];

            GameObject textObj = new GameObject($"TargetValueText_{i}");
            textObj.transform.SetParent(slot.transform);
            textObj.transform.localPosition = new Vector3(-1.8f, 0f, 0f);

            TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
            tmp.text = targetValuesBinary[i]; // Display binary string
            tmp.fontSize = 5;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.yellow;

            Renderer renderer = tmp.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 10;
            }
        }
    }

    public void AssignAllBlocksFromInspector()
    {
        for (int i = 0; i < blocksToAssign.Count; i++)
        {
            if (blocksToAssign[i] != null)
            {
                AssignBlockToSlot(i);
            }
        }

        ValidatePageOffsetPairs();
    }

    private void AssignBlockToSlot(int index)
    {
        if (index < 0 || index >= blocksToAssign.Count) return;

        GameObject block = blocksToAssign[index];
        int slotIndex = slotIndicesToAssign[index];
        BlockType.Type slotType = slotTypesToAssign[index];

        if (block == null) return;

        List<GameObject> slotList = slotType == BlockType.Type.PageNumber ? pageSlots : offsetSlots;

        if (slotIndex >= slotList.Count)
        {
            Debug.LogWarning($"[secondaryMemory] Slot index {slotIndex} exceeds the available number of {slotType} slots ({slotList.Count}). Block '{block.name}' was not assigned.");
            return;
        }

        GameObject slot = slotList[slotIndex];
        bool success = TryAddBlockToSlot(slot, block);

        if (success)
        {
            block.transform.SetParent(slot.transform);
            block.transform.position = slot.transform.position;
            block.transform.localRotation = Quaternion.identity;
            ValidatePageOffsetPairs();
        }

        // Call SetTargetValue to update the color immediately after placing the block
        if (blockColorChangers.Count > index)
        {
            GameObject pageBlock = GetBlockInSlot(pageSlots[index]);
            GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

            if (pageBlock != null && offsetBlock != null)
            {
                BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
                BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

                int pageValue = pageBlockType != null ? pageBlockType.addressValue : 0;
                int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
                int sum = pageValue + offsetValue;

                blockColorChangers[index].SetTargetValue(sum, GetTargetValue(index));
            }
        }
    }

    private void UpdateBlockColorsOnStart()
    {
        for (int i = 0; i < pageSlots.Count && i < offsetSlots.Count; i++)
        {
            ValidatePageOffsetPair(i);

            if (blockColorChangers.Count > i)
            {
                GameObject pageBlock = GetBlockInSlot(pageSlots[i]);
                GameObject offsetBlock = GetBlockInSlot(offsetSlots[i]);

                if (pageBlock != null && offsetBlock != null)
                {
                    BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
                    BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

                    string pageBinary = pageBlockType != null ? pageBlockType.binaryAddressValue : "0";
                    string offsetBinary = offsetBlockType != null ? offsetBlockType.binaryAddressValue : "0";

                    string combinedBinary = pageBinary + offsetBinary;
                    string targetBinary = targetValuesBinary[i];

                    bool isMatch = combinedBinary == targetBinary;
                    blockColorChangers[i].SetTargetValue(isMatch ? 1 : 0, 1); 
                    // Trick: if match, set (1,1) => yellow; if mismatch, set (0,1) => blue
                }
                else
                {
                    blockColorChangers[i].TurnOff(); // Default to blue if missing blocks
                }
            }
        }
    }

}
