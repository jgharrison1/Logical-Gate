using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isOpen;

    public Door()
    {
        this.isOpen = false;
        this.check_Door();
    }

    void change_door(bool receiver)
    {
        this.isOpen = receiver;
    }

    bool check_Door()
    {
        return this.isOpen;
    }

}


