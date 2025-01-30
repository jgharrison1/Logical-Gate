using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class CutsceneEvent
{
    public GameObject obj;
    public bool isMoveEvent;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public bool isSetActiveEvent;
    public bool isDialogueEvent;
    public Dialogue dialogue;
}
