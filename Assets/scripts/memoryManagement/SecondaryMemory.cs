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

    public GameObject GetBlockInSlot(GameObject slot)
    {
        if (slotToBlockMap.ContainsKey(slot))
        {
            return slotToBlockMap[slot];
        }
        return null;  // If the slot is empty, return null
    }


    public GameObject RemoveBlockFromSlot(GameObject slot)
    {
        if (!IsValidSlot(slot) || !slotToBlockMap.ContainsKey(slot))
        {
            return null;  // Return null if slot is invalid or empty
        }

        GameObject block = slotToBlockMap[slot];

        slotToBlockMap.Remove(slot);

        block.transform.SetParent(null);  
        block.SetActive(false);  

        Debug.Log($"Block {block.name} removed from slot {slot.name}. Slot is now free.");
        return block;  // Return the block so it can be reused
    }

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (slot == null || block == null || !IsValidSlot(slot))
        {
            return false;
        }

        slotToBlockMap[slot] = block;
        block.transform.SetParent(slot.transform);  // Attach the block to the slot
        block.transform.localPosition = Vector3.zero;  // Align the block with the slot's position

        Debug.Log($"Block {block.name} added to slot {slot.name}");
        return true;  // Successful placement
    }

    public bool IsSlotOccupied(GameObject slot, GameObject block)
    {
        // Returns true if the slot is occupied by another block, but not the same block
        return slotToBlockMap.ContainsKey(slot) && slotToBlockMap[slot] != block;
    }

    private bool IsValidSlot(GameObject slot)
    {
        return slot != null && slots.Contains(slot);
    }
}
