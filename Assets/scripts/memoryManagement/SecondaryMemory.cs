using UnityEngine;

public class SecondaryMemory : MonoBehaviour
{
    public Transform[] slots; // Array of slot positions
    private GameObject[] blocks; // Blocks currently in the slots

    private void Start()
    {
        blocks = new GameObject[slots.Length];
    }

    public bool TryPlaceBlock(GameObject block, Transform player)
    {
        // Find the first empty slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (blocks[i] == null)
            {
                // Place the block in the slot
                blocks[i] = block;
                block.transform.position = slots[i].position;
                return true;
            }
        }

        return false; // No empty slot found
    }
}
