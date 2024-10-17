using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputController : MonoBehaviour
{
    // public ButtonState buttonState;
    public ParentGate ConnectedGate;
    public bool buttonInput1;
    public bool buttonInput2;
    public string buttonName1;
    public string buttonName2;
    private string playerTag = "Player";

    private SpriteRenderer spriteRenderer;
    private bool color;
    public Color activatedColor = Color.green;
    public Color deactivatedColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonColor();
    }

    // void Update()
    // {
    //     if(buttonState.buttonInput1 == 1)
    //     {
    //         buttonInput1 = true;
    //     }
    //     else
    //     {
    //         buttonInput2 = false;
    //     }
    // }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && gameObject.name == buttonName1)
        {
            buttonInput1 = !buttonInput1;
            ConnectedGate.input1 = buttonInput1;
            color = buttonInput1;
            UpdateButtonColor();
        }
        else if (other.CompareTag(playerTag) && gameObject.name == buttonName2)
        {
            buttonInput2 = !buttonInput2;
            ConnectedGate.input2 = buttonInput2;
            color = buttonInput2;
            UpdateButtonColor();
        }
    }

    private void UpdateButtonColor()
    {
        if (spriteRenderer != null)
        {
            if (color)
            {
                spriteRenderer.color = activatedColor;
            }
            else
            {
                spriteRenderer.color = deactivatedColor;
            }
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ButtonInputController : MonoBehaviour
// {
//     public ParentGate ConnectedGate;
//     public bool buttonInput1;
//     public bool buttonInput2;
//     public string buttonName1;
//     public string buttonName2;
//     private string playerTag = "Player";

//     private SpriteRenderer spriteRenderer;
//     public Color activatedColor = Color.green;
//     public Color deactivatedColor = Color.red;

//     void Start()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();

//         LoadButtonStates();
//         UpdateButtonColor();
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag(playerTag))
//         {
//             if (gameObject.name == buttonName1)
//             {
//                 buttonInput1 = !buttonInput1;
//                 SaveButtonState(buttonName1, buttonInput1, true); 
//                 Debug.Log($"buttonInput1 toggled: {buttonInput1} for {buttonName1}");
//             }
//             else if (gameObject.name == buttonName2)
//             {
//                 buttonInput2 = !buttonInput2;
//                 SaveButtonState(buttonName2, buttonInput2, false); 
//                 Debug.Log($"buttonInput toggled: {buttonInput2} for {buttonName2}");
//             }

//             UpdateButtonColor();
//         }
//     }

//     private void UpdateButtonColor()
//     {
//         if (spriteRenderer != null)
//         {
//             if (buttonInput1 || buttonInput2)
//             {
//                 spriteRenderer.color = activatedColor;
//             }
//             else
//             {
//                 spriteRenderer.color = deactivatedColor;
//             }
//         }
//     }

//     private void SaveButtonState(string buttonName, bool state, bool isInput1)
//     {
//         Debug.Log($"Saving state for {buttonName}: {state}");

//         if (isInput1)
//         {
//             ConnectedGate.input1 = state; 
//         }
//         else
//         {
//             ConnectedGate.input2 = state; 
//         }

//         PlayerPrefs.SetInt(buttonName, state ? 1 : 0); 
//         PlayerPrefs.Save(); 
//         Debug.Log($"Saved state for {buttonName}: {PlayerPrefs.GetInt(buttonName, -1)}"); 
//     }

//     private void LoadButtonStates()
//     {
//         Debug.Log($"Loading button states...");

//         if (!string.IsNullOrEmpty(buttonName1) || !string.IsNullOrEmpty(buttonName2))
//         {
//             if (PlayerPrefs.HasKey(buttonName1))
//             {
//                 buttonInput1 = PlayerPrefs.GetInt(buttonName1, 0) == 1;
//                 Debug.Log($"Loaded buttonInput1 for {buttonName1}: {buttonInput1}");
//             }

//             else if (PlayerPrefs.HasKey(buttonName2))
//             {
//                 buttonInput2 = PlayerPrefs.GetInt(buttonName2, 0) == 1;
//                 Debug.Log($"Loaded buttonInput2 for {buttonName2}: {buttonInput2}");
//             }
//             else
//             {
//                 Debug.LogWarning($"neither buttonNames are found in PlayerPrefs!");
//             }
//         }

//         // Make sure to update the connected gate with both inputs
//         ConnectedGate.input1 = buttonInput1;
//         ConnectedGate.input2 = buttonInput2;
//     }
// }
