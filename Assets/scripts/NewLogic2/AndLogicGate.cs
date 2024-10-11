using UnityEngine;

public class AndLogicGate : MonoBehaviour
{
    public bool input1;
    public bool input2;
    public bool output;

    public GameObject input1ButtonObject;
    public GameObject input2ButtonObject;
    public string playerTag = "Player";

    void Update()
    {
        ProcessAndGate();
    }

    void ProcessAndGate()
    {
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
