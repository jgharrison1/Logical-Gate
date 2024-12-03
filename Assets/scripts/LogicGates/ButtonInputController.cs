using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonInputController : MonoBehaviour, IDataPersistence
{
    public ParentGate ConnectedGate;
    public bool buttonInput1;
    public bool buttonInput2;
    public string buttonName1;
    public string buttonName2;
    private string playerTag = "Player";
    private string enemyTag = "Enemy";

    private SpriteRenderer spriteRenderer;
    private bool color;
    public Color activatedColor = Color.green;
    public Color deactivatedColor = Color.red;
    [SerializeField] private AudioClip buttonHitSFX;
    [SerializeField] private string ID;

    [ContextMenu("Generate guid for ID")]
    private void generateGuid() 
    {
        ID = System.Guid.NewGuid().ToString();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {    
        if (other.CompareTag(playerTag) && gameObject.name == buttonName1)
        {
            buttonInput1 = !buttonInput1;
            ConnectedGate.input1 = buttonInput1;
            color = buttonInput1;
            UpdateButtonColor();
        }
        else if (other.CompareTag(playerTag) && gameObject.name == buttonName2)
        {
            buttonInput2 = !buttonInput2;
            ConnectedGate.input2 = buttonInput2;
            color = buttonInput2;
            UpdateButtonColor();
        }
        if (other.CompareTag(enemyTag) && gameObject.name == buttonName1)
        {
            buttonInput1 = !buttonInput1;
            ConnectedGate.input1 = buttonInput1;
            color = buttonInput1;
            UpdateButtonColor();
        }
        else if (other.CompareTag(enemyTag) && gameObject.name == buttonName2)
        {
            buttonInput2 = !buttonInput2;
            ConnectedGate.input2 = buttonInput2;
            color = buttonInput2;
            UpdateButtonColor();
        }
        if(buttonHitSFX!=null)
            SoundFXManager.instance.playSoundFXClip(buttonHitSFX, transform, 1f);
    }

    private void UpdateButtonColor()
    {
        if (spriteRenderer != null)
        {
            if (color)
            {
                spriteRenderer.color = activatedColor;
            }
            else
            {
                spriteRenderer.color = deactivatedColor;
            }
        }
    }

    public void LoadData(GameData data) 
    {
        if(!String.IsNullOrEmpty(buttonName1)){
            data.buttonStatus.TryGetValue(ID, out buttonInput1);  
            if(buttonInput1) {
                ConnectedGate.input1 = buttonInput1;
                color = buttonInput1;
                UpdateButtonColor();
            } 
        }

        else if(!String.IsNullOrEmpty(buttonName2)){
            data.buttonStatus.TryGetValue(ID, out buttonInput2);
            if(buttonInput2) {
                ConnectedGate.input2 = buttonInput2;
                color = buttonInput2;
                UpdateButtonColor();
            } 
        }
    }

    public void SaveData(GameData data) 
    {
        if (data.buttonStatus.ContainsKey(ID)) {
            data.buttonStatus.Remove(ID);
        }
        if(!String.IsNullOrEmpty(buttonName1))
            data.buttonStatus.Add(ID, buttonInput1);
        else if(!String.IsNullOrEmpty(buttonName2))
            data.buttonStatus.Add(ID, buttonInput2);
    }
}
