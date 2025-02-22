using System.Collections.Generic;
using UnityEngine;

public class secondaryMemory : MonoBehaviour
{
    [Header("Page Number Slots")]
    public List<GameObject> pageSlots = new List<GameObject>(); // Slots for page number blocks

    [Header("Offset Slots")]
    public List<GameObject> offsetSlots = new List<GameObject>(); // Slots for offset blocks

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();

    public playerMovement playerMovementScript;  // Reference to the playerMovement script

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

        Debug.Log($"Slot {slot.name} registered as {slotType}.");
    }

    public void UnregisterSlot(GameObject slot)
    {
        if (slot == null) return;

        if (pageSlots.Remove(slot) || offsetSlots.Remove(slot))
        {
            Debug.Log($"Slot {slot.name} unregistered.");
        }
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

        Debug.Log($"Block {block.name} removed from slot {slot.name}. Slot is now free.");
        return block;
    }

public bool TryAddBlockToSlot(GameObject slot, GameObject block)
{
    if (slot == null || block == null || !IsValidSlot(slot))
    {
        return false;
    }

    BlockType blockType = block.GetComponent<BlockType>();
    if (blockType == null)
    {
        Debug.LogError($"Block {block.name} is missing BlockType script!");
        return false;
    }

    // Check if block matches the column type
    if ((pageSlots.Contains(slot) && blockType.blockType != BlockType.Type.PageNumber) ||
        (offsetSlots.Contains(slot) && blockType.blockType != BlockType.Type.Offset))
    {
        Debug.LogWarning($"Block {block.name} does not match slot type {slot.name}.");

        // If types don't match, place the block above the player's head and make it follow the player
        block.transform.SetParent(playerMovementScript.transform);  // Attach block to the player
        block.transform.position = playerMovementScript.holdPosition.position;  // Set position to above player's head
        block.SetActive(true);  // Ensure block is active (if necessary)

        return false;  // Return false to indicate placement failed
    }

    // If block type matches the slot type, proceed to place it in the slot
    slotToBlockMap[slot] = block;
    block.transform.SetParent(slot.transform);
    block.transform.localPosition = Vector3.zero;

    Debug.Log($"Block {block.name} added to slot {slot.name}");
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
}
