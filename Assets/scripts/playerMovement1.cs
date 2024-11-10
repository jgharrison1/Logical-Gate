using UnityEngine;

public class playerMovement1 : MonoBehaviour
{
    public float speed;
    public float jump;
    float horizontalInput;
    //private Rigidbody2D rb;
    private float move;
    private bool isJumping = false;
    private Vector3 respawnPoint;
    private bool isFacingRight = true;

    //variables for walljump/wallslide
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 12f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    //[SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Flip left to right, now handled by Flip() function
        horizontalInput = Input.GetAxisRaw("Horizontal");
        /*
        if (horizontalInput > 0)
        {
            gameObject.transform.localScale = new Vector3(2, 2, 1);
        }
        if (horizontalInput < 0)
        {
            gameObject.transform.localScale = new Vector3(-2, 2, 1);
        }
        */

        // Jumping
        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            isJumping = true;
        }
        move = Input.GetAxis("Horizontal");

        wallSlide();
        wallJump();

        if (!isWallJumping)
        { //cannot flip while walljumping
            Flip();
        }
    }
    //fixed update is not called per frame, it should be even more responsive
    private void FixedUpdate()
    {
        if(!isWallJumping)
        {   // while walljumping, player cannot move horizontally until wallJumpDuration becomes 0
            rb.velocity = new Vector2(move * speed, rb.velocity.y); //move horizontally
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if player crosses checkpoint, respawnPoint gets updated
        if (other.CompareTag("Checkpoint"))
        {
            respawnPoint = transform.position;
        }
        //if player enters lava, they die and go to respawn point
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
            representationChanger.CycleType();
        }

        /*check if player is touching wall
        if (other.gameObject.CompareTag("Wall"))
        {
            isWallSliding = true;
        }
        */
    }
/*
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            isWallSliding = false;
        }
    }
*/

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
    
    private bool isGrounded() 
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void wallSlide() 
    {
        //if player is touching wall (isWallSliding = true), not touching ground, and is moving horizontally(pushing into wall)
        if (isWalled() && !isGrounded() && horizontalInput != 0f)
        {
            //while wallsliding, "clamp" the rigidbody's y velocity between negative value of wallslidespeed(2 as of now)
            // and the float.Maxvalue
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else {
            isWallSliding = false;
        }
    }

    private void wallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime; //

            CancelInvoke(nameof(StopWallJumping));

        }
        else 
        {
            wallJumpingCounter -= Time.deltaTime; //this allows us to turn away from the wall and be able to walljump for a brief moment
        }
        if(Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
