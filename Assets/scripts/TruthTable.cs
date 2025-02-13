using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TruthTable : MonoBehaviour
{
    public TextMeshPro I1_1;
    public TextMeshPro I1_2;
    public TextMeshPro I1_3;
    public TextMeshPro I1_4;
    public TextMeshPro I2_1;
    public TextMeshPro I2_2;
    public TextMeshPro I2_3;
    public TextMeshPro I2_4;
    public TextMeshPro O1;
    public TextMeshPro O2;
    public TextMeshPro O3;
    public TextMeshPro O4;
    private int cycleNum = 0;
    public List<GameObject> gates;
    public GameObject gateTransform;
    private Vector3 previous;

    // Start is called before the first frame update
    void Start()
    {
        /*
        I1_1.text = "";
        I1_2.text = "";
        I1_3.text = "";
        I1_4.text = "";
        I2_1.text = "";
        I2_2.text = "";
        I2_3.text = "";
        I2_4.text = "";
        O1.text = "";
        O2.text = "";
        O3.text = "";
        O4.text = "";
        */
        AndGate();
        previous = gates[0].transform.position;
        
    }

    public void nextGate() {
        gates[cycleNum%8].transform.position = previous;
        cycleNum++;
        previous = gates[cycleNum%8].transform.position;
        switch(cycleNum % 8) {
            case 0:
                AndGate();
                gates[0].transform.position = gateTransform.transform.position;
                break;
            case 1:
                OrGate();
                gates[1].transform.position = gateTransform.transform.position;
                break;
            case 2:
                XorGate();
                gates[2].transform.position = gateTransform.transform.position;
                break;
            case 3:
                NandGate();
                gates[3].transform.position = gateTransform.transform.position;
                break;
            case 4:
                NorGate();
                gates[4].transform.position = gateTransform.transform.position;
                break;
            case 5:
                XnorGate();
                gates[5].transform.position = gateTransform.transform.position;
                break;
            case 6:
                NotGate();
                gates[6].transform.position = gateTransform.transform.position;
                break;
            case 7:
                BufferGate();
                gates[7].transform.position = gateTransform.transform.position;
                break;
            default:
                Debug.Log("cycleNum is not in range. - TruthTable");
                cycleNum = 0;
                break;
        }
    }

    public void AndGate() {
        I1_1.text = "T";
        I1_2.text = "T";
        I1_3.text = "F";
        I1_4.text = "F";
        I2_1.text = "T";
        I2_2.text = "F";
        I2_3.text = "T";
        I2_4.text = "F";
        O1.text = "T";
        O2.text = "F";
        O3.text = "F";
        O4.text = "F";
    }

    public void OrGate() {
        I1_1.text = "T";
        I1_2.text = "T";
        I1_3.text = "F";
        I1_4.text = "F";
        I2_1.text = "T";
        I2_2.text = "F";
        I2_3.text = "T";
        I2_4.text = "F";
        O1.text = "T";
        O2.text = "T";
        O3.text = "T";
        O4.text = "F";
    }

    public void XorGate() {
        I1_1.text = "T";
        I1_2.text = "T";
        I1_3.text = "F";
        I1_4.text = "F";
        I2_1.text = "T";
        I2_2.text = "F";
        I2_3.text = "T";
        I2_4.text = "F";
        O1.text = "F";
        O2.text = "T";
        O3.text = "T";
        O4.text = "F";
    }

    public void NandGate() {
        I1_1.text = "T";
        I1_2.text = "T";
        I1_3.text = "F";
        I1_4.text = "F";
        I2_1.text = "T";
        I2_2.text = "F";
        I2_3.text = "T";
        I2_4.text = "F";
        O1.text = "F";
        O2.text = "T";
        O3.text = "T";
        O4.text = "T";
    }

    public void NorGate() {
        I1_1.text = "T";
        I1_2.text = "T";
        I1_3.text = "F";
        I1_4.text = "F";
        I2_1.text = "T";
        I2_2.text = "F";
        I2_3.text = "T";
        I2_4.text = "F";
        O1.text = "F";
        O2.text = "F";
        O3.text = "F";
        O4.text = "T";
    }

    public void XnorGate() {
        I1_1.text = "T";
        I1_2.text = "T";
        I1_3.text = "F";
        I1_4.text = "F";
        I2_1.text = "T";
        I2_2.text = "F";
        I2_3.text = "T";
        I2_4.text = "F";
        O1.text = "T";
        O2.text = "F";
        O3.text = "F";
        O4.text = "T";
    }

    public void NotGate() {
        I1_1.text = "T";
        I1_2.text = "F";
        I1_3.text = "";
        I1_4.text = "";
        I2_1.text = "";
        I2_2.text = "";
        I2_3.text = "";
        I2_4.text = "";
        O1.text = "F";
        O2.text = "T";
        O3.text = "";
        O4.text = "";
    }

    public void BufferGate() {
        I1_1.text = "T";
        I1_2.text = "F";
        I1_3.text = "";
        I1_4.text = "";
        I2_1.text = "";
        I2_2.text = "";
        I2_3.text = "";
        I2_4.text = "";
        O1.text = "T";
        O2.text = "F";
        O3.text = "";
        O4.text = "";
    }
}
