using System;
using System.Collections.Generic;
using UnityEngine;

public class OrLogicGate : ParentGate
{
    public OrLogicGate(bool input1, bool input2) : base(input1, input2)
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

        ProcessOrGate();
    }

    void ProcessOrGate()
    {
        output = input1 || input2;
    }

}
