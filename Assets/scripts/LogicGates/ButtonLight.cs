using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLight : MonoBehaviour
{
    public ParentGate ConnectedGate;
    private SpriteRenderer spriteRenderer;
    private bool color;
    public Color activatedColor = Color.green;
    public Color deactivatedColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonColor();
    }

    void Update()
    {
        if (ConnectedGate != null){
            // if (ConnectedGate.output)
            color = ConnectedGate.output;
            UpdateButtonColor();
            // else{
            //     UpdateButtonColor();
            // }
        }
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
}
