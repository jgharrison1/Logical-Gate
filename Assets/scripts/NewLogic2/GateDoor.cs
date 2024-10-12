using UnityEngine;

public class GateController : MonoBehaviour
{
    public ParentGate ConnectedGate;
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float speed = 2f;
    private Vector2 targetPosition;

    void Update()
    {
        if (ConnectedGate != null)
        {
            if (ConnectedGate.output)
            {
                OpenGate();
            }
            else
            {
                CloseGate();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    void OpenGate()
    {
        Vector2 worldOpenPosition = transform.parent.TransformPoint(openPosition);
        targetPosition = worldOpenPosition;
    }

    void CloseGate()
    {
        Vector2 worldClosedPosition = transform.parent.TransformPoint(closedPosition);
        targetPosition = worldClosedPosition;
    }
}

