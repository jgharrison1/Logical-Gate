using System.Collections;
using UnityEngine;

public class Health_Enemy : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set;}
    private Animator anim;
    private bool dead;
    [SerializeField] private float damage;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision Normal: " + other.contacts[0].normal);
            if (other.contacts[0].normal.y < 0)
            {
                Debug.Log("Player landed on enemy from above!");
                TakeDamage(1);
            }
            else
            {
                Debug.Log("Player hit from the side or below.");
                other.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }  
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth <= 0)
        {
            if(!dead)
            {
                dead = true;
                StartCoroutine(Death());
            }
        }
    }

    private IEnumerator Death()
    {
        anim.SetTrigger("dies");
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1,0,0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
        }
        gameObject.SetActive(false);
    }
}
