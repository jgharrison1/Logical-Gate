using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBehavior : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
    void Start() //comment for test push
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(door.activeSelf == false){
            if(other.gameObject.CompareTag("Player"))
            door.SetActive(true);
        }
        else if(other.gameObject.CompareTag("Player")){
                door.SetActive(false);
        }

    }

    // private void OnCollisionExit2D(Collision2D other){
    //     if(other.gameObject.CompareTag("Player")){
    //         door.SetActive(true);
    //     }
    // }
}
