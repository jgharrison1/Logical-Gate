using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //public int deathCount; // temporary - using for development purposes
    public float playerHealth; // initialize in playermovement script
    public Vector3 playerPosition; // initialize in player movement script
    public Vector3 respawnPoint;
    public bool firstLoad;
    //public SerializableDictionary<string, bool> enemiesDefeated; // initialize in player movement script
    //public SerializableDictionary<string, bool> buttonStatus; //initialize in  button script

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        //this.deathCount = 0;
        firstLoad = true;
        this.playerHealth = 3.0f;
        //enemiesDefeated = new SerializableDictionary<string, bool>();
        //buttonStatus = new SerializableDictionary<string, bool>();
    }
}
