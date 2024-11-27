using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;

    private Vector2 targetPosition;
    private bool shouldMove = false;

    void Start()
    {
        targetPosition = closedPosition;
    }

    void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                shouldMove = false;
                transform.position = targetPosition;
            }
        }
    }

    public void MoveElevator(Vector2 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        shouldMove = true;
    }

    public void OpenElevator()
    {
        targetPosition = openPosition;
        shouldMove = true;
    }

    public void CloseElevator()
    {
        targetPosition = closedPosition;
        shouldMove = true;
    }
}
