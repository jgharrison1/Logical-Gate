using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class CutsceneEvent
{
    public List<GameObject> objects;
    public bool isMoveEvent; //F or when we want to move an object from one part of screen to another. 
    public Vector3 startPosition;
    public Vector3 endPosition;
    public bool isSetActiveEvent; // For when we want an object to disappear or reappear
    public bool isDialogueEvent; // For when we want text to show up.
    public Dialogue dialogue;
    public bool isWaitEvent; // To Wait between events
    public float waitTime = 0.0f;

}
