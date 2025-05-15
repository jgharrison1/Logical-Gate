using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MMBATeleport : MonoBehaviour, IDataPersistence
{
    public bool BA;
    private bool BATutorialVisited = false;
    private bool MMTutorialVisited = false;
    private Vector3 tmp1;
    private Vector3 tmp2;

    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(BA){
                if(BATutorialVisited) SceneManager.LoadSceneAsync("BPL1");
                else SceneManager.LoadSceneAsync("BTL-J");
            }
            else {
                if(MMTutorialVisited) SceneManager.LoadSceneAsync("MML");
                else SceneManager.LoadSceneAsync("MMTL");
            }
        }
    }

    public void SaveData(GameData data) 
    {
        //nothing to save
        return;
    }

    public void LoadData(GameData data) 
    {
        if(data.scenesVisited.TryGetValue("BTL-J", out Vector3 tmp1)){
            BATutorialVisited = true;
        }

        if(data.scenesVisited.TryGetValue("MMTL", out Vector3 tmp2)){
            MMTutorialVisited = true;
        }
    }
}
