using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject characterSelectUI;
    private bool activeDialogue;
    public GameObject dialogueBox;
    public GameObject exitWarning;
    private Button tmpButton;

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

    public void Pause() 
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //freeze gameplay
        isPaused = true;
        activeDialogue = GameObject.Find("DialogueBox").activeSelf;
        if(GameObject.Find("ExitWarning")!=null)
        {
            GameObject.Find("ExitWarning").SetActive(false);
        }
        if(activeDialogue){
            FindObjectOfType<DialogueManager>().continueButton.interactable = false;
            tmpButton = GameObject.Find("ExitDialogueButton").GetComponent<Button>();
            tmpButton.interactable = false;

        }
/*
        if(GameObject.Find("ExitWarning")!=null)
        {
            GameObject.Find("ExitWarning").SetActive(false);
        }
        */
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //resumes gameplay
        isPaused = false;
        
        if(activeDialogue){
            FindObjectOfType<DialogueManager>().continueButton.interactable = true;
            tmpButton.interactable = true;
            activeDialogue = false;
        }
    }
}
