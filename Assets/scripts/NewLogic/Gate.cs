using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private bool x;
    private bool y;
    private bool output;
    public Gate ConnectionOut;
    public Door Door;

    public Gate(){
        this.x = false;
        this.y = false;
        this.output = false;
    }

    void change_x(bool power){
        this.x = power;
        this.check();
    }

    void change_y(bool power){
        this.y = power;
        this.check();
    }

    void change_output(){
        this.output = !this.output;
    }

    void check(){

    }

}