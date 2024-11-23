using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit lava");
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}  