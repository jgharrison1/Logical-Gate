using System.Collections.Generic;
using UnityEngine;

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

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
    private playerMovement playerMovementScript;

    private void Start()
    {
        playerMovementScript = FindObjectOfType<playerMovement>();
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
}
