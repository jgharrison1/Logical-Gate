using UnityEngine;

public class ElevatorButtonController : MonoBehaviour
{
    public ElevatorController elevatorController;
    public string playerTag = "Player";
    
    private SpriteRenderer spriteRenderer;
    private bool isElevatorActive = false;
    public Color activatedColor = Color.green;
    public Color deactivatedColor = Color.red;

    public Vector3 buttonOffset;  // Offset from the elevator's position

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateButtonColor();
    }

    private void Update()
    {
        // Update the button's position based on the elevator's position and the offset
        if (elevatorController != null)
        {
            transform.position = elevatorController.transform.position + buttonOffset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isElevatorActive = !isElevatorActive;

            if (isElevatorActive)
            {
                elevatorController.OpenElevator();
            }
            else
            {
                elevatorController.CloseElevator();
            }
            UpdateButtonColor();
        }
    }

    private void UpdateButtonColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isElevatorActive ? activatedColor : deactivatedColor;
        }
    }
}
