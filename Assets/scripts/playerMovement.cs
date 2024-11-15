
















































































































































































































































// using System;
// using UnityEngine;

// public class playerMovement : MonoBehaviour
// {
//     public float speed;
//     public float jump;
//     private float horizontalInput;
//     private Rigidbody2D rb;
//     private float move;
//     private bool isJumping = false;
//     private Vector3 respawnPoint;
//     private Animator anim;
//     private BoxCollider2D boxCollider;
//     [SerializeField] private LayerMask groundLayer;
//     [SerializeField] private LayerMask wallLayer;
//     private float wallJumpCooldown;
//     private float horizontalInput;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         respawnPoint = transform.position;
//     }

//     void Update()
//     {
//         // Flip left to right
//         horizontalInput = Input.GetAxisRaw("Horizontal");
        
//         // Horizontal movement
//         if (horizontalInput > 0)
//         {
//             gameObject.transform.localScale = new Vector3(3, 3, 1);
//         }
//         if (horizontalInput < 0)
//         {
//             gameObject.transform.localScale = new Vector3(-3, 3, 1);
//         }

//         // Jumping
//         if(isGrounded() && Input.GetKeyDown(KeyCode.W))
//             Jump();
//         if(wallJumpCooldown > 0)
//             wallJumpCooldown -= Time.deltaTime;
//         else if(wallJumpCooldown <= 0)
//         {
//             rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

//             if(onWall() && !isGrounded() && Input.GetKeyDown(KeyCode.W))
//                 wallJump();

//             if(onWall() && !isGrounded())
//             {
//                 rb.gravityScale = 0;
//                 rb.velocity = Vector2.zero;
//             }
//             else
//                 rb.gravityScale = 2;
//         }

//         // animations
//         anim.SetBool("run", horizontalInput != 0);
//         anim.SetBool("grounded", isGrounded());
//     }
//     private void Jump()
//     {
//         rb.velocity = new Vector2(rb.velocity.x, jump); // Apply jump velocity
//         anim.SetTrigger("jump");
//     }
//     private void wallJump()
//     {
//         if(onWall() && !isGrounded() && wallJumpCooldown <= 0)
//         {
//             wallJumpCooldown = 0.2f;
//             float wallJumpDirection = Mathf.Sign(transform.localScale.x) * -1;
//             rb.velocity = new Vector2(wallJumpDirection * 3, 6);

//             anim.SetTrigger("jump");
//         }
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
//         if (other.CompareTag("Checkpoint"))
//         {
//             respawnPoint = transform.position;
//         }
//         else if (other.CompareTag("Lava"))
//         {
//             transform.position = respawnPoint;
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         if (other.gameObject.CompareTag("Button"))
//         {
//             HandleButtonInteraction(other.collider);
//         }
//         if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Button"))
//         {
//             isJumping = false;
//         }

//         RepresentationTypeChanger representationChanger = other.gameObject.GetComponent<RepresentationTypeChanger>();
//         if (representationChanger != null)
//         {
//             // Cycle the representation type
//             representationChanger.CycleType();

//             // Update the sum output in BinaryArrayAdder
//             if (binaryArrayAdder != null)
//             {
//                 binaryArrayAdder.UpdateSumOutput();
//             }
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
// }