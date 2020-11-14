using UnityEngine;

public class LevelController : MonoBehaviour
{
    private BackgroundController _backgroundController;

    private int _level = 1;
    
    private void Awake()
    {
        _backgroundController = GetComponent<BackgroundController>();
        EventManager.Instance.OnLevelFinished.AddListener(HandleLevelEnd);
    }

    private void HandleLevelEnd()
    {
        Debug.Log("reached end of level");
        _level++;
        _backgroundController.InstantiateNextBackground();
    }
}
