using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject popupText; 
    private bool isPopupActive = false; 

    void Start()
    {
        if (popupText != null)
        {
            popupText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPopupActive = !isPopupActive;
            if (popupText != null)
            {
                popupText.SetActive(isPopupActive);
            }
        }
    }
}
