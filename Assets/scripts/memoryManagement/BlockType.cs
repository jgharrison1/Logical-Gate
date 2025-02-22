using UnityEngine;

public class BlockType : MonoBehaviour
{
    public enum Type { PageNumber, Offset, FrameNumber }
    public Type blockType;

    public void SetBlockType(Type newType)
    {
        blockType = newType;
    }
}

