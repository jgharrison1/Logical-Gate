using System.Collections;
using UnityEngine;

public class Health_Enemy : MonoBehaviour, IDataPersistence
{
    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set;}
    private Animator anim;
    private bool dead;
    [SerializeField] private float damage;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private string ID;
    private SpriteRenderer spriteRend;

    [ContextMenu("Generate guid for ID")]
    private void generateGuid() 
    {
        ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Player") && !dead)
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
        if (other.gameObject.CompareTag("Button"))
        {
            HandleButtonInteraction(other.collider);
        }
    }  

    private void HandleButtonInteraction(Collider2D buttonCollider)
    {
        ButtonIndex buttonIndex = buttonCollider.GetComponent<ButtonIndex>();

        if (buttonIndex != null && buttonIndex.buttonArray != null)
        {
            int index = buttonIndex.index;
            BinaryButtonArray buttonArrayManager = buttonIndex.buttonArray;

            buttonArrayManager.ToggleBinaryValue(index, buttonArrayManager.arrayID);
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

    public void LoadData(GameData data) 
    {
        data.enemiesDefeated.TryGetValue(ID, out dead);
        if (dead)
        {
            this.gameObject.SetActive(false); //Should deactivate enemy if dead = true
        }
    }

    public void SaveData(GameData data) 
    {
        if (data.enemiesDefeated.ContainsKey(ID)) {
            data.enemiesDefeated.Remove(ID);
        }
        data.enemiesDefeated.Add(ID, dead);
    }
}
