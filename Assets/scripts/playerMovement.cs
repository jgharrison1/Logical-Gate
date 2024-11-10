using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float jump;
    private float horizontalInput;
    private Rigidbody2D rb;
    private float move;
    private bool isJumping = false;
    private Vector3 respawnPoint;

    // Reference to BinaryArrayAdder for updating the sum
    public BinaryArrayAdder binaryArrayAdder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
    }

    void Update()
    {
        // Flip left to right
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0)
        {
            gameObject.transform.localScale = new Vector3(2, 2, 1);
        }
        if (horizontalInput < 0)
        {
            gameObject.transform.localScale = new Vector3(-2, 2, 1);
        }

        // Jumping
        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            isJumping = true;
        }
        move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            respawnPoint = transform.position;
        }
        else if (other.CompareTag("Lava"))
        {
            transform.position = respawnPoint;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Button"))
        {
            HandleButtonInteraction(other.collider);
        }
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Button"))
        {
            isJumping = false;
        }

        RepresentationTypeChanger representationChanger = other.gameObject.GetComponent<RepresentationTypeChanger>();
        if (representationChanger != null)
        {
            // Cycle the representation type
            representationChanger.CycleType();

            // Update the sum output in BinaryArrayAdder
            if (binaryArrayAdder != null)
            {
                binaryArrayAdder.UpdateSumOutput();
            }
        }
    }

    private void HandleButtonInteraction(Collider2D buttonCollider)
    {
        ButtonIndex buttonIndex = buttonCollider.GetComponent<ButtonIndex>();

        if (buttonIndex != null && buttonIndex.buttonArray != null)
        {
            int index = buttonIndex.index;
            BinaryButtonArray buttonArrayManager = buttonIndex.buttonArray;

            buttonArrayManager.ToggleBinaryValue(index, buttonArrayManager.arrayID);
        }
    }
}
