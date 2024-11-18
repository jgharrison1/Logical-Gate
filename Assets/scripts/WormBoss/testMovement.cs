using UnityEngine;

public class testMovement : MonoBehaviour
{
    public float speed;
    public float jump;
    private float horizontalInput;
    private Rigidbody2D rb;
    private float move;
    private bool isJumping = false;
    private Vector3 respawnPoint;
    private BinaryArrayAdder binaryArrayAdder;
    public float knockbackForce = 5f; // Knockback force for when colliding with the boss

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

        if (other.gameObject.CompareTag("WormBoss"))
        {
            HandleBossCollision(other.gameObject);
        }

        // Handle representation change if applicable
        RepresentationTypeChanger representationChanger = other.gameObject.GetComponent<RepresentationTypeChanger>();
        if (representationChanger != null)
        {
            representationChanger.CycleType();

            if (binaryArrayAdder != null)
            {
                binaryArrayAdder.UpdateSumOutput();
            }
        }

        // Change the segment's binary value when the player hits a segment
        WormSegment wormSegment = other.gameObject.GetComponent<WormSegment>();
        if (wormSegment != null)
        {
            wormSegment.ToggleBinaryValue();
        }
    }

    private void HandleBossCollision(GameObject boss)
    {
        WormBoss wormBoss = boss.GetComponent<WormBoss>();

        if (wormBoss != null)
        {

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

