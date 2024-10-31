using UnityEngine;

public class RepresentationTypeChanger : MonoBehaviour
{
    public BinaryButtonArray targetBinaryArray;

    public void CycleType()
    {
        if (targetBinaryArray != null)
        {
            targetBinaryArray.CycleRepresentationType(); 
        }
    }
}