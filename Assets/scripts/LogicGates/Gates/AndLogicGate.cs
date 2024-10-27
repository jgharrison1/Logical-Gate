// using System;
// using System.Collections.Generic;
// using UnityEngine;

// public class AndLogicGate : ParentGate
// {
//     public AndLogicGate(bool input1, bool input2) : base(input1, input2)
//     {    
//     }

//     void Update()
//     {
//         if(previousGate1 != null)
//         {
//             input1 = previousGate1.output;
//         }

//         if(previousGate2 != null)
//         {
//             input2 = previousGate2.output;
//         }
        
//         ProcessAndGate();
//     }

//     void ProcessAndGate()
//     {
//         output = input1 && input2;
//     }
// }

using System;
using System.Collections.Generic;
using UnityEngine;

public class AndLogicGate : ParentGate
{
    public AndLogicGate(bool input1, bool input2) : base(input1, input2)
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
        
        ProcessAndGate();
        UpdateSprite();
    }

    void ProcessAndGate()
    {
        output = input1 && input2;
    }
}
