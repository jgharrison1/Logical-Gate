using System.Collections;
using UnityEngine;
using TMPro;

public class MMGateDoor : MonoBehaviour
{
    public mainMemory memorySystem; // Assign in the Inspector
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;
    public TMP_Text text;

    private Vector2 targetPosition;
    private bool playerOnPlatform = false;
    private Transform playerTransform;
    private Vector3 previousPosition;
    [SerializeField] private AudioClip doorOpenSFX;
    //private bool hasRun = false;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (memorySystem != null)
        {
            if (memorySystem.sequenceCompleted)
            {
                OpenGate();
            }
            else
            {
                CloseGate();
            }

            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            Vector2 platformMovement = newPosition - (Vector2)transform.position;
            transform.position = newPosition;

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
        openDoorSoundFX();
        //hasRun = true;
    }

    void CloseGate()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(closedPosition) : closedPosition;
        //hasRun = false;
    }

    void openDoorSoundFX()
    {
        //if (!hasRun && doorOpenSFX != null)
            //SoundFXManager.instance.playSoundFXClip(doorOpenSFX, transform, 1f);
    }
}
