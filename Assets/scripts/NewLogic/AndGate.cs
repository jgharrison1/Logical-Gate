using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndGate : Gate
{
    public Gate ConnectionA;
    public Gate ConnectionB;

    public AndGate()
    {
        // this.connection = connection;
    }

    void update()
    {
        if(ConnectionA && ConnectionB){
            
        }

        if(ConnectionOut){
            
        }
    }

}
