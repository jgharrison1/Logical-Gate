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
    public GameObject reviewConcepts1;//1 is first layer RC menu, 2 is second...
    public GameObject reviewConcepts2;
    public GameObject reviewConcepts3;
    private bool activeDialogue = false;
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
                reviewConcepts1.SetActive(false);
                reviewConcepts2.SetActive(false);
                reviewConcepts3.SetActive(false);
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
        if(dialogueBox!=null) {
            activeDialogue = dialogueBox.activeSelf;
            FindObjectOfType<DialogueManager>().continueButton.interactable = false;
            tmpButton = GameObject.Find("ExitDialogueButton").GetComponent<Button>();
            tmpButton.interactable = false;
        }
        if(exitWarning!=null)
        {
            exitWarning.SetActive(false);
        }
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

    public void QuitGame() {
        #if !UNITY_EDITOR
			Application.Quit();
		#endif
		
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
    }
}
