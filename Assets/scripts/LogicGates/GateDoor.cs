using UnityEngine;

public class GateController : MonoBehaviour
{
    public ParentGate ConnectedGate;
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;
    private Vector2 targetPosition;

    private bool playerOnPlatform = false;
    private Transform playerTransform;
    private Vector3 playerOffset;

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
            Vector2 previousPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (playerOnPlatform && playerTransform != null)
            {
                // Update player's position relative to the platform's movement
                Vector2 platformMovement = (Vector2)transform.position - previousPosition;
                playerTransform.position += (Vector3)platformMovement;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y) // Check that player is on top of the platform
        {
            playerOnPlatform = true;
            playerTransform = collision.transform;
            playerOffset = playerTransform.position - transform.position; // Store initial offset
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == playerTransform)
        {
            playerOnPlatform = false;
            playerTransform = null;
        }
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
