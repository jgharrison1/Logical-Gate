using System;
using System.Collections.Generic;
using UnityEngine;

public class XnorLogicGate : ParentGate
{
    public XnorLogicGate(bool input1, bool input2) : base(input1, input2)
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
        
        ProcessXnorGate();
    }

    void ProcessXnorGate()
    {
        output = !(input1 || input2);
        if(input1 && input2)
            output = true;
    }

}
