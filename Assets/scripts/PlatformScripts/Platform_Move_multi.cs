using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Platform_Move_multi : MonoBehaviour
{
    public int startPosition;
    public float speed = 2f;
    public Transform[] points;
    private int i = 0;

/*
This script is unfinished but will move between multiple points
*/
    void Start()
    {
        transform.position = points[startPosition].position;
    }
    void Update()
    {
        //check distance of platform and point
        if(Vector2.Distance(transform.position, points[i].position) < 0.02f) //if platform is very close to it's target position, move to next
        {
            i++;
            if(i == points.Length) //check if the platform was on the last point after the index increase
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.transform.position.y > transform.position.y) //check that player is on top of the platform, not touching the side or bottom
        { 
            collision.transform.SetParent(transform); //sets platform as the parent object of the object colliding with it, which should be the player
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        collision.transform.SetParent(null); //when player exits platform, they are no longer moving with the platform
    }

}
