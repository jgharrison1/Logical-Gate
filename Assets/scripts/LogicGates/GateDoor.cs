using UnityEngine;

public class GateController : MonoBehaviour
{
    public ParentGate ConnectedGate;
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;
    private Vector2 targetPosition;

    void Update()
    {
        if (ConnectedGate != null)
        {
            if (ConnectedGate.output)
            {
                OpenGate();
            }
            else
            {
                CloseGate();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.position.y > transform.position.y) //check that player is on top of the platform, not touching the side or bottom
        { 
            collision.transform.SetParent(transform); //sets platform as the parent object of the object colliding with it, which should be the player
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null); //when player exits platform, they are no longer moving with the platform
    }
    
    void OpenGate()
    {
        Vector2 worldOpenPosition = transform.parent.TransformPoint(openPosition);
        targetPosition = worldOpenPosition;
    }

    void CloseGate()
    {
        Vector2 worldClosedPosition = transform.parent.TransformPoint(closedPosition);
        targetPosition = worldClosedPosition;
    }
}

