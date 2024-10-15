using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour{
    public float speed;
    public float jump;
    float horizontalInput;


    private Rigidbody2D rb;
    private float move;
    private bool isJumping = false;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    //update is called once per frame
    void Update(){
        //flip left to right -> not done
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if(horizontalInput > 0){
            gameObject.transform.localScale = new Vector3(2, 2, 1);
        }
        if(horizontalInput < 0){
            gameObject.transform.localScale = new Vector3(-2, 2, 1);
        }

        //jumping
        if(Input.GetKey(KeyCode.W) && !isJumping){
            rb.velocity = new Vector2(rb.velocity.x, jump);
            isJumping = true;
        }    
        move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform")){
            isJumping = false;
        }
    }

}




