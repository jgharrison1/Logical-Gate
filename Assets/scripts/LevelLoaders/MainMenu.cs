using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    string sceneName;

    private void Start() 
    {
        if (!DataPersistenceManager.instance.HasGameData()) 
        {
            continueGameButton.interactable = false;
        }
    }

    public void LoadData(GameData data) 
    {
        //load the name of the last scene player visited to use for the continue button function
        sceneName = data.currentScene;
    }
    //No data should be saved from the main menu script but we still need the function call here
    public void SaveData(GameData data) 
    {      
    }

    public void OnNewGameClicked() 
    {
        DisableMenuButtons();
        // create a new game - which will initialize our game data
        DataPersistenceManager.instance.NewGame();
        // load the gameplay scene - which will in turn save the game because of
        // OnSceneUnloaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("Hub");
    }

    public void OnContinueGameClicked() 
    {
        DisableMenuButtons();
        // load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync(sceneName);
        //since continue button is disabled when data==null, we shouldn't have to worry about this function being called without a saved scene name
    }

    private void DisableMenuButtons() 
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
