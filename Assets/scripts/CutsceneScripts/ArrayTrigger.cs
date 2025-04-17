using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTrigger : MonoBehaviour
{
    public List<CutsceneEvent> eventList = new List<CutsceneEvent>();
    public BinaryButtonArray array;
    public int targetValue;

    public void triggerCutscene() {
        FindObjectOfType<CutsceneManager>().StartCutscene(eventList);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(array.GetDecimalValue() == targetValue) triggerCutscene();
    }
}
