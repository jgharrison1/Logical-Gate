using System.Collections;
using UnityEngine;

public class Health_Enemy : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set;}
    private Animator anim;
    private bool dead;
    [Header ("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
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

    private void Update() // Practice Take Damage
    {
        if(Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);
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
    }
}
