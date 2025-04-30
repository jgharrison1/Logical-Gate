using UnityEngine;
using TMPro;

public class MMGateDoor : MonoBehaviour
{
    public mainMemory memoryManager; // Reference to the mainMemory script
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
        targetPosition = transform.position;

        if (memoryManager == null)
        {
            Debug.LogWarning("MemoryManager reference is not set on MMGateDoor.");
        }
    }

    void Update()
    {
        if (memoryManager != null && memoryManager.sequenceCompleted)
        {
            OpenGate();
        }
        else
        {
            CloseGate();
        }

        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        Vector2 movementDelta = newPosition - (Vector2)transform.position;
        transform.position = newPosition;

        if (text != null)
        {
            text.transform.position += (Vector3)movementDelta;
        }

        if (playerOnPlatform && playerTransform != null)
        {
            playerTransform.position += (Vector3)movementDelta;
        }

        previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
        {
            playerOnPlatform = true;
            playerTransform = collision.transform;
            collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == playerTransform)
        {
            playerOnPlatform = false;
            playerTransform = null;
            collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    void OpenGate()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(openPosition) : openPosition;
        PlayDoorOpenSound();
        //hasRun = true;
    }

    void CloseGate()
    {
        targetPosition = transform.parent != null ? transform.parent.TransformPoint(closedPosition) : closedPosition;
        //hasRun = false;
    }

    void PlayDoorOpenSound()
    {
        //if (!hasRun && doorOpenSFX != null)
            //SoundFXManager.instance.playSoundFXClip(doorOpenSFX, transform, 1f);
    }
}
