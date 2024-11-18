using UnityEngine;

public class WormPlatformMover : MonoBehaviour
{
    public WormBoss wormBoss;
    public Vector3 openPositionOffset;
    public float moveSpeed = 2f;

    private Vector3 openPosition;
    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool shouldMove = false;

    void Start()
    {
        closedPosition = transform.position;        
        openPosition = closedPosition + openPositionOffset;
        targetPosition = closedPosition;
    }

    void Update()
    {
        if (wormBoss != null && wormBoss.GetCurrentHealth() <= 0 && !shouldMove)
        {
            shouldMove = true;
            targetPosition = openPosition;
        }

        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                shouldMove = false;
            }
        }
    }
}
