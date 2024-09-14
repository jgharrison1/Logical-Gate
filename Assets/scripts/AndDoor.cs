using UnityEngine;

public class AndDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Vector3 openPosition;
    public Vector3 closedPosition;
    public float speed = 2f;

    private void Update()
    {
        Vector3 targetPosition;
        if (isOpen)
        {
            targetPosition = openPosition;
        }
        else
        {
            targetPosition = closedPosition;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void ClosedDoor()
    {
        isOpen = false;
    }
}
