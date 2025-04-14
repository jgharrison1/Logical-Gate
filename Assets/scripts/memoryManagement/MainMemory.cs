using System.Collections.Generic;
using UnityEngine;

public class mainMemory : MonoBehaviour
{
    [Header("Frame Number Slots")]
    public List<GameObject> frameSlots = new List<GameObject>();

    [Header("Offset Slots")]
    public List<GameObject> offsetSlots = new List<GameObject>(); 

    [Header("Target Values")]
    public List<int> targetValues = new List<int>(); 

    [Header("Block Color Changers")]
    public List<BlockColorChanger> blockColorChangers = new List<BlockColorChanger>();

    [Header("Runtime Assignment")]
    public List<GameObject> blocksToAssign = new List<GameObject>();
    public List<int> slotIndicesToAssign = new List<int>(); 
    public List<BlockType.Type> slotTypesToAssign = new List<BlockType.Type>(); 

    [Header("Sequence Validation")]
    public List<int> expectedSequence = new List<int>();  // Define in Unity Inspector
    private List<int> currentSequence = new List<int>();
    public bool sequenceCompleted = false; // Outputs true if correct sequence is achieved

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
    private playerMovement playerMovementScript;

    private void Start()
    {
        if (blocksToAssign.Count > 0)
        {
            for (int i = 0; i < blocksToAssign.Count; i++)
            {
                if (blocksToAssign[i] != null) { AssignBlockToSlot(i); }
            }
            ValidateFrameOffsetPairs();
        }
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
            blockColorChangers[0].SetTargetValue(0, targetValues[0]);
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
        if (blockType == null) { return false; }

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
        if (index < 0 || index >= targetValues.Count) return;

        GameObject frameBlock = GetBlockInSlot(frameSlots[index]);
        GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

        if (frameBlock != null && offsetBlock != null)
        {
            BlockType frameBlockType = frameBlock.GetComponent<BlockType>();
            BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();
            int frameValue = frameBlockType != null ? frameBlockType.addressValue : 0;
            int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
            int sum = frameValue + offsetValue;

            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(sum, targetValues[index]);
            }
        }
        else
        {
            if (blockColorChangers.Count > index)
            {
                blockColorChangers[index].SetTargetValue(0, targetValues[index]);
            }
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
        for (int i = 0; i < frameSlots.Count && i < offsetSlots.Count; i++)
        {
            ValidateFrameOffsetPair(i);

            if (blockColorChangers.Count > i)
            {
                GameObject frameBlock = GetBlockInSlot(frameSlots[i]);
                GameObject offsetBlock = GetBlockInSlot(offsetSlots[i]);

                if (frameBlock != null && offsetBlock != null)
                {
                    BlockType frameBlockType = frameBlock.GetComponent<BlockType>();
                    BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

                    int frameValue = frameBlockType != null ? frameBlockType.addressValue : 0;
                    int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
                    int sum = frameValue + offsetValue;
                    blockColorChangers[i].SetTargetValue(sum, targetValues[i]);
                }
            }
        }
    }

    private void HandleBlockColorChange(BlockColorChanger changer, bool isYellow)
    {
        int index = blockColorChangers.IndexOf(changer);
        if (index == -1) return;

        if (isYellow)
        {
            if (!currentSequence.Contains(index))
            {
                currentSequence.Add(index);
                Debug.Log($"Added index {index} to current sequence. Current sequence: [{string.Join(", ", currentSequence)}]");
            }

            // Only validate once the full sequence is entered
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
                    Debug.Log(" Correct sequence achieved!");
                }
                else
                {
                    Debug.Log(" Incorrect sequence. Resetting.");
                    ResetSequence();
                }
            }
            else
            {
                sequenceCompleted = false;
            }
        }
        else
        {
            // Optional: remove from sequence if a block is turned off
            if (currentSequence.Contains(index))
            {
                Debug.Log($"Block at index {index} turned off. Resetting sequence.");
                ResetSequence();
            }
        }
    }

    private void ResetSequence()
    {
        currentSequence.Clear();
        sequenceCompleted = false;
    }

}

