using System;
using System.Collections.Generic;
using UnityEngine;

public class NotLogicGate : ParentGate
{
    public NotLogicGate(bool input1, bool input2) : base(input1, input2)
    {    
    }

    void Update()
    {
        if(previousGate1 != null)
        {
            input1 = previousGate1.output;
        }
        
        ProcessNotGate();
        UpdateSprite();
    }

    void ProcessNotGate()
    {
        output = !input1;
    }

}
