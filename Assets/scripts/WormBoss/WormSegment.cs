using UnityEngine;

public class WormSegment : MonoBehaviour
{
    public float followSpeed = 5f;
    public float rotationSpeed = 10f;
    private Transform target;

    public int binaryValue = 0;
    public Sprite[] segmentSprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        target = transform.parent ? transform.parent : null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void Update()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

        Vector3 direction = targetPosition - transform.position;
        if (direction.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ToggleBinaryValue()
    {
        binaryValue = (binaryValue == 0) ? 1 : 0;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (spriteRenderer != null && segmentSprites.Length == 2)
        {
            spriteRenderer.sprite = segmentSprites[binaryValue];
        }
    }
}
