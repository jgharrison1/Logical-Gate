using System.Collections.Generic;
using UnityEngine;

public class secondaryMemory : MonoBehaviour
{
    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();

    /// <summary>
    /// Tries to add a block to the given slot.
    /// </summary>
    /// <param name="slot">The slot GameObject to place the block in.</param>
    /// <param name="block">The block GameObject to place in the slot.</param>
    /// <returns>True if the block was successfully added; otherwise, false.</returns>

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (slot == null || block == null)
        {
            Debug.LogError("Slot or block is null.");
            return false;
        }

        Debug.Log($"Attempting to place block: {block.name} into slot: {slot.name}");

        // Check if the slot is already occupied
        if (slotToBlockMap.ContainsKey(slot))
        {
            Debug.LogWarning("Slot is already occupied.");
            return false;
        }

        // Add the block to the slot
        slotToBlockMap[slot] = block;

        // Parent the block to the slot for proper positioning and hierarchy
        block.transform.SetParent(slot.transform);
        block.transform.position = slot.transform.position;

        Debug.Log("Block successfully added to slot.");
        return true;
    }

    // public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    // {
    //     if (slot == null || block == null)
    //     {
    //         Debug.Log("Slot or block is null.");
    //         return false;
    //     }

    //     Debug.Log($"Attempting to place block: {block.name} into slot: {slot.name}");

    //     // Check if the slot is already occupied
    //     if (slotToBlockMap.ContainsKey(slot))
    //     {
    //         Debug.Log("Slot is already occupied.");
    //         return false;
    //     }

    //     // Add the block to the slot
    //     slotToBlockMap[slot] = block;

    //     // Parent the block to the slot for proper positioning and hierarchy
    //     block.transform.SetParent(slot.transform);
    //     block.transform.position = slot.transform.position;

    //     Debug.Log("Block successfully added to slot.");
    //     return true;
    // }


    /// <summary>
    /// Removes a block from the given slot.
    /// </summary>
    /// <param name="slot">The slot GameObject to remove the block from.</param>
    /// <returns>The block GameObject if removed, otherwise null.</returns>
    
    public GameObject RemoveBlockFromSlot(GameObject slot)
    {
        if (slot == null)
        {
            Debug.LogError("Slot is null.");
            return null; // Return null if the slot is invalid
        }

        if (!slotToBlockMap.ContainsKey(slot))
        {
            Debug.LogWarning("No block to remove from this slot.");
            return null; // Return null if the slot is empty
        }

        GameObject block = slotToBlockMap[slot];
        slotToBlockMap.Remove(slot);

        // Detach the block from the slot
        block.transform.SetParent(null);

        Debug.Log($"Block {block.name} removed from slot {slot.name}.");
        return block; // Return the removed block
    }

    // public GameObject RemoveBlockFromSlot(GameObject slot)
    // {
    //     if (slot == null)
    //     {
    //         Debug.Log("Slot is null.");
    //         return null; // Return null if the slot is invalid
    //     }

    //     if (!slotToBlockMap.ContainsKey(slot))
    //     {
    //         Debug.Log("No block to remove from this slot.");
    //         return null; // Return null if the slot is empty
    //     }

    //     GameObject block = slotToBlockMap[slot];
    //     slotToBlockMap.Remove(slot);

    //     // Detach the block from the slot
    //     block.transform.SetParent(null);
    //     Debug.Log($"Block {block.name} removed from slot {slot.name}.");
    //     return block; // Return the removed block
    // }


    /// <summary>
    /// Checks if a slot is occupied.
    /// </summary>
    /// <param name="slot">The slot to check.</param>
    /// <returns>True if the slot is occupied; otherwise, false.</returns>
    public bool IsSlotOccupied(GameObject slot)
    {
        return slot != null && slotToBlockMap.ContainsKey(slot);
    }

    /// <summary>
    /// Retrieves the block currently in a slot.
    /// </summary>
    /// <param name="slot">The slot to retrieve the block from.</param>
    /// <returns>The block GameObject if it exists; otherwise, null.</returns>
    public GameObject GetBlockInSlot(GameObject slot)
    {
        if (slot == null || !slotToBlockMap.ContainsKey(slot))
        {
            return null;
        }

        return slotToBlockMap[slot];
    }

    /// <summary>
    /// Clears all slots and blocks.
    /// </summary>
    public void ClearAllSlots()
    {
        foreach (var kvp in slotToBlockMap)
        {
            GameObject block = kvp.Value;
            block.transform.SetParent(null);
        }

        slotToBlockMap.Clear();
        Debug.Log("All slots cleared.");
    }
}
