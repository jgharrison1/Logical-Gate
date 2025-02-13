using UnityEngine;

public class mainMemory : MonoBehaviour
{
    public Transform[] slots;
    private GameObject[] blocks;

    private void Start()
    {
        blocks = new GameObject[slots.Length];
    }

    public bool TryPlaceBlock(GameObject block, Transform player)
    {
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
