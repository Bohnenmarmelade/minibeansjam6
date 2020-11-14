using UnityEngine;

public class LevelController : MonoBehaviour
{
    private BackgroundController _backgroundController;

    private void Awake()
    {
        _backgroundController = GetComponent<BackgroundController>();
    }
}
