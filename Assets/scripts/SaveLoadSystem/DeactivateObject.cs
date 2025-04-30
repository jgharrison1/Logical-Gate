using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour, IDataPersistence
{
    //this script is for 3 objects in the hub level: IntroCutsceneTrigger, MM door, and binary levels door.
    //these objects should be deactivated upon re-entry of the hub in various parts of the game.
    //Intro cutscene should deactivate after first time loading into the hub.
    //Binary door should deactivate upon completion of final logic gate puzzle level.
    //MM door should deactivate upon completeion of final binary puzzle level.
    public bool isIntroTrigger;
    public bool isBPLDoor;
    public bool isMMDoor;   //these bool values will be true for their respective objects.

    public void LoadData(GameData data) 
    {
        if(isIntroTrigger) {
            if(data.levelsCompleted.TryGetValue("Hub", out isIntroTrigger)){
                this.gameObject.SetActive(false);
            }
        }
        else if(isBPLDoor) {
            if(data.levelsCompleted.TryGetValue("LGPL3-K", out isBPLDoor)){
                this.gameObject.SetActive(false);
            }
        }
        else if(isMMDoor) {
            if(data.levelsCompleted.TryGetValue("BPL2", out isMMDoor)){
                this.gameObject.SetActive(false);
            }
        }
    }


    public void SaveData(GameData data)
    {
        return; //nothing to save for this script, but SaveData function is still required.
    }
}
