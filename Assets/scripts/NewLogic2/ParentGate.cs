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
        else if (other.CompareTag(playerTag) && other.gameObject == input2ButtonObject)
        {
            input2 = !input2;
        }
    }
}
