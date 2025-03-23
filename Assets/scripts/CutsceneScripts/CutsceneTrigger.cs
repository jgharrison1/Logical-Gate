using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public List<CutsceneEvent> eventList = new List<CutsceneEvent>();

    public void triggerCutscene() {
        FindObjectOfType<CutsceneManager>().StartCutscene(eventList);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            triggerCutscene();
        }
        else return;
        
    }

}