using UnityEngine;

public class GameController : MonoBehaviour
{
    internal static GameController Instance;

    private void Start()
    {
        Instance = this;
    }
}
