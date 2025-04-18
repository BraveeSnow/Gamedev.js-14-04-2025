using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private static readonly int RESTAURANT_SCENE = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(RESTAURANT_SCENE);
    }
}
