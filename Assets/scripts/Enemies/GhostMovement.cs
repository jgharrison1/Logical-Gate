using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private float stuckTimer = 0f;
    private float stuckThreshold = 0.5f;
    private BinaryArrayAdder binaryArrayAdder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomDirection();
        lastPosition = transform.position;
    }

    void Update()
    {
        rb.velocity = direction * speed;

        if (Vector2.Distance(lastPosition, transform.position) < 0.1f)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }
        if (stuckTimer > stuckThreshold)
        {
            SetRandomDirection();
            stuckTimer = 0f;
        }

        lastPosition = transform.position;

        FaceDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SetRandomDirection();
        // Handle representation change if applicable
        RepresentationTypeChanger representationChanger = collision.gameObject.GetComponent<RepresentationTypeChanger>();
        if (representationChanger != null)
        {
            representationChanger.CycleType();

            if (binaryArrayAdder != null)
            {
                binaryArrayAdder.UpdateSumOutput();
            }
        }
    }

    void SetRandomDirection()
    {
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void FaceDirection()
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
