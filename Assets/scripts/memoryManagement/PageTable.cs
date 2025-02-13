using UnityEngine;

public class pageTable : MonoBehaviour
{
    public Transform[] slots; // Array of slot positions
    private GameObject[] blocks;

    private void Start()
    {
        blocks = new GameObject[slots.Length];
    }

    public bool TryPlaceBlock(GameObject block, Transform player)
    {
        // Similar to SecondaryMemory, manage placements
        for (int i = 0; i < slots.Length; i++)
        {
            if (blocks[i] == null)
            {
                blocks[i] = block;
                block.transform.position = slots[i].position;
                return true;
            }
        }

        return false;
    }
}
