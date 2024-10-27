using UnityEngine;

public class BinaryDoorController : MonoBehaviour
{
    public BinaryArrayAdder binaryAdder; 
    public Vector2 openPosition; 
    public Vector2 closedPosition; 
    public float speed = 2f; 
    public int targetValue; 

    private Vector2 targetPosition; 
    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the GameObject.");
        }

        if (binaryAdder != null)
        {
            Debug.Log("BinaryAdder assigned: " + binaryAdder.name);
        }
        else
        {
            Debug.LogWarning("BinaryAdder is not assigned.");
        }
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

        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));
    }

    void OpenDoor()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(openPosition) : openPosition;
        Debug.Log("Opening Door"); // Log when the door opens
    }

    void CloseDoor()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(closedPosition) : closedPosition;
        Debug.Log("Closing Door"); // Log when the door closes
    }
}
