using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpLines : MonoBehaviour
{
    public bool input;
    public Sprite on;
    public Sprite off;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(input)
        {
            spriteRenderer.sprite = on;
        }
        else
        {
            spriteRenderer.sprite = off;
        }
    }
}
