
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
    [SerializeField] private float grabDistance = 2f; // Distance to detect blocks
    [SerializeField] private LayerMask blockLayer; // Layer for blocks
    [SerializeField] private LayerMask slotLayer; // Layer for blocks

    private secondaryMemory secondaryMemoryInstance;

    private GameObject highlightedSlot; // Currently highlighted slot
    private GameObject highlightedBorder; // Border for the highlight
    [SerializeField] private GameObject highlightBorderPrefab; // Prefab for the border


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        walkSFX = GetComponent<AudioSource>();

        secondaryMemoryInstance = FindObjectOfType<secondaryMemory>();

        if (secondaryMemoryInstance == null)
        {
            Debug.LogError("No secondaryMemory instance found in the scene!");
        }
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

        HandleMovement();
        HandleBlockInteraction();
        HighlightSlot();
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


    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
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
        // Detect blocks in range (use the blockLayer to filter)
        Collider2D detectedCollider = Physics2D.OverlapCircle(transform.position, grabDistance, blockLayer);

        if (detectedCollider != null)
        {
            // Get the block object
            GameObject block = detectedCollider.gameObject;

            // Grab the block and place it above the player
            heldBlock = block;
            block.transform.SetParent(transform); // Make the block a child of the player
            block.transform.position = transform.position + Vector3.up; // Place above the player
            block.SetActive(true); // Make sure the block is active
            Debug.Log("Grabbed block: " + block.name);
        }
        else
        {
            Debug.Log("No block detected to grab.");
        }
    }

    private void TryPlaceBlock()
    {
        if (heldBlock == null)
        {
            Debug.LogError("No block is currently being held!");
            return;
        }

        // Detect if the player is near a valid object
        Collider2D detectedCollider = Physics2D.OverlapCircle(transform.position, grabDistance, slotLayer);

        if (detectedCollider == null)
        {
            Debug.Log("No valid object detected.");
            return;
        }

        GameObject detectedObject = detectedCollider.gameObject;

        // Check if the detected object is a valid slot by comparing tag or layer
        if (detectedObject.CompareTag("Slot") || detectedObject.layer == LayerMask.NameToLayer("slotLayer"))
        {
            Debug.Log("Valid slot detected. Attempting to place block...");

            // Check if the slot is already occupied by another block
            GameObject existingBlock = secondaryMemoryInstance.GetBlockInSlot(detectedObject);

            if (existingBlock != null)
            {
                // Remove the existing block from the slot before placing the new block
                secondaryMemoryInstance.RemoveBlockFromSlot(detectedObject);
                Debug.Log("Removed existing block from slot.");
            }

            // Place the new block in the slot
            if (secondaryMemoryInstance.TryAddBlockToSlot(detectedObject, heldBlock))
            {
                Debug.Log("Block successfully placed in the slot.");
                heldBlock = null; // Release the block after placement
            }
            else
            {
                Debug.Log("Slot placement failed.");
            }
        }
        else
        {
            Debug.Log("Detected object is not a valid slot.");
        }
    }

    private void HighlightSlot()
    {
        // Only proceed if the player is holding a block
        if (heldBlock == null)
        {
            RemoveHighlight();
            return;
        }

        // Determine the direction to check for slots
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        // Cast a ray or overlap circle to detect slots in front of the player
        RaycastHit2D detectedSlot = Physics2D.Raycast(transform.position, direction, grabDistance, slotLayer);

        if (detectedSlot.collider != null)
        {
            GameObject detectedSlotObject = detectedSlot.collider.gameObject;

            // Highlight the detected slot if it's not already highlighted
            if (highlightedSlot != detectedSlotObject)
            {
                RemoveHighlight(); // Remove any existing highlight
                highlightedSlot = detectedSlotObject;

                // Instantiate or move the highlight border to the detected slot's position
                highlightedBorder = Instantiate(highlightBorderPrefab, detectedSlotObject.transform.position, Quaternion.identity);
                highlightedBorder.transform.SetParent(detectedSlotObject.transform); // Attach it to the slot

                // Optionally change the color of the highlight when the player can place a block
                if (CanPlaceBlockInSlot(detectedSlotObject))
                {
                    highlightedBorder.GetComponent<SpriteRenderer>().color = Color.green; // Valid slot for placement (green)
                }
                else
                {
                    highlightedBorder.GetComponent<SpriteRenderer>().color = Color.red; // Invalid slot (red)
                }
            }
        }
        else
        {
            // No valid slot detected; remove the highlight
            RemoveHighlight();
        }
    }

    private bool CanPlaceBlockInSlot(GameObject slot)
    {
        // Check if the slot is valid and not already occupied
        GameObject existingBlock = secondaryMemoryInstance.GetBlockInSlot(slot);
        if (existingBlock == null) // No block already in the slot
        {
            return true;
        }

        // Slot is occupied, cannot place the block
        return false;
    }

    private void RemoveHighlight()
    {
        if (highlightedBorder != null)
        {
            Destroy(highlightedBorder); // Destroy the highlight border object
            highlightedBorder = null;
            highlightedSlot = null;
        }
    }

    // For debug visualization of grab distance (in scene view)
    private void OnDrawGizmosSelected()
    {
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
