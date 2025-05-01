using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialTeleport : MonoBehaviour, IDataPersistence
{
    public bool t1, t2, t3;
    public Button b1,b2,b3;
    private Vector3 tmp1, tmp2, tmp3;
    public GameObject SelectionMenu;
    // Start is called before the first frame update
    public void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            SelectionMenu.SetActive(true);
            if(!t1){
                b1.interactable = false;
            }
            if(!t2){
                b2.interactable = false;
            }
            if(!t3){
                b3.interactable = false;
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
        if(data.scenesVisited.TryGetValue("LGTL-J", out Vector3 tmp1)){
            t1 = true;
        }
        else if(data.scenesVisited.TryGetValue("BTL-J", out Vector3 tmp2)){
            t2 = true;
        }
        else if(data.scenesVisited.TryGetValue("MMTL", out Vector3 tmp3)){
            t3 = true;
        }
    }

    public void t1Selected(){
        SceneManager.LoadSceneAsync("LGTL-J");
    }

    public void t2Selected(){
        SceneManager.LoadSceneAsync("BTL-J");
    }

    public void t3Selected(){
        SceneManager.LoadSceneAsync("MMTL");
    }
}
