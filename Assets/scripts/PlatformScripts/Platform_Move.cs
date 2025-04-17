using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Platform_Move : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float speed = 2f;
    private bool direction = true; //when false, move back to start
/*
This script requires an endpoint and a startpoint and will move continuously between them
if more points are required, I will make another script with that functionality
*/

    void Update()
    {
        //check distance of platform and point
        if(direction) {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        }
        else{
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, endPosition) < 0.02f) direction = false;
        if(Vector2.Distance(transform.position, startPosition) < 0.02f) direction = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.position.y > transform.position.y & collision.gameObject.CompareTag("Player")) //check that player is on top of the platform, not touching the side or bottom
        { 
            collision.transform.SetParent(transform); //sets platform as the parent object of the object colliding with it, which should be the player
            collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None; //turn off interpolation while on platform, otherwise velocity gets messed up
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null); //when player exits platform, they are no longer moving with the platform
        collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate; //turn rigidbody interpolation back on.
    }

}
