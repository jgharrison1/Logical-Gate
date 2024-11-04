// using UnityEngine;

// public class GateController : MonoBehaviour
// {
//     public ParentGate ConnectedGate;
//     public Vector2 openPosition;
//     public Vector2 closedPosition;
//     public float speed = 2f;
//     private Vector2 targetPosition;

//     private bool playerOnPlatform = false;
//     private Transform playerTransform;
//     private Vector3 playerOffset;

//     void Update()
//     {
//         if (ConnectedGate != null)
//         {
//             if (ConnectedGate.output)
//             {
//                 OpenGate();
//             }
//             else
//             {
//                 CloseGate();
//             }
//             Vector2 previousPosition = transform.position;
//             transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

//             if (playerOnPlatform && playerTransform != null)
//             {
//                 // Update player's position relative to the platform's movement
//                 Vector2 platformMovement = (Vector2)transform.position - previousPosition;
//                 playerTransform.position += (Vector3)platformMovement;
//             }
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.transform.position.y > transform.position.y) // Check that player is on top of the platform
//         {
//             playerOnPlatform = true;
//             playerTransform = collision.transform;
//             playerOffset = playerTransform.position - transform.position; // Store initial offset
//         }
//     }

//     private void OnCollisionExit2D(Collision2D collision)
//     {
//         if (collision.transform == playerTransform)
//         {
//             playerOnPlatform = false;
//             playerTransform = null;
//         }
//     }
    
//     void OpenGate()
//     {
//         Vector2 worldOpenPosition = transform.parent.TransformPoint(openPosition);
//         targetPosition = worldOpenPosition;
//     }

//     void CloseGate()
//     {
//         Vector2 worldClosedPosition = transform.parent.TransformPoint(closedPosition);
//         targetPosition = worldClosedPosition;
//     }
// }

using UnityEngine;
using TMPro;

public class GateController : MonoBehaviour
{
    public ParentGate ConnectedGate;
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;
    public TMP_Text text; // Assign this in the inspector to link the TextMeshPro text object

    private Vector2 targetPosition;
    private bool playerOnPlatform = false;
    private Transform playerTransform;
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

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

            // Calculate platform movement
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

    void OpenGate()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(openPosition) : openPosition;
    }

    void CloseGate()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(closedPosition) : closedPosition;
    }
}
