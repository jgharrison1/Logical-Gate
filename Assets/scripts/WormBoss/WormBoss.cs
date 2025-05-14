using UnityEngine;
using TMPro;
using System.Collections;

public class WormBoss : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    private int currentWaypointIndex = 0;
    public GameObject segmentPrefab;
    public int segmentCount = 5;
    public float segmentSpacing = 0.5f;
    public float rotationSpeed = 10f;
    private WormSegment[] segments;
    private Rigidbody2D rb2d;
    public int maxHealth = 100;
    private int currentHealth;
    public TextMeshProUGUI healthText;
    [SerializeField] private AudioSource bossMovingSFX;
    [SerializeField] private AudioClip bossDamageSFX;

    public float waitTimeAtWaypoint = 2f;
    private bool isWaiting = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bossMovingSFX = GetComponent<AudioSource>();
        segments = new WormSegment[segmentCount];
        Vector3 spawnPosition = transform.position;
        GameObject previousSegment = null;
        Vector3 headScale = transform.localScale;

        for (int i = 0; i < segmentCount; i++)
        {
            spawnPosition += new Vector3(0, -segmentSpacing, 0);
            GameObject segment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
            segment.transform.position = new Vector3(segment.transform.position.x, segment.transform.position.y, 0);
            segment.transform.localScale = headScale;

            SpriteRenderer spriteRenderer = segment.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Default";
                spriteRenderer.sortingOrder = 0;
            }

            segments[i] = segment.GetComponent<WormSegment>();

            if (previousSegment != null)
            {
                segments[i].SetTarget(previousSegment.transform);
            }

            previousSegment = segment;
        }
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    void Update()
    {
        if (waypoints.Length == 0 || isWaiting) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        MoveHead(direction);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            Waypoint waypointScript = targetWaypoint.GetComponent<Waypoint>();
            if (waypointScript != null && waypointScript.shouldStop)
            {
                StartCoroutine(WaitAtWaypoint(waypointScript));
            }
            else
            {
                MoveToNextWaypoint();
            }
        }
        if(!isWaiting)
        {
            if(!bossMovingSFX.isPlaying)
                bossMovingSFX.Play();
        }
        else
        {
            bossMovingSFX.Stop();
        }
        MoveSegments();
    }

    private void MoveHead(Vector3 direction)
    {
        if (isWaiting)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }

        Vector2 velocity = direction.normalized * speed;
        rb2d.velocity = velocity;

        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb2d.rotation = Mathf.LerpAngle(rb2d.rotation, angle, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveSegments()
    {
        Vector3 previousPosition = transform.position;

        for (int i = 0; i < segments.Length; i++)
        {
            WormSegment segment = segments[i];
            Vector3 direction = previousPosition - segment.transform.position;
            Vector3 targetPosition = previousPosition - direction.normalized * segmentSpacing;
            segment.transform.position = Vector3.MoveTowards(segment.transform.position, targetPosition, speed * Time.deltaTime);
            segment.transform.position = new Vector3(segment.transform.position.x, segment.transform.position.y, 0);
            segment.transform.rotation = Quaternion.Euler(0, 0, 0);
            previousPosition = segment.transform.position;
        }
    }

    private IEnumerator WaitAtWaypoint(Waypoint waypointScript)
    {
        isWaiting = true;
        rb2d.gravityScale = 0f;
        rb2d.velocity = Vector2.zero;
        // Freeze position during wating time
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;

        float timer = 0f;

        while (timer < waitTimeAtWaypoint)
        {
            int platformNumber = waypointScript.GetPlatformNumber();
            int wormBinaryValue = GetSegmentBinaryValue();

            // Check if the worm's binary value == platform's number
            if (!waypointScript.HasDamaged() && wormBinaryValue == platformNumber)
            {
                TakeDamage(25);  
                waypointScript.MarkAsDamaged();  
            }

            timer += Time.deltaTime;
            yield return null;
        }

        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        isWaiting = false;
        rb2d.gravityScale = 1f;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (currentWaypointIndex == waypoints.Length - 1)
        {
            currentWaypointIndex = 0;
        }
        else
        {
            currentWaypointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthDisplay();

        if (currentHealth == 0)
        {
            gameObject.GetComponent<CutsceneTrigger>().triggerCutscene();
            StartCoroutine(DestroyWithDelay());
        }
        SoundFXManager.instance.playSoundFXClip(bossDamageSFX, transform, 1f);
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }


    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = "Worm Health: " + currentHealth;
        }
    }

    public int GetSegmentBinaryValue()
    {
        int decimalValue = 0;
        for (int i = 0; i < segments.Length; i++)
        {
            decimalValue += segments[i].binaryValue * (int)Mathf.Pow(2, segments.Length - i - 1);
        }
        return decimalValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isWaiting)
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    public int GetCurrentWaypointIndex()
    {
        return currentWaypointIndex;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}