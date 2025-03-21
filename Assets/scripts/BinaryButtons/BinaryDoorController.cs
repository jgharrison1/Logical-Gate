using UnityEngine;
using TMPro;

public class BinaryDoorController : MonoBehaviour
{
    public enum ComparisonSource
    {
        BinaryAdder,
        BinaryButtonArray
    }

    [Header("Comparison Settings")]
    public ComparisonSource comparisonSource; 
    public BinaryArrayAdder binaryAdder;
    public BinaryButtonArray binaryButtonArray;
    public int targetValue;

    [Header("Door Settings")]
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;
    public TMP_Text text;

    private Vector2 targetPosition;
    private bool playerOnPlatform = false;
    private Transform playerTransform;
    private Vector3 previousPosition;
    [SerializeField] private AudioClip doorOpenSFX;

    void Start()
    {
        if (binaryAdder == null && binaryButtonArray == null)
        {
            Debug.LogWarning("Neither BinaryAdder nor BinaryButtonArray is assigned.");
        }

        previousPosition = transform.position;
    }

    void Update()
    {
        int comparisonValue = GetComparisonValue();

        if (comparisonValue == targetValue)
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

    int GetComparisonValue()
    {
        if (comparisonSource == ComparisonSource.BinaryAdder && binaryAdder != null)
        {
            return binaryAdder.GetOutputValue();
        }
        else if (comparisonSource == ComparisonSource.BinaryButtonArray && binaryButtonArray != null)
        {
            return binaryButtonArray.GetDecimalValue();
        }

        Debug.LogWarning("Invalid comparison source or missing reference.");
        return -1;
    }

    void OpenDoor()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(openPosition) : openPosition;
        openDoorSoundFX();
    }

    void CloseDoor()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(closedPosition) : closedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
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

    void openDoorSoundFX()
    {
        //if(!hasRun && doorOpenSFX!=null)
            //SoundFXManager.instance.playSoundFXClip(doorOpenSFX, transform, 1f);
    }
}
