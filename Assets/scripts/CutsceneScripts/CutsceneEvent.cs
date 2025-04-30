using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class CutsceneEvent
{
    /*
    public bool isMoveEvent = false; //For when we want to move an object from one part of screen to another.
    public bool isCameraEvent = false; // Adjusting Camera is best done as it's own event rather than in the move event. Still uses endPosition.
    public bool isSetActiveEvent = false; // For when we want an object to disappear or reappear
    public bool isDialogueEvent = false; // For when we want text to show up.
    public bool isTableEvent = false; // For changing the truth table
    public bool isButtonEvent = false; // Change button values. The button must be the first object in objects list.
    public bool isWaitEvent = false; // To Wait between events
    public float smoothTime = 0f; //Controls speed during move object event. 0f smoothTime will make the move event instantaneous(I think, will confirm later).
    public float resize = 0f; //amount that camera will be resized, which is added to the orthographicSize variable in increments of 0.1f
    //public int tableEventNumber = 0; //1=and,2=or,3=xor,4=nand,5=nor,6=xnor,7=not,8=buffer
    public Vector3 endPosition;
    public List<GameObject> objects;
    public Dialogue dialogue;
    public float waitTime = 0.0f;
    */
    public enum Events
    {
        WaitEvent,
        DialogueEvent,
        CameraEvent,
        MoveEvent,
        SetActive,
        TableEvent,
        ButtonEvent,
        ArrayEvent,
        AdderEvent,
        MMEvent,
        TextEvent
    };
    public Events EventType = new Events();
    public float waitTime = 0.0f; //wait time may be used in more than just the wait event
    public float smoothTime = 0f; //Controls speed during move object event. 0f smoothTime will make the move event instantaneous(I think, will confirm later).
    public float resizeCamera = 0f; //Camera's Orthographic size will be resized to this value in a CameraEvent
    public Vector3 endPosition; //Used in move event and camera event
    public List<GameObject> objects; //list of objects for move, setactive, or button events
    public Dialogue dialogue;
    public bool doOnExit = false; //true for events that need to be done even after premature exit of cutscene, such as opening doors.
}
