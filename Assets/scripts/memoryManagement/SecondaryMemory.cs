using System.Collections.Generic;
using UnityEngine;

public class secondaryMemory : MonoBehaviour
{
    [Header("Page Number Slots")]
    public List<GameObject> pageSlots = new List<GameObject>(); // Slots for page number blocks

    [Header("Offset Slots")]
    public List<GameObject> offsetSlots = new List<GameObject>(); // Slots for offset blocks

    [Header("Target Values")]
    public List<int> targetValues = new List<int>(); // List of target values for each page-offset pair

    [Header("Block Color Changers")]
    public List<BlockColorChanger> blockColorChangers = new List<BlockColorChanger>();

    // Make this field visible in the Inspector
    [Header("Debugging: Pair Match Status")]
    public List<bool> isMatchList = new List<bool>(); // List to store match status for each pair

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();

    private playerMovement playerMovementScript;  // Reference to the playerMovement script

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

        // After removing the block, check if it was from the 0th slot (either page or offset)
        if (pageSlots[0] == slot || offsetSlots[0] == slot)
        {
            // Set the match status for the 0th element to false
            if (isMatchList.Count > 0)
            {
                isMatchList[0] = false;
                Debug.Log("isMatchList[0] set to false because a block was removed from slot 0");
            }

            // Update the color to blue
            if (blockColorChangers.Count > 0)
            {
                blockColorChangers[0].SetTargetValue(0, targetValues[0]);
            }
        }

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

        // Trigger the match check after adding the block
        ValidatePageOffsetPairs(); // Call to validate all pairs
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

    // Validate and compare the sum of address values and target value
    public void ValidatePageOffsetPairs()
    {
        for (int i = 0; i < pageSlots.Count && i < offsetSlots.Count; i++)
        {
            ValidatePageOffsetPair(i); // Validate for each pair of page and offset blocks
        }
    }

    public void ValidatePageOffsetPair(int index)
    {
        if (index < 0 || index >= targetValues.Count) return;

        // Get the blocks in the current page and offset slots
        GameObject pageBlock = GetBlockInSlot(pageSlots[index]);
        GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

        // Check if both blocks exist
        if (pageBlock != null && offsetBlock != null)
        {
            BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
            BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

            // Get the address values from the blocks
            int pageValue = pageBlockType != null ? pageBlockType.addressValue : 0;
            int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;

            // Calculate the sum of the values
            int sum = pageValue + offsetValue;

            // Debug: Log values being compared
            Debug.Log($"Checking Pair {index}: Page Value = {pageValue}, Offset Value = {offsetValue}, Sum = {sum}, Target = {targetValues[index]}");

            // Check if sum matches the target value
            bool isMatch = (sum == targetValues[index]);

            // Log if the values match or not
            if (isMatch)
            {
                Debug.Log($"Pair {index} matches the target value!");
            }
            else
            {
                Debug.Log($"Pair {index} does NOT match the target value.");
            }

            // Store the match result in the list for display in the Inspector
            if (isMatchList.Count > index)
            {
                isMatchList[index] = isMatch; // Update the match status for this pair
            }

            // Trigger color change based on the result
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(sum, targetValues[index]);
            }
        }
        else
        {
            // If either block is missing, do not check the match status
            Debug.LogWarning($"Pair {index} is missing either page or offset block! Match status cannot be checked.");
            
            // Set the "isMatch" to false if any block is missing
            if (isMatchList.Count > index)
            {
                isMatchList[index] = false;
            }

            // Update the color to blue if blocks are missing
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(0, targetValues[index]); // Set to blue (default) if missing
            }
        }
    }
}
