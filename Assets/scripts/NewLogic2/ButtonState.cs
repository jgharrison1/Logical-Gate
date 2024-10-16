using UnityEngine;

public class ButtonState : MonoBehaviour {
    private bool isOn;
    [SerializeField]
    public int buttonInput1; 

    private void Start() 
    {
        isOn = LoadButtonState(gameObject.name); 
        buttonInput1 = LoadButtonInput(gameObject.name);
        UpdateButtonVisuals(); 
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player")) {
            isOn = !isOn; 
            SaveButtonState(gameObject.name, isOn); 
            buttonInput1 = isOn ? 1 : 0; 
            SaveButtonInput(gameObject.name, buttonInput1); 
            UpdateButtonVisuals(); 
        }
    }

    private void SaveButtonState(string buttonID, bool isOn) {
        PlayerPrefs.SetInt(buttonID + "_State", isOn ? 1 : 0); 
        PlayerPrefs.Save(); 
    }

    private bool LoadButtonState(string buttonID) {
        return PlayerPrefs.GetInt(buttonID + "_State", 0) == 1;
    }

    private void SaveButtonInput(string buttonID, int input) {
        PlayerPrefs.SetInt(buttonID + "_Input1", input);
        PlayerPrefs.Save();
    }

    private int LoadButtonInput(string buttonID) {
        return PlayerPrefs.GetInt(buttonID + "_Input1", 0); 
    }

    private void UpdateButtonVisuals() {
        if (isOn) {
            GetComponent<SpriteRenderer>().color = Color.green;
        } else {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
