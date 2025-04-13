// using System.Collections.Generic;
// using UnityEngine;

// public class secondaryMemory : MonoBehaviour
// {
//     [Header("Page Number Slots")]
//     public List<GameObject> pageSlots = new List<GameObject>();

//     [Header("Offset Slots")]
//     public List<GameObject> offsetSlots = new List<GameObject>(); 

//     [Header("Target Values")]
//     public List<int> targetValues = new List<int>(); 
    
//     [Header("Block Color Changers")]
//     public List<BlockColorChanger> blockColorChangers = new List<BlockColorChanger>();

//     [Header("Runtime Assignment")]
//     public List<GameObject> blocksToAssign = new List<GameObject>();
//     public List<int> slotIndicesToAssign = new List<int>(); 
//     public List<BlockType.Type> slotTypesToAssign = new List<BlockType.Type>(); 

//     private void Start()
//     {
//         if (blocksToAssign.Count > 0)
//         {
//             for (int i = 0; i < blocksToAssign.Count; i++)
//             {
//                 if (blocksToAssign[i] != null) {AssignBlockToSlot(i);}
//             }
//             ValidatePageOffsetPairs();
//         }
//         UpdateBlockColorsOnStart();
//     }

//     private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
//     private playerMovement playerMovementScript;

//     public void RegisterSlot(GameObject slot, BlockType.Type slotType)
//     {
//         if (slot == null) return;

//         if (slotType == BlockType.Type.PageNumber && !pageSlots.Contains(slot))
//         {
//             pageSlots.Add(slot);
//         }
//         else if (slotType == BlockType.Type.Offset && !offsetSlots.Contains(slot))
//         {
//             offsetSlots.Add(slot);
//         }
//     }

//     public void UnregisterSlot(GameObject slot)
//     {
//         if (slot == null) return;
//     }

//     public GameObject GetBlockInSlot(GameObject slot)
//     {
//         return slotToBlockMap.ContainsKey(slot) ? slotToBlockMap[slot] : null;
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

//         if (pageSlots[0] == slot || offsetSlots[0] == slot)
//         {
//             if (blockColorChangers.Count > 0)
//             {
//                 blockColorChangers[0].SetTargetValue(0, targetValues[0]);
//             }
//         }
//         return block;
//     }

//     public bool TryAddBlockToSlot(GameObject slot, GameObject block)
//     {
//         if (slot == null || block == null || !IsValidSlot(slot))
//         {
//             return false;
//         }

//         BlockType blockType = block.GetComponent<BlockType>();
//         if (blockType == null) {return false;}

//         if ((pageSlots.Contains(slot) && blockType.blockType != BlockType.Type.PageNumber) ||
//             (offsetSlots.Contains(slot) && blockType.blockType != BlockType.Type.Offset))
//         {
//             block.transform.SetParent(playerMovementScript.transform); 
//             block.transform.position = playerMovementScript.holdPosition.position; 
//             block.SetActive(true);
//             return false;
//         }
//         slotToBlockMap[slot] = block;
//         block.transform.SetParent(slot.transform);
//         block.transform.localPosition = Vector3.zero;
//         ValidatePageOffsetPairs(); 
//         return true;
//     }

//     public bool IsSlotOccupied(GameObject slot, GameObject block)
//     {
//         return slotToBlockMap.ContainsKey(slot) && slotToBlockMap[slot] != block;
//     }

//     private bool IsValidSlot(GameObject slot)
//     {
//         return slot != null && (pageSlots.Contains(slot) || offsetSlots.Contains(slot));
//     }

//     public void ValidatePageOffsetPairs()
//     {
//         for (int i = 0; i < pageSlots.Count && i < offsetSlots.Count; i++)
//         {
//             ValidatePageOffsetPair(i);
//         }
//     }

//     public void ValidatePageOffsetPair(int index)
//     {
//         if (index < 0 || index >= targetValues.Count) return;

//         GameObject pageBlock = GetBlockInSlot(pageSlots[index]);
//         GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

//         if (pageBlock != null && offsetBlock != null)
//         {
//             BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
//             BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();
//             int pageValue = pageBlockType != null ? pageBlockType.addressValue : 0;
//             int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
//             int sum = pageValue + offsetValue;

//             if (blockColorChangers.Count > index)
//             {
//                 blockColorChangers[index].SetTargetValue(sum, targetValues[index]); 
//             }
//         }
//         else
//         {
//             if (blockColorChangers.Count > index)
//             {
//                 blockColorChangers[index].SetTargetValue(0, targetValues[index]);
//             }
//         }
//     }

//     public void AssignAllBlocksFromInspector()
//     {
//         for (int i = 0; i < blocksToAssign.Count; i++)
//         {
//             if (blocksToAssign[i] != null) {AssignBlockToSlot(i);}
//         }
//         ValidatePageOffsetPairs();
//     }

//     private void AssignBlockToSlot(int index)
//     {
//         if (index < 0 || index >= blocksToAssign.Count) {return;}

//         GameObject block = blocksToAssign[index];
//         int slotIndex = slotIndicesToAssign[index];
//         BlockType.Type slotType = slotTypesToAssign[index];

//         if (block == null) {return;}

//         List<GameObject> slotList = slotType == BlockType.Type.PageNumber ? pageSlots : offsetSlots;

//         if (slotIndex >= slotList.Count) {return;}

//         GameObject slot = slotList[slotIndex];
//         bool success = TryAddBlockToSlot(slot, block);

//         if (success)
//         {
//             block.transform.SetParent(slot.transform);
//             block.transform.position = slot.transform.position;
//             block.transform.localRotation = Quaternion.identity;
//             ValidatePageOffsetPairs();
//         }
//     }

//     private void UpdateBlockColorsOnStart()
//     {
//         for (int i = 0; i < pageSlots.Count && i < offsetSlots.Count; i++)
//         {
//             ValidatePageOffsetPair(i); 

//             if (blockColorChangers.Count > i)
//             {
//                 GameObject pageBlock = GetBlockInSlot(pageSlots[i]);
//                 GameObject offsetBlock = GetBlockInSlot(offsetSlots[i]);

//                 if (pageBlock != null && offsetBlock != null)
//                 {
//                     BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
//                     BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

//                     int pageValue = pageBlockType != null ? pageBlockType.addressValue : 0;
//                     int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
//                     int sum = pageValue + offsetValue;
//                     blockColorChangers[i].SetTargetValue(sum, targetValues[i]);
//                 }
//             }
//         }
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public class secondaryMemory : MonoBehaviour
{
    [Header("Page Number Slots")]
    public List<GameObject> pageSlots = new List<GameObject>();

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

    private Dictionary<GameObject, GameObject> slotToBlockMap = new Dictionary<GameObject, GameObject>();
    private playerMovement playerMovementScript;

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
        }

        UpdateBlockColorsOnStart();
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
                blockColorChangers[0].SetTargetValue(0, targetValues[0]);
            }
        }

        return block;
    }

    public bool TryAddBlockToSlot(GameObject slot, GameObject block)
    {
        if (slot == null || block == null || !IsValidSlot(slot)) return false;

        BlockType blockType = block.GetComponent<BlockType>();
        if (blockType == null) return false;

        if ((pageSlots.Contains(slot) && blockType.blockType != BlockType.Type.PageNumber) ||
            (offsetSlots.Contains(slot) && blockType.blockType != BlockType.Type.Offset))
        {
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
        if (index < 0 || index >= targetValues.Count) return;

        GameObject pageBlock = GetBlockInSlot(pageSlots[index]);
        GameObject offsetBlock = GetBlockInSlot(offsetSlots[index]);

        if (pageBlock != null && offsetBlock != null)
        {
            BlockType pageBlockType = pageBlock.GetComponent<BlockType>();
            BlockType offsetBlockType = offsetBlock.GetComponent<BlockType>();

            int pageValue = pageBlockType != null ? pageBlockType.addressValue : 0;
            int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
            int sum = pageValue + offsetValue;

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

                    int pageValue = pageBlockType != null ? pageBlockType.addressValue : 0;
                    int offsetValue = offsetBlockType != null ? offsetBlockType.addressValue : 0;
                    int sum = pageValue + offsetValue;

                    blockColorChangers[i].SetTargetValue(sum, targetValues[i]);
                }
            }
        }
    }
}
