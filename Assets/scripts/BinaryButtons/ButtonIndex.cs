using UnityEngine;

public class ButtonIndex : MonoBehaviour
{
    public int index;
    public BinaryButtonArray buttonArray;

    public void OnButtonPressed()
    {
        if (buttonArray != null)
        {
            Debug.Log("Button: " + gameObject.name + " toggling index " + index + " on array: " + buttonArray.arrayID);
            buttonArray.ToggleBinaryValue(index, buttonArray.arrayID);  // Pass arrayID to ensure correct instance
        }
        else
        {
            Debug.LogWarning("ButtonArray is not assigned for " + gameObject.name);
        }
    }
}
