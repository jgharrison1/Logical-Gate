using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubLoaderMM : MonoBehaviour
{
    
    public void PlayGame(){
        SceneManager.LoadSceneAsync("NewOrLoadGame");
    } 
}
