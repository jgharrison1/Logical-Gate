using UnityEngine;

public class GateController : MonoBehaviour
{
    public AndLogicGate andLogicGate;
    public Vector3 openPosition;
    public Vector3 closedPosition;
    public float speed = 2f;
    public bool gateOutput;
    private Vector3 targetPosition;

    void Update()
    {
        if (andLogicGate != null)
        {
            if (andLogicGate.output)
            {
                gateOutput = andLogicGate.output;
                OpenGate();
            }
            else
            {
                gateOutput = andLogicGate.output;
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
