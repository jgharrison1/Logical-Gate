using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    public GameObject TruthTable;
    // Start is called before the first frame update
    void Start()
    {
        //TruthTable = GameObject.Find("TruthTable");
    }

    private void OnTriggerEnter2D() {
        TruthTable.GetComponent<TruthTable>().nextGate();
    }
}
