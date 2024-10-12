using UnityEngine;

public class AndLogicGate : MonoBehaviour
{
    public bool input1;
    public bool input2;
    public bool output;
    public string gateName1;
    public string gateName2;

    public GameObject input1ButtonObject;
    public GameObject input2ButtonObject;
    public string playerTag = "Player";

    public AndLogicGate previousGate1;
    public AndLogicGate previousGate2;

    void Update()
    {
        ProcessAndGate();
    }

    void ProcessAndGate()
    {
        if(previousGate1 != null)
        {
            input1 = previousGate1.output;
        }

        if(previousGate2 != null)
        {
            input2 = previousGate2.output;
        }

        output = input1 && input2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && other.gameObject == input1ButtonObject)
        {
            input1 = !input1;
        }
        else if (other.CompareTag(playerTag) && other.gameObject == input2ButtonObject)
        {
            input2 = !input2;
        }
    }
}
