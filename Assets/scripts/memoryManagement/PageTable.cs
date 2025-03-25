// using System.Collections.Generic;
// using UnityEngine;

// public class pageTable : MonoBehaviour
// {
//     [Header("Page Number Slots")]
//     public List<GameObject> pageSlots = new List<GameObject>();

//     [Header("Frame Number Slots")]
//     public List<GameObject> frameSlots = new List<GameObject>();

//     private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();

//     private playerMovement playerMovementScript;

//     private void Start()
//     {
//         playerMovementScript = FindObjectOfType<playerMovement>();
//     }

//     public void RegisterSlot(GameObject slot, BlockType.Type slotType)
//     {
//         if (slot == null) return;

//         if (slotType == BlockType.Type.PageNumber && !pageSlots.Contains(slot))
//         {
//             pageSlots.Add(slot);
//         }
//         else if (slotType == BlockType.Type.FrameNumber && !frameSlots.Contains(slot))
//         {
//             frameSlots.Add(slot);
//         }
//     }

//     public GameObject GetBlockInSlot(GameObject slot)
//     {
//         return slotToBlockMap.ContainsKey(slot) ? slotToBlockMap[slot] : null;
//     }

//     public bool TryAddBlockToSlot(GameObject slot, GameObject block)
//     {
//         if (slot == null || block == null || !IsValidSlot(slot))
//         {
//             return false;
//         }

//         BlockType blockType = block.GetComponent<BlockType>();
//         if (blockType == null) return false;

//         // Ensure block type matches slot type
//         if ((pageSlots.Contains(slot) && blockType.blockType != BlockType.Type.PageNumber) ||
//             (frameSlots.Contains(slot) && blockType.blockType != BlockType.Type.FrameNumber))
//         {
//             block.transform.SetParent(playerMovementScript.transform);
//             block.transform.position = playerMovementScript.holdPosition.position;
//             block.SetActive(true);
//             return false;
//         }

//         slotToBlockMap[slot] = block;
//         block.transform.SetParent(slot.transform);
//         block.transform.localPosition = Vector3.zero;

//         return true;
//     }

//     public GameObject RemoveBlockFromSlot(GameObject slot)
//     {
//         if (!IsValidSlot(slot) || !slotToBlockMap.ContainsKey(slot))
//         {
//             return null;
//         }

//         GameObject block = slotToBlockMap[slot];
//         slotToBlockMap.Remove(slot);
//         block.transform.SetParent(null);
//         block.SetActive(false);

//         return block;
//     }

//     private bool IsValidSlot(GameObject slot)
//     {
//         return slot != null && (pageSlots.Contains(slot) || frameSlots.Contains(slot));
//     }
// }

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

        slotToBlockMap[slot] = block;
        block.transform.SetParent(slot.transform);
        block.transform.localPosition = Vector3.zero;

        HandleFrameTeleport(block);

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

    private bool IsValidSlot(GameObject slot)
    {
        return pageSlots.Contains(slot) || frameSlots.Contains(slot);
    }

    public GameObject GetBlockInSlot(GameObject slot)
    {
        return slotToBlockMap.ContainsKey(slot) ? slotToBlockMap[slot] : null;
    }
}
