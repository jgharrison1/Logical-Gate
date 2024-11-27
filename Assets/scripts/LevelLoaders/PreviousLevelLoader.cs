using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousLevelLoader : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int previousSceneIndex = currentSceneIndex - 1;
            
            // Check if previous scene exists to avoid errors
            if (previousSceneIndex >= 0)
            {
                SceneManager.LoadSceneAsync(previousSceneIndex);
            }
        }
    }
}
