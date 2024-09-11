using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;

    public void ButtonPress(){
        myText.text = "Welcome";
    }
}
