using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private Queue<CutsceneEvent> actions;
    private GameObject Camera;
    private GameObject Player;
    public bool sceneActive = false;
    // Start is called before the first frame update
    void Start()
    {
        actions = new Queue<CutsceneEvent>();
        Camera = GameObject.FindWithTag("MainCamera"); //this will allow us to take over camera controls for the cutscene
        Player = GameObject.FindWithTag("Player"); //this will deactivate character movement and move character according to cutscene
    }

    public void StartCutscene(List<CutsceneEvent> cutscene){
        sceneActive = true;

        foreach(CutsceneEvent action in cutscene) 
        {
            actions.Enqueue(action);
        }

        Camera.GetComponent<cameraFollow>().enabled = false; //camera follow script is disabled and we must now manually move the camera as an event.

        Player.GetComponent<playerMovement>().stopMoving = true; //disable player movement

        NextAction();

        //return;
    }

    public void NextAction() {
        if (actions.Count == 0) {
            EndCutscene();
            return;
        }
        CutsceneEvent action = actions.Dequeue();

        //move event is not a smooth movement right now, it will be instant.
        if(action.isMoveEvent) {
            foreach(GameObject obj in action.objects) {
                obj.transform.position = action.endPosition;
            }
            holUp(action.waitTime);
        }

        else if(action.isDialogueEvent) {
            FindObjectOfType<DialogueManager>().StartDialogue(action.dialogue); //same as dialogue trigger script
            //holUp(action.waitTime);
        }

        else if(action.isSetActiveEvent) {
            foreach(GameObject obj in action.objects)
                if(obj.activeSelf){
                    obj.SetActive(false);
                }
                else obj.SetActive(true);
            holUp(action.waitTime);
        }

        else if(action.isWaitEvent) {
            holUp(action.waitTime);
        }

        else {
            Debug.Log("No event type selected.");
            NextAction();
        }
    }

    public void moveEvent(CutsceneEvent action) {

    }

    public void dialogueEvent(CutsceneEvent action) {
        
    }

    public void setActiveEvent(CutsceneEvent action) {
        
    }

    public void holUp(float seconds) {
        Invoke("NextAction", seconds);
    }

    public void EndCutscene() {
        Debug.Log("End of Cutscene");
        Camera.GetComponent<cameraFollow>().enabled = true; //return camera to player
        Player.GetComponent<playerMovement>().stopMoving = false;
        sceneActive = false;
        return;
    }
}
