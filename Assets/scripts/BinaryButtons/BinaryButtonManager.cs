using UnityEngine;

public class BinaryButtonManager : MonoBehaviour
{
    public GameObject binaryButtonPrefab; // Assign your prefab in the Inspector
    public int numberOfInstances = 5; // Number of instances to create
    public Vector3 initialPosition; // Starting position for the first instance
    public float spacing = 2.0f; // Space between instances

    private void Start()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            // Calculate position for each instance
            Vector3 position = initialPosition + new Vector3(i * spacing, 0, 0);
            // Instantiate a new BinaryButtonArray
            Instantiate(binaryButtonPrefab, position, Quaternion.identity);
        }
    }
}
