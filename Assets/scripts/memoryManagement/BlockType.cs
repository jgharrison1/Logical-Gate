using UnityEngine;

public class BlockType : MonoBehaviour
{
    public enum Type { PageNumber, Offset, FrameNumber }
    public Type blockType;

    public int addressValue;

    public void SetBlockType(Type newType) {blockType = newType;}

    public void SetAddress(int newAddress) {addressValue = newAddress;}

    public int GetAddress() {return addressValue;}
}

