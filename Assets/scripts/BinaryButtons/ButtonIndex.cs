using UnityEngine;

public class ButtonIndex : MonoBehaviour
{
    public int index;
    public BinaryButtonArray buttonArray;

    public void OnButtonPressed()
    {
        if (buttonArray != null)
        {
            buttonArray.ToggleBinaryValue(index, buttonArray.arrayID); 
        }
    }
}
