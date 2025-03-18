using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDataPersistence
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
    //soundFX
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip deathSFX;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0)
        {
            StartCoroutine(Invulnerability());
            SoundFXManager.instance.playSoundFXClip(damageSFX, transform, 1f);
        }
        else 
        {
            if(!dead)
            {
                anim.SetTrigger("dies");
                dead = true;
                //Play sound effect playerHurt
                SoundFXManager.instance.playSoundFXClip(deathSFX, transform, 1f);
                StartCoroutine(Death());
            }
        }
    }
    public void RestoreHealth()
    {
        currentHealth = startingHealth;
        dead = false;
    }

    private IEnumerator Death()
    {
        SoundFXManager.instance.playSoundFXClip(deathSFX, transform, 1f);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        FindObjectOfType<playerMovement>()?.Respawn();
    }


    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(7,8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1,0,0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes*2));
        }
        Physics2D.IgnoreLayerCollision(7,8, false);
    }

    public void LoadData(GameData data) 
    {
        this.currentHealth = data.playerHealth;
    }

    public void SaveData(GameData data) 
    {
        data.playerHealth = this.currentHealth;
    }
}
