using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _initialBackgroundPos = Vector2.zero;
    
    public GameObject backgroundPrefab;
    
    private BackgroundController _backgroundController;

    private void Awake()
    {
        _backgroundController = Instantiate(backgroundPrefab, _initialBackgroundPos, Quaternion.identity).GetComponent<BackgroundController>();
        _backgroundController.IsScrolling = true;

    }

    private void FixedUpdate()
    {
        if (_backgroundController.isNearEnd())
        {
            _backgroundController.IsScrolling = false;
        }
    }
}
