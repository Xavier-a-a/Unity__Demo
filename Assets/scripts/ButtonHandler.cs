using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
