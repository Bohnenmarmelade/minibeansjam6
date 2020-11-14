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
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("player died. reached level: " + _level);
        //_backgroundController.StopScrolling(); 
    }
}
