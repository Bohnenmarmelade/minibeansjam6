using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        EventManager.Instance.OnStartGame.Invoke();
    }
}
