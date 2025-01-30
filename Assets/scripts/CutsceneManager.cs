using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private Queue<CutsceneEvent> actions;
    // Start is called before the first frame update
    void Start()
    {
        actions = new Queue<CutsceneEvent>();
    }

    public void StartCutscene(List<CutsceneEvent> cutscene){
        foreach(CutsceneEvent action in cutscene) {
            actions.Enqueue(action);
        }

        NextAction();

        
        return;
    }

    public void NextAction() {
        if (actions.Count == 0) {
            EndCutscene();
            return;
        }
        CutsceneEvent action = actions.Dequeue();

        //move event is not a smooth movement right now, it will be instant.
        if(action.isMoveEvent) {
            action.obj.transform.position = action.endPosition;
        }

        else if(action.isDialogueEvent) {
            FindObjectOfType<DialogueManager>().StartDialogue(action.dialogue); //same as dialogue trigger script
        }

        else if(action.isSetActiveEvent) {
            if(action.obj.activeSelf){
                action.obj.SetActive(false);
            }
            else action.obj.SetActive(true);
        }

        else {
            Debug.Log("No event type selected.");
        }
    }

    public void EndCutscene() {
        Debug.Log("End of Cutscene");
    }
}
