using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubLoaderMM : MonoBehaviour
{

    
    public void PlayGame(){
        SceneManager.LoadSceneAsync("Hub");
    } 

    public void ContinueGame() {
        SceneManager.LoadSceneAsync(FindObjectOfType<DataPersistenceManager>().getSceneName());
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        //script can be attached to whichever teleports need to teleport back to hub. note isTrigger must be unselected.
        if(other.gameObject.CompareTag("Player")){
            SceneManager.LoadSceneAsync("Hub");
        }
    }
}
