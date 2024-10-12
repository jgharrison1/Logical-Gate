using UnityEngine;

public class GateController : MonoBehaviour
{
    public ParentGate ConnectedGateGate;
    public Vector3 openPosition;
    public Vector3 closedPosition;
    public float speed = 2f;
    public bool gateOutput;
    private Vector3 targetPosition;

    void Update()
    {
        if (ConnectedGateGate != null)
        {
            if (ConnectedGateGate.output)
            {
                gateOutput = ConnectedGateGate.output;
                OpenGate();
            }
            else
            {
                gateOutput = ConnectedGateGate.output;
                CloseGate();
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

    }

    void OpenGate()
    {
        targetPosition = openPosition;
    }

    void CloseGate()
    {
        targetPosition = closedPosition;
    }
}
