using UnityEngine;
using TMPro;

public class BinaryDoorController : MonoBehaviour
{
    public BinaryArrayAdder binaryAdder; 
    public Vector2 openPosition; 
    public Vector2 closedPosition; 
    public float speed = 2f; 
    public int targetValue; 
    public TMP_Text text; // Assign this in the inspector to link the TextMeshPro text object

    private Vector2 targetPosition; 
    private bool playerOnPlatform = false;
    private Transform playerTransform;
    private Vector3 previousPosition;

    void Start()
    {
        if (binaryAdder == null)
        {
            Debug.LogWarning("BinaryAdder is not assigned.");
        }

        previousPosition = transform.position;
    }

    void Update()
    {
        if (binaryAdder == null)
        {
            Debug.LogWarning("BinaryAdder is not assigned.");
            return;
        }

        int adderOutput = binaryAdder.GetOutputValue();

        if (adderOutput == targetValue)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        Vector2 platformMovement = newPosition - (Vector2)transform.position;
        transform.position = newPosition;

        // Update TMP_Text position to follow the platform
        if (text != null)
        {
            text.transform.position += (Vector3)platformMovement;
        }

        if (playerOnPlatform && playerTransform != null)
        {
            playerTransform.position += (Vector3)platformMovement;
        }

        previousPosition = transform.position;
    }

    void OpenDoor()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(openPosition) : openPosition;
    }

    void CloseDoor()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(closedPosition) : closedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y) // Check that player is on top of the platform
        {
            playerOnPlatform = true;
            playerTransform = collision.transform;
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
}

