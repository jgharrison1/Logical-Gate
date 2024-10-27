// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public class ParentGate : MonoBehaviour
// {
//     public bool input1;
//     public bool input2;
//     public bool output;
//     public GameObject input1ButtonObject;
//     public GameObject input2ButtonObject;
//     public ParentGate previousGate1;
//     public ParentGate previousGate2;
//     private string playerTag = "Player";


//     public ParentGate(bool input1, bool input2)
//     {
//         this.input1 = input1;
//         this.input2 = input2;
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag(playerTag) && other.gameObject == input1ButtonObject)
//         {
//             input1 = !input1;
//         }
//         if (other.CompareTag(playerTag) && other.gameObject == input2ButtonObject)
//         {
//             input2 = !input2;
//         }
//     }
// }

using System;
using System.Collections.Generic;
using UnityEngine;

public class ParentGate : MonoBehaviour
{
    public bool input1;
    public bool input2;
    public bool output;
    public GameObject input1ButtonObject;
    public GameObject input2ButtonObject;
    public ParentGate previousGate1;
    public ParentGate previousGate2;
    private string playerTag = "Player";

    // Fields for sprite representation
    public SpriteRenderer spriteRenderer;  // Attach the SpriteRenderer component here
    public Sprite spriteTrue;              // Attach the sprite for output = true here
    public Sprite spriteFalse;             // Attach the sprite for output = false here

    public ParentGate(bool input1, bool input2)
    {
        this.input1 = input1;
        this.input2 = input2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && other.gameObject == input1ButtonObject)
        {
            input1 = !input1;
        }
        if (other.CompareTag(playerTag) && other.gameObject == input2ButtonObject)
        {
            input2 = !input2;
        }
    }

    // Method to update the sprite based on output value
    protected void UpdateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = output ? spriteTrue : spriteFalse;
        }
    }
}
