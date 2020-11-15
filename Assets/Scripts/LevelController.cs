using System;
using System.Net.NetworkInformation;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LevelController : MonoBehaviour
{
    public int levelDuration = 60;
    public int minLevelDuration = 20;
    public int levelDurationDecrease = 10;
    
    public GameObject playerPrefab;
    public GameObject pinkyPrefab;
    
    private GameObject _currentPlayer;
    private GameObject _nextPlayer;
    private GameObject _pinky;
    
    
    private Vector2 _nextPlayerMeetPosition = Vector2.zero;
    private Vector2 _currentPlayerMeetPosition = new Vector2(-2, 0);
    
    
    private BackgroundController _backgroundController;

    private int _level = 1;
    private bool _playerIsAdult = false;

    private bool _transition = false;
    private bool _prepareNextPlayer = false;
    private float _pinkyMeetTime = 0;
    
    private void Awake()
    {
        _backgroundController = GetComponent<BackgroundController>();
        _backgroundController.LevelDuration = levelDuration;
        _currentPlayer = Instantiate(playerPrefab, new Vector2(-2, 0), Quaternion.identity);
        
        EventManager eventManager = EventManager.Instance;
        eventManager.OnPlayerDeath.AddListener(HandlePlayerDeath);
        eventManager.OnLevelFinished.AddListener(HandleLevelFinished);
        eventManager.OnPlayerAdult.AddListener(HandleAdult);
        
        eventManager.OnStartGame.Invoke();
    }

    private void HandleLevelFinished()
    {
        if (_playerIsAdult)
        {
            Debug.Log("level finished!");
            _level++;
            _playerIsAdult = false;
            
            //instantiate next whale
            Debug.Log("PINKY!");
            _pinky = Instantiate(pinkyPrefab, new Vector2(20, 0), quaternion.identity);
            
            //_nextPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;

            _currentPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;

            _transition = true;

            var l = _backgroundController.LevelDuration;
            if (l - levelDurationDecrease > minLevelDuration)
            {
                _backgroundController.LevelDuration = l - levelDurationDecrease;
            }
                
        }
        else
        {
            Debug.Log("you lost!");
            _backgroundController.LevelDuration = levelDuration;
        }


    }

    private void Update()
    {
        if (_transition && !_prepareNextPlayer)
        {
            if (Vector2.Distance(_pinky.transform.position, _nextPlayerMeetPosition) < .1)
            {
                _transition = false;
                _nextPlayer = Instantiate(playerPrefab, new Vector3(-2, -1), Quaternion.identity);
                _nextPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;
                _pinkyMeetTime = Time.time;
                _prepareNextPlayer = true;
                return;
            }
            
            if (!_currentPlayer.GetComponent<PlayerMovement>().TransitionToDie) {            
                var currentPos = _currentPlayer.transform.position;
                currentPos = Vector2.MoveTowards(currentPos, _currentPlayerMeetPosition, 5 *Time.deltaTime);
                _currentPlayer.transform.position = currentPos;
            }
            
            var nextPos = _pinky.transform.position;
            nextPos = Vector2.MoveTowards(nextPos, _nextPlayerMeetPosition, 5 * Time.deltaTime);
            _pinky.transform.position = nextPos;
        } else if (_prepareNextPlayer)
        {
            if (Time.time - _pinkyMeetTime > 1)
            {
                
                Debug.Log(1);
                
                _currentPlayer.GetComponent<PlayerMovement>().TransitionToDie = true;
                _pinky.GetComponent<PinkyMovement>().TransitionToDie = true;
                Destroy(_currentPlayer, 1f);
                Destroy(_pinky, 1f);
                _currentPlayer = _nextPlayer;
                _nextPlayer = null;
                _pinky = null;
                _currentPlayer.GetComponent<PlayerMovement>().PlayerHasControl = true;
                _backgroundController.StartNextLevel();

                _prepareNextPlayer = false;
                _transition = false;
                
                _backgroundController.StartNextLevel();
                
                Debug.Log(2);
            }   
        }
    }

    private void HandleAdult()
    {
        _playerIsAdult = true;
    }
    private void HandlePlayerDeath()
    {
        Debug.Log("player died. reached level: " + _level);
        _level = 0;
        _playerIsAdult = false;
    }

    public int LevelDuration()
    {
        return _backgroundController.LevelDuration;
    }
}
