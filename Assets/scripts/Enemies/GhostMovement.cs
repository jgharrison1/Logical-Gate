using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    private Vector3 direction;

    void Start()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        FaceDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            if (collision.contacts[0].normal.x != 0)
            {
                direction.x = -direction.x;
            }
            if (collision.contacts[0].normal.y != 0)
            {
                direction.y = -direction.y;
            }
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
