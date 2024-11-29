using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;
    private float stuckTimer = 0f;
    private float stuckThreshold = 1f;
    private int collisionCount = 0;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (Mathf.Abs(direction.x) > 0.1f || Mathf.Abs(direction.y) > 0.1f)
        {
            stuckTimer = 0f;
        }
        else
        {
            stuckTimer += Time.deltaTime;
        }
        if (stuckTimer > stuckThreshold)
        {
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            stuckTimer = 0f;
        }
        FaceDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.x != 0)
        {
            direction.x = -direction.x; 
            collisionCount++;
        }
        if (collision.contacts[0].normal.y != 0)
        {
            direction.y = -direction.y;
            collisionCount++;
        }

        if (collisionCount > 10)
        {
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            collisionCount = 0;
        }
    }

    void FaceDirection()
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
