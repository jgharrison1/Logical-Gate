using System;
using System.Collections.Generic;
using UnityEngine;

public class BufferLogicGate : ParentGate
{
    public BufferLogicGate(bool input1, bool input2) : base(input1, input2)
    {    
    }

    void Update()
    {
        if(previousGate1 != null)
        {
            input1 = previousGate1.output;
        }
        
        ProcessBufferGate();
    }

    void ProcessBufferGate()
    {
        output = input1;
    }

}
