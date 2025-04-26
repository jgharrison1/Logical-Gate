using System.Collections.Generic;
using UnityEngine;
using System;

public class pageTable : MonoBehaviour
{
    [Header("Page Number Slots")]
    public List<GameObject> pageSlots = new List<GameObject>();

    [Header("Frame Number Slots")]
    public List<GameObject> frameSlots = new List<GameObject>();

    [Header("Page Blocks (p)")]
    public List<GameObject> pageBlocks = new List<GameObject>();

    [Header("Frame Blocks (f)")]
    public List<GameObject> frameBlocks = new List<GameObject>();

    [Header("Runtime Assignment")]
    public List<GameObject> blocksToAssign = new List<GameObject>();
    public List<int> slotIndicesToAssign = new List<int>();
    public List<BlockType.Type> slotTypesToAssign = new List<BlockType.Type>();

    [Header("Block Color Changers")]
    public List<BlockColorChanger> blockColorChangers = new List<BlockColorChanger>();

    [Header("Target Values")]
    public List<string> targetValues = new List<string>(); // Now storing binary strings

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
    private playerMovement playerMovementScript;

    private void Start()
    {
        playerMovementScript = FindObjectOfType<playerMovement>();

        if (blocksToAssign.Count > 0)
        {
            AssignAllBlocksFromInspector();
        }

        UpdateBlockColorsOnStart();
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
    }

    private void AssignBlockToSlot(int index)
    {
        if (index < 0 || index >= blocksToAssign.Count) return;

        GameObject block = blocksToAssign[index];
        int slotIndex = slotIndicesToAssign[index];
        BlockType.Type slotType = slotTypesToAssign[index];

        if (block == null) return;

        List<GameObject> slotList = slotType == BlockType.Type.PageNumber ? pageSlots : frameSlots;

        if (slotIndex >= slotList.Count) return;

        GameObject slot = slotList[slotIndex];
        bool success = TryAddBlockToSlot(slot, block);

        if (!success)
        {
            Debug.LogWarning($"Failed to assign {block.name} to {slot.name}.");
        }
    }

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (slot == null || block == null || !IsValidSlot(slot))
            return false;

        BlockType blockType = block.GetComponent<BlockType>();
        if (blockType == null) return false;

        if (pageSlots.Contains(slot) && blockType.blockType != BlockType.Type.PageNumber)
            return false;
        if (frameSlots.Contains(slot) && blockType.blockType != BlockType.Type.FrameNumber)
            return false;

        slotToBlockMap[slot] = block;
        block.transform.SetParent(slot.transform);
        block.transform.localPosition = Vector3.zero;

        if (blockType.blockType == BlockType.Type.PageNumber)
        {
            HandleFrameTeleport(block);
        }
        else if (blockType.blockType == BlockType.Type.FrameNumber)
        {
            HandlePageTeleport(block);
        }

        ValidatePageFramePairs();
        return true;
    }

    private void HandleFrameTeleport(GameObject pageBlock)
    {
        int index = pageBlocks.IndexOf(pageBlock);

        if (index != -1 && index < frameBlocks.Count && index < frameSlots.Count)
        {
            GameObject frameBlock = frameBlocks[index];
            GameObject frameSlot = frameSlots[index];

            if (frameBlock != null && frameSlot != null)
            {
                frameBlock.transform.position = frameSlot.transform.position;
                frameBlock.transform.SetParent(frameSlot.transform);
            }
        }
    }

    private void HandlePageTeleport(GameObject frameBlock)
    {
        int index = frameBlocks.IndexOf(frameBlock);

        if (index != -1 && index < pageBlocks.Count && index < pageSlots.Count)
        {
            GameObject pageBlock = pageBlocks[index];
            GameObject pageSlot = pageSlots[index];

            if (pageBlock != null && pageSlot != null)
            {
                pageBlock.transform.position = pageSlot.transform.position;
                pageBlock.transform.SetParent(pageSlot.transform);
            }
        }
    }

    private bool IsValidSlot(GameObject slot)
    {
        return pageSlots.Contains(slot) || frameSlots.Contains(slot);
    }

    public GameObject GetBlockInSlot(GameObject slot)
    {
        return slotToBlockMap.ContainsKey(slot) ? slotToBlockMap[slot] : null;
    }

private void ValidatePageFramePairs()
{
    for (int i = 0; i < pageSlots.Count && i < frameSlots.Count && i < targetValues.Count; i++)
    {
        GameObject pageBlock = GetBlockInSlot(pageSlots[i]);
        GameObject frameBlock = GetBlockInSlot(frameSlots[i]);

        int pageValue = (pageBlock != null && pageBlock.GetComponent<BlockType>() != null)
            ? pageBlock.GetComponent<BlockType>().addressValue
            : 0;

        int frameValue = (frameBlock != null && frameBlock.GetComponent<BlockType>() != null)
            ? frameBlock.GetComponent<BlockType>().addressValue
            : 0;

        int sum = pageValue + frameValue;

        // Ensure targetValues[i] is treated as an integer
        int targetValueAsInt = 0;

        if (targetValues[i] is string binaryString)
        {
            // Convert binary string to integer
            targetValueAsInt = Convert.ToInt32(binaryString, 2);
        }

        // Now set the color changers based on the sum and the target value
        if (blockColorChangers.Count > i)
        {
            blockColorChangers[i].SetTargetValue(sum, targetValueAsInt);  // Second argument must be an integer
        }

        // Ensure that the lights are turned on for the blocks that match the conditions
        if (pageBlock != null && pageBlocks.Contains(pageBlock))
        {
            int blockIndex = pageBlocks.IndexOf(pageBlock);
            if (blockIndex < blockColorChangers.Count)
            {
                blockColorChangers[blockIndex].TurnOn();
            }
        }
    }
}

private void UpdateBlockColorsOnStart()
{
    // Ensure all block lights are turned off initially
    foreach (var changer in blockColorChangers)
    {
        changer.TurnOff(); // Add this line to turn off all lights initially
    }

    // Now call ValidatePageFramePairs to set them according to the values
    ValidatePageFramePairs();
}
}
