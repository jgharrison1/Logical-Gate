using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndDoor : MonoBehaviour
{
    public bool button1 = false;
    public bool button2 = false;
    private Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (button1 && button2)
        {
            doorAnimator.SetBool("isOpen", true);
        }
        else
        {
            doorAnimator.SetBool("isOpen", false);
        }
    }
}
