using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject characterSelectUI;
    private bool activeDialogue;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused) 
            {
                settingsMenuUI.SetActive(false);
                characterSelectUI.SetActive(false);
                Resume(); //pressing P while paused will unpause
            }
            else Pause();
        }
        if(!isPaused) Time.timeScale = 1f;
    }

    void Pause() 
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //freeze gameplay
        isPaused = true;
        activeDialogue = GameObject.Find("DialogueBox").activeSelf;
        if(GameObject.Find("DialogueBox").activeSelf){
            GameObject.Find("DialogueBox").SetActive(false);
        }

        // if(){
        //     GameObject.Find("ExitWarning").SetActive(false);
        // }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //resumes gameplay
        isPaused = false;
        if(GameObject.Find("DialogueBox").activeSelf){
            GameObject.Find("DialogueBox").SetActive(true);
        }
    }
}
