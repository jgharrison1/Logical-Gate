using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private Health_Enemy healthEnemy;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        healthEnemy = GetComponent<Health_Enemy>();
    }
    private void Update()
    {
        if (healthEnemy != null && healthEnemy.currentHealth <= 0)
        {
            return;
        }
        if(movingLeft)
        {
            if(transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                gameObject.transform.localScale = new Vector3(3, 3, 1);
            }
            else
                movingLeft = false;
        }
        else
        {
            if(transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                gameObject.transform.localScale = new Vector3(-3, 3, 1);

            }
            else
                movingLeft = true;
        }
    }
}
