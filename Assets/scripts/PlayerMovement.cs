// using System;
// using UnityEditor.Tilemaps;
// using UnityEngine;

// public class playerMovement : MonoBehaviour, IDataPersistence
// {
//     public float speed;
//     public float jump;
//     private Rigidbody2D rb;
//     private Vector3 respawnPoint;
//     private Animator anim;
//     private BoxCollider2D boxCollider;
//     [SerializeField] private LayerMask groundLayer;
//     [SerializeField] private LayerMask wallLayer;
//     private float wallJumpCooldown;
//     private float horizontalInput;
//     private bool isFacingRight = true;
//     private bool isWallSliding;
//     private float wallSlidingSpeed = 1.5f;
//     private bool isWallJumping;
//     private float wallJumpingDirection;
//     private float wallJumpingTime = 0.4f;
//     private float wallJumpingCounter;
//     private float wallJumpingDuration = 0.4f;
//     private Vector2 wallJumpingPower = new Vector2(2.2f, 12f);
//     private BinaryArrayAdder binaryArrayAdder;
//     public float knockbackForce = 5f; // Knockback force for when colliding with the boss
//     [SerializeField] private AudioSource walkSFX;
//     [SerializeField] private GameObject startSpawn;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         respawnPoint = transform.position;
//         anim = GetComponent<Animator>();
//         boxCollider = GetComponent<BoxCollider2D>();
//         walkSFX = GetComponent<AudioSource>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         horizontalInput = Input.GetAxisRaw("Horizontal");

//         // Jumping
//         if(isGrounded() && Input.GetKeyDown(KeyCode.W))
//             Jump();
        
//         wallSlide();
//         wallJump();

//         if(!isWallJumping)
//             Flip();

//         // animations
//         anim.SetBool("run", horizontalInput != 0);
//         anim.SetBool("grounded", isGrounded());
//         //play walk sound if grounded and walking
//         if(rb.velocity.x!=0 && isGrounded())
//         {
//             if(!walkSFX.isPlaying)
//                 walkSFX.Play();
//         }
//         else
//         {
//             walkSFX.Stop();
//         }
//     }

//     private void FixedUpdate()
//     {
//         if(!isWallJumping)
//         {   // while walljumping, player cannot move horizontally until wallJumpDuration becomes 0
//             rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); //move horizontally
//         }
//     }

//     private void Jump()
//     {
//         rb.velocity = new Vector2(rb.velocity.x, jump); // Apply jump velocity
//         anim.SetTrigger("jump");
//     }
//     private void wallJump()
//     {
//         if(isWallSliding)
//         {
//             isWallJumping = false;
//             wallJumpingDirection = -transform.localScale.x;
//             wallJumpingCounter = wallJumpingTime; //

//             CancelInvoke(nameof(StopWallJumping));

//         }
//         else 
//         {
//             wallJumpingCounter -= Time.deltaTime; //this allows us to turn away from the wall and be able to walljump for a brief moment
//         }
//         if(Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f)
//         {
//             isWallJumping = true;
//             rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
//             wallJumpingCounter = 0f;

//             if (transform.localScale.x != wallJumpingDirection)
//             {
//                 isFacingRight = !isFacingRight;
//                 Vector3 localScale = transform.localScale;
//                 localScale.x *= -1f;
//                 transform.localScale = localScale;
//             }
//         }
//         Invoke(nameof(StopWallJumping), wallJumpingDuration);
//     }

//      private void StopWallJumping()
//     {
//         isWallJumping = false;
//     }

//     private bool isGrounded()
//     {
//         RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
//         return raycastHit.collider != null;
//     }
//     private bool onWall()
//     {
//         RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
//         return raycastHit.collider != null;
//     }

//         private void wallSlide() 
//     {
//         //if player is touching wall (isWallSliding = true), not touching ground, and is moving horizontally(pushing into wall)
//         if (onWall() && !isGrounded() && horizontalInput != 0f)
//         {
//             //while wallsliding, "clamp" the rigidbody's y velocity between negative value of wallslidespeed(2 as of now)
//             // and the float.Maxvalue
//             isWallSliding = true;
//             rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
//         }
//         else {
//             isWallSliding = false;
//         }
//     }
//      private void Flip()
//     {
//         if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
//         {
//             Vector3 localScale = transform.localScale;
//             isFacingRight = !isFacingRight;
//             localScale.x *= -1f;
//             transform.localScale = localScale;
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Checkpoint")) {
//             Debug.Log("Checkpoint activated");
//             respawnPoint = transform.position;
//             DataPersistenceManager.instance.SaveGame();
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         if (other.gameObject.CompareTag("Button"))
//         {
//             HandleButtonInteraction(other.collider);
//         }

//         if (other.gameObject.CompareTag("WormBoss"))
//         {
//             HandleBossCollision(other.gameObject);
//         }

//         // Handle representation change if applicable
//         RepresentationTypeChanger representationChanger = other.gameObject.GetComponent<RepresentationTypeChanger>();
//         if (representationChanger != null)
//         {
//             representationChanger.CycleType();

//             if (binaryArrayAdder != null)
//             {
//                 binaryArrayAdder.UpdateSumOutput();
//             }
//         }

//         // Change the segment's binary value when the player hits a segment
//         WormSegment wormSegment = other.gameObject.GetComponent<WormSegment>();
//         if (wormSegment != null)
//         {
//             wormSegment.ToggleBinaryValue();
//         }

//     }

//     private void HandleBossCollision(GameObject boss)
//     {
//         WormBoss wormBoss = boss.GetComponent<WormBoss>();

//         if (wormBoss != null)
//         {

//         }
//     }

//     private void HandleButtonInteraction(Collider2D buttonCollider)
//     {
//         ButtonIndex buttonIndex = buttonCollider.GetComponent<ButtonIndex>();

//         if (buttonIndex != null && buttonIndex.buttonArray != null)
//         {
//             int index = buttonIndex.index;
//             BinaryButtonArray buttonArrayManager = buttonIndex.buttonArray;

//             buttonArrayManager.ToggleBinaryValue(index, buttonArrayManager.arrayID);
//         }
//     }
//     public void Respawn()
//     {
//         transform.position = respawnPoint;

//         Health health = GetComponent<Health>();
//         if (health != null)
//         {
//             health.RestoreHealth();
//         }
//     }


//     public void LoadData(GameData data) 
//     {
//         if(data.scenesVisited.TryGetValue(data.currentScene, out Vector3 prevPosition)) //if scene has been visited, update player position to position saved
//         {
//             this.transform.position = prevPosition;
//             this.respawnPoint = data.respawnPoint;
//         }
//         //if scene has not been visited, keep player's default position
//     }

//     public void SaveData(GameData data) 
//     {
//         data.respawnPoint = this.respawnPoint;
//         if (data.scenesVisited.ContainsKey(data.currentScene)) {
//             data.scenesVisited.Remove(data.currentScene);
//         }
//         data.scenesVisited.Add(data.currentScene, this.transform.position);
//     }
// }


using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class playerMovement : MonoBehaviour, IDataPersistence
{
    public float speed;
    public float jump;
    private Rigidbody2D rb;
    private Vector3 respawnPoint;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCooldown;
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private float wallSlidingSpeed = 1.5f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.4f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(2.2f, 12f);
    private BinaryArrayAdder binaryArrayAdder;
    public float knockbackForce = 5f; // Knockback force for when colliding with the boss
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private GameObject startSpawn;

    public Transform holdPosition; // Position above player's head to hold blocks
    private GameObject heldBlock = null; // The block currently being held
    [SerializeField] private float grabDistance = 1.5f; // Distance to detect blocks
    [SerializeField] private LayerMask blockLayer; // Layer for blocks

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        walkSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jumping
        if(isGrounded() && Input.GetKeyDown(KeyCode.W))
            Jump();
        
        wallSlide();
        wallJump();

        if(!isWallJumping)
            Flip();

        // animations
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        //play walk sound if grounded and walking
        if(rb.velocity.x!=0 && isGrounded())
        {
            if(!walkSFX.isPlaying)
                walkSFX.Play();
        }
        else
        {
            walkSFX.Stop();
        }

        HandleBlockInteraction();
    }

    private void FixedUpdate()
    {
        if(!isWallJumping)
        {   // while walljumping, player cannot move horizontally until wallJumpDuration becomes 0
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); //move horizontally
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump); // Apply jump velocity
        anim.SetTrigger("jump");
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

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

        private void wallSlide() 
    {
        //if player is touching wall (isWallSliding = true), not touching ground, and is moving horizontally(pushing into wall)
        if (onWall() && !isGrounded() && horizontalInput != 0f)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint")) {
            Debug.Log("Checkpoint activated");
            respawnPoint = transform.position;
            DataPersistenceManager.instance.SaveGame();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Button"))
        {
            HandleButtonInteraction(other.collider);
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

    private void HandleBlockInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldBlock == null)
            {
                TryGrabBlock();
            }
            else
            {
                TryPlaceBlock();
            }
        }
    }

    private void TryGrabBlock()
    {
        // Detect blocks in range
        Collider2D blockCollider = Physics2D.OverlapCircle(transform.position, grabDistance, blockLayer);
        if (blockCollider != null)
        {
            GameObject block = blockCollider.gameObject;

            // Pick up the block
            heldBlock = block;
            block.transform.SetParent(holdPosition);
            block.transform.localPosition = Vector3.zero;
            block.GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics for the block
        }
    }

    private void TryPlaceBlock()
    {
        // Detect if the player is near a valid slot
        Collider2D slotCollider = Physics2D.OverlapCircle(transform.position, grabDistance, blockLayer);
        if (slotCollider != null && slotCollider.CompareTag("Slot"))
        {
            GameObject slot = slotCollider.gameObject;

            // Place the block in the slot
            heldBlock.transform.SetParent(null);
            heldBlock.transform.position = slot.transform.position;
            heldBlock.GetComponent<Rigidbody2D>().isKinematic = false; // Re-enable physics
            heldBlock = null; // Release the block
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize grab range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabDistance);
    }

    public void Respawn()
    {
        transform.position = respawnPoint;

        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.RestoreHealth();
        }
    }

    public void LoadData(GameData data) 
    {
        if(data.scenesVisited.TryGetValue(data.currentScene, out Vector3 prevPosition)) //if scene has been visited, update player position to position saved
        {
            this.transform.position = prevPosition;
            this.respawnPoint = data.respawnPoint;
        }
        //if scene has not been visited, keep player's default position
    }

    public void SaveData(GameData data) 
    {
        data.respawnPoint = this.respawnPoint;
        if (data.scenesVisited.ContainsKey(data.currentScene)) {
            data.scenesVisited.Remove(data.currentScene);
        }
        data.scenesVisited.Add(data.currentScene, this.transform.position);
    }
}
