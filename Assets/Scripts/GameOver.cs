using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(0);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
