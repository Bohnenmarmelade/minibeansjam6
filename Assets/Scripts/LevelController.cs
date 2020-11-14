using System.Net.NetworkInformation;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    
    private BackgroundController _backgroundController;

    private int _level = 1;
    private bool _isInLevel = false;
    
    private void Awake()
    {
        _backgroundController = GetComponent<BackgroundController>();
        
        EventManager eventManager = EventManager.Instance;
        eventManager.OnPlayerDeath.AddListener(HandlePlayerDeath);
        eventManager.OnLevelFinished.AddListener(HandleLevelFinished);
    }

    private void HandleLevelFinished()
    {
        _backgroundController.StartNextLevel();
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("player died. reached level: " + _level);
        //_backgroundController.StopScrolling(); 
    }

    public int LevelDuration()
    {
        return _backgroundController.LevelDuration;
    }
}
