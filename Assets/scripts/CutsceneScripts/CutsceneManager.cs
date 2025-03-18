using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private Queue<CutsceneEvent> actions;
    private Camera MCamera;
    private GameObject CameraObject;
    private GameObject Player;
    public bool sceneActive = false;
    public bool movingCamera = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private float smoothSpeed = 0.25f; //same as smoothTime
    private float resizeAmount = 0f; //

    // Start is called before the first frame update
    void Start()
    {
        actions = new Queue<CutsceneEvent>();
        MCamera = Camera.main; //this will allow us to take over camera controls for the cutscene
        CameraObject = GameObject.FindWithTag("MainCamera"); //need a reference to the gameobject to deactivate components
        Player = GameObject.FindWithTag("Player"); //this will deactivate character movement and move character according to cutscene
    }

    void Update() 
    {
        if(movingCamera) {
            if(Vector3.Distance(MCamera.transform.position, targetPosition) > 0.2f)
            {
                MCamera.transform.position = Vector3.SmoothDamp(MCamera.transform.position, targetPosition, ref velocity, smoothSpeed);
            }
            if(Mathf.Abs(resizeAmount) > 0.2f ){
                if(resizeAmount > 0f){
                    MCamera.orthographicSize += 0.1f;
                    resizeAmount -= 0.1f;
                }
                else if(resizeAmount < 0f) {
                    MCamera.orthographicSize -= 0.1f;
                    resizeAmount += 0.1f;
                }
            }
            if((Vector3.Distance(MCamera.transform.position, targetPosition) < 0.2f) & (Mathf.Abs(resizeAmount) < 0.2f )) {
                movingCamera = false; // once camera is in range of target and resizeAmount, stop moving
                NextAction();
            }
        }

    }

    public void StartCutscene(List<CutsceneEvent> cutscene)
    {
        sceneActive = true;

        foreach(CutsceneEvent action in cutscene) 
        {
            actions.Enqueue(action);
        }

        CameraObject.GetComponent<cameraFollow>().enabled = false; //camera follow script is disabled and we must now manually move the camera as an event.
        Player.GetComponent<playerMovement>().stopMoving = true; //disable player movement

        NextAction();

        //return;
    }


    public void NextAction() 
    {
        if (actions.Count == 0) {
            Debug.Log("actions.Count is 0");
            EndCutscene();
            return;
        }
        CutsceneEvent action = actions.Dequeue();

        if(action.isMoveEvent) {
            Debug.Log("Move Event");
            foreach(GameObject obj in action.objects) {
                StartCoroutine(moveEvent(obj, action.endPosition, action.smoothTime, action.waitTime));
            }
            //holUp(action.waitTime);
            //having holdup here can mess up and call nextAction because of the coroutine. delete later if needed.
        }
        
        else if(action.isCameraEvent) {
            Debug.Log("Camera Event");
            targetPosition = action.endPosition;
            resizeAmount = action.resize;
            movingCamera = true;
            //holUp(action.waitTime);
        }

        else if(action.isDialogueEvent) {
            Debug.Log("Dialogue Event");
            FindObjectOfType<DialogueManager>().StartDialogue(action.dialogue); //same as dialogue trigger script
            //holUp(action.waitTime);
        }

        else if(action.isSetActiveEvent) {
            Debug.Log("Set Active Event");
            foreach(GameObject obj in action.objects) {
                if(obj.activeSelf) obj.SetActive(false);
                else obj.SetActive(true);
            }
            holUp(action.waitTime);
        }

        else if(action.isTableEvent) {
            Debug.Log("Table Event");
            tableEvent(action);
            //holUp(action.waitTime);
        }

        else if(action.isButtonEvent) {
            Debug.Log("Button Event");
            //buttonEvent(action);
            action.objects[0].GetComponent<ButtonInputController>().changeButton();
            holUp(action.waitTime);
        }

        else if(action.isWaitEvent) {
            Debug.Log("Wait Event");
            holUp(action.waitTime);
        }

        else {
            Debug.Log("No event type selected.");
            NextAction();
        }
    }


    IEnumerator moveEvent(GameObject obj, Vector3 endPosition, float smoothTime, float seconds) {
        //move event is working decently well for smooth time of .2 though it is still quick.
        while(Vector3.Distance(obj.transform.position, endPosition) > 0.02f) {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, endPosition, smoothTime * Time.deltaTime);
            //obj.transform.position = Vector3.SmoothDamp(obj.transform.position, endPosition, ref velocity, smoothTime);
            yield return new WaitForSeconds(0.001f);
        }
        Debug.Log("Done Moving.");
        holUp(seconds);
        yield return null;
    }


    public void dialogueEvent(CutsceneEvent action) {
        
    }


    public void setActiveEvent(CutsceneEvent action) {
        
    }


    public void tableEvent(CutsceneEvent action) {
        //note that for a table event to work, truth table MUST be the first object in the objects list of the event.
        //1=and,2=or,3=xor,4=nand,5=nor,6=xnor,7=not,8=buffer
        /*
        switch(action.tableEventNumber) {
            case 1:
                Debug.Log("table event 1 successful.");
                action.objects[0].GetComponent<TruthTable>().AndGate();
                break;
            case 2:
                action.objects[0].GetComponent<TruthTable>().OrGate();
                break;
            case 3:
                action.objects[0].GetComponent<TruthTable>().XorGate();
                break;
            case 4:
                action.objects[0].GetComponent<TruthTable>().NandGate();
                break;
            case 5:
                action.objects[0].GetComponent<TruthTable>().NorGate();
                break;
            case 6:
                action.objects[0].GetComponent<TruthTable>().XnorGate();
                break;
            case 7:
                action.objects[0].GetComponent<TruthTable>().NotGate();
                break;
            case 8:
                action.objects[0].GetComponent<TruthTable>().BufferGate();
                break;
            default:
                Debug.Log("Invalid input for table event.");
                break;
        }
        */
        action.objects[0].GetComponent<TruthTable>().nextGate();
        holUp(action.waitTime);
    }


    public void buttonEvent(CutsceneEvent action) {
        //for button events, Button must be first object in list. 
        //As a result, button event and table event must be separate events.
        //Only one button can be acted upon in a single event.
        action.objects[0].GetComponent<ButtonInputController>().changeButton();
    }


    public void holUp(float seconds) {
        Debug.Log("Holding for " + seconds + " seconds.");
        Invoke("NextAction", seconds); //Next action will be called after specified number of seconds.
    }


    public void EndCutscene() {
        Debug.Log("End of Cutscene");
        MCamera.orthographicSize = 7; //reset camera size in case it wasn't done in an event.
        CameraObject.GetComponent<cameraFollow>().enabled = true; //return camera to player
        Player.GetComponent<playerMovement>().stopMoving = false; //return control to player
        sceneActive = false; //must be set before calling EndDialogue
        FindObjectOfType<DialogueManager>().EndDialogue();
        return;
    }
}
