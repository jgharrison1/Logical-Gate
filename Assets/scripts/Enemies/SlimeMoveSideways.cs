using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private Health_Enemy healthEnemy;
    private BinaryArrayAdder binaryArrayAdder;

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
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
                movingLeft = false;
        }
        else
        {
            if(transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                gameObject.transform.localScale = new Vector3(-1, 1, 1);

            }
            else
                movingLeft = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        foreach (ContactPoint2D contact in other.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y))
            {
                movingLeft = !movingLeft;
                break;
            }
        }

        RepresentationTypeChanger representationChanger = other.gameObject.GetComponent<RepresentationTypeChanger>();
        if (representationChanger != null)
        {
            representationChanger.CycleType();

            if (binaryArrayAdder != null)
            {
                binaryArrayAdder.UpdateSumOutput();
            }
        }
    }

    
}
