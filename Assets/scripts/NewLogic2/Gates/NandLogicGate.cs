using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NandLogicGate : ParentGate
{
    public NandLogicGate(bool input1, bool input2) : base(input1, input2)
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
        
        ProcessNandGate();
    }

    void ProcessNandGate()
    {
        output = !(input1 && input2);
    }
}
