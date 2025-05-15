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
                if(previousSceneIndex == 2 || previousSceneIndex == 6 || previousSceneIndex == 9){
                    SceneManager.LoadSceneAsync("Hub");
                }
                
                else SceneManager.LoadSceneAsync(previousSceneIndex);
            }
        }
    }
}
