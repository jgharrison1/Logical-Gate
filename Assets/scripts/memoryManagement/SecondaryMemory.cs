// using System.Collections.Generic;
// using UnityEngine;

// public class secondaryMemory : MonoBehaviour
// {
//     [Header("Slots")]
//     public List<GameObject> slots = new List<GameObject>(); // Expose slots in Inspector
//     private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();

//     /// <summary>
//     /// Registers a slot to the secondary memory.
//     /// </summary>
//     /// <param name="slot">The slot GameObject to register.</param>
//     public void RegisterSlot(GameObject slot)
//     {
//         if (slot != null && !slots.Contains(slot))
//         {
//             slots.Add(slot);
//             Debug.Log($"Slot {slot.name} registered to secondary memory.");
//         }
//     }

//     /// <summary>
//     /// Unregisters a slot from the secondary memory.
//     /// </summary>
//     /// <param name="slot">The slot GameObject to unregister.</param>
//     public void UnregisterSlot(GameObject slot)
//     {
//         if (slot != null && slots.Contains(slot))
//         {
//             slots.Remove(slot);
//             Debug.Log($"Slot {slot.name} unregistered from secondary memory.");
//         }
//     }

//     /// <summary>
//     /// Gets all registered slots.
//     /// </summary>
//     /// <returns>A list of all registered slots.</returns>
//     public List<GameObject> GetRegisteredSlots()
//     {
//         return new List<GameObject>(slots);
//     }

//     /// <summary>
//     /// Tries to add a block to the given slot.
//     /// </summary>
//     /// <param name="slot">The slot GameObject to place the block in.</param>
//     /// <param name="block">The block GameObject to place in the slot.</param>
//     /// <returns>True if the block was successfully added; otherwise, false.</returns>

//     public bool TryAddBlockToSlot(GameObject slot, GameObject block)
//     {
//         if (slot == null || block == null)
//         {
//             Debug.LogError("Slot or block is null.");
//             return false;
//         }

//         Debug.Log($"Attempting to place block: {block.name} into slot: {slot.name}");

//         // Check if the slot is already occupied by another block in the memory map
//         if (slotToBlockMap.ContainsKey(slot) && slotToBlockMap[slot] != null)
//         {
//             // If there's already a block in this slot, we can't place another one
//             Debug.LogWarning("Slot is already occupied.");
//             return false;
//         }

//         // Now check if the slot actually has a block in it by checking the slot's children
//         if (slot.transform.childCount > 0)
//         {
//             // If the slot has a child (block), we can't place another one
//             Debug.LogWarning("Slot is already occupied (via child).");
//             return false;
//         }

//         // Add the block to the slot in the slot-to-block map
//         slotToBlockMap[slot] = block;

//         // Parent the block to the slot for proper positioning and hierarchy
//         block.transform.SetParent(slot.transform);
//         block.transform.position = slot.transform.position;

//         Debug.Log("Block successfully added to slot.");
//         return true;
//     }



//     /// <summary>
//     /// Removes a block from the given slot.
//     /// </summary>
//     /// <param name="slot">The slot GameObject to remove the block from.</param>
//     /// <returns>The block GameObject if removed, otherwise null.</returns>
//     public GameObject RemoveBlockFromSlot(GameObject slot)
//     {
//         if (slot == null)
//         {
//             Debug.LogError("Slot is null.");
//             return null; // Return null if the slot is invalid
//         }

//         if (!slotToBlockMap.ContainsKey(slot))
//         {
//             Debug.LogWarning("No block to remove from this slot.");
//             return null; // Return null if the slot is empty
//         }

//         GameObject block = slotToBlockMap[slot];
//         slotToBlockMap.Remove(slot);

//         // Detach the block from the slot
//         block.transform.SetParent(null);

//         Debug.Log($"Block {block.name} removed from slot {slot.name}.");
//         return block; // Return the removed block
//     }

//     /// <summary>
//     /// Checks if a slot is occupied.
//     /// </summary>
//     /// <param name="slot">The slot to check.</param>
//     /// <returns>True if the slot is occupied; otherwise, false.</returns>
//     public bool IsSlotOccupied(GameObject slot)
//     {
//         return slot != null && slotToBlockMap.ContainsKey(slot);
//     }

//     /// <summary>
//     /// Retrieves the block currently in a slot.
//     /// </summary>
//     /// <param name="slot">The slot to retrieve the block from.</param>
//     /// <returns>The block GameObject if it exists; otherwise, null.</returns>
//     public GameObject GetBlockInSlot(GameObject slot)
//     {
//         if (slot == null || !slotToBlockMap.ContainsKey(slot))
//         {
//             return null;
//         }

//         return slotToBlockMap[slot];
//     }

//     /// <summary>
//     /// Clears all slots and blocks.
//     /// </summary>
//     public void ClearAllSlots()
//     {
//         foreach (var kvp in slotToBlockMap)
//         {
//             GameObject block = kvp.Value;
//             block.transform.SetParent(null);
//         }

//         slotToBlockMap.Clear();
//         slots.Clear();
//         Debug.Log("All slots cleared.");
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public class secondaryMemory : MonoBehaviour
{
    [Header("Slots")]
    public List<GameObject> slots = new List<GameObject>(); // Expose slots in Inspector
    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();

    public void RegisterSlot(GameObject slot)
    {
        if (slot != null && !slots.Contains(slot))
        {
            slots.Add(slot);
            Debug.Log($"Slot {slot.name} registered.");
        }
    }

    public void UnregisterSlot(GameObject slot)
    {
        if (slot != null && slots.Remove(slot))
        {
            Debug.Log($"Slot {slot.name} unregistered.");
        }
    }

    public List<GameObject> GetRegisteredSlots() => new List<GameObject>(slots);

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (!IsValidSlot(slot) || block == null)
        {
            Debug.LogError("Invalid slot or block.");
            return false;
        }

        if (slotToBlockMap.ContainsKey(slot))
        {
            Debug.LogWarning("Slot is already occupied.");
            return false;
        }

        if (slot.transform.childCount > 0)
        {
            Debug.LogWarning("Slot already contains a child object.");
            return false;
        }

        slotToBlockMap[slot] = block;
        block.transform.SetParent(slot.transform);
        block.transform.localPosition = Vector3.zero;

        Debug.Log($"Block {block.name} added to slot {slot.name}.");
        return true;
    }

    public GameObject RemoveBlockFromSlot(GameObject slot)
    {
        if (!IsValidSlot(slot) || !slotToBlockMap.TryGetValue(slot, out var block))
        {
            Debug.LogWarning("No block found in this slot.");
            return null;
        }

        slotToBlockMap.Remove(slot);

        if (block != null)
        {
            block.transform.SetParent(null);
            Debug.Log($"Block {block.name} removed from slot {slot.name}.");
        }

        return block;
    }

    public bool IsSlotOccupied(GameObject slot) => slot != null && slotToBlockMap.ContainsKey(slot);

    public GameObject GetBlockInSlot(GameObject slot) => slotToBlockMap.TryGetValue(slot, out var block) ? block : null;

    public void ClearAllSlots()
    {
        foreach (var slot in slotToBlockMap.Keys)
        {
            var block = slotToBlockMap[slot];
            if (block != null) block.transform.SetParent(null);
        }

        slotToBlockMap.Clear();
        slots.Clear();
        Debug.Log("All slots cleared.");
    }

    private bool IsValidSlot(GameObject slot) => slot != null && slots.Contains(slot);
}
