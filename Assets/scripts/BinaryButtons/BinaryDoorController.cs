using UnityEngine;

public class BinaryDoorController : MonoBehaviour
{
    public BinaryArrayAdder binaryAdder; // Reference to the binary adder
    public Vector2 openPosition; // Position when the door is open
    public Vector2 closedPosition; // Position when the door is closed
    public float speed = 2f; // Speed of the door movement
    public int targetValue; // The value to compare with the binary output

    private Vector2 targetPosition; // Target position for the door
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the GameObject.");
        }
    }

    void Update()
    {
        // Ensure binaryAdder is assigned
        if (binaryAdder == null)
        {
            Debug.LogWarning("BinaryAdder is not assigned.");
            return;
        }

        // Get the output from the binary adder
        int adderOutput = binaryAdder.GetOutputValue();

        // Open the door if the adder output matches the target value
        if (adderOutput == targetValue)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

        // Move the door towards the target position
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
    }

    void OpenDoor()
    {
        // If there's a parent transform, use it to calculate the world position; otherwise, use openPosition directly
        targetPosition = transform.parent != null ? 
                         transform.parent.TransformPoint(openPosition) : 
                         openPosition;
        Debug.Log("Opening Door"); // Log when the door opens
    }

    void CloseDoor()
    {
        // If there's a parent transform, use it to calculate the world position; otherwise, use closedPosition directly
        targetPosition = transform.parent != null ? 
                         transform.parent.TransformPoint(closedPosition) : 
                         closedPosition;
        Debug.Log("Closing Door"); // Log when the door closes
    }
}
