using System;
using System.Collections.Generic;
using UnityEngine;

public class XorLogicGate : ParentGate
{
    public XorLogicGate(bool input1, bool input2) : base(input1, input2)
    {    
    }

    void Update()
    {
        if(previousGate1 != null)
        {
            input1 = previousGate1.output;
        }

        if(previousGate2 != null)
        {
            input2 = previousGate2.output;
        }
        
        ProcessXorGate();
        UpdateSprite();
    }

    void ProcessXorGate()
    {
        output = input1 || input2;
        if(input1 && input2)
            output = false;
    }

}
