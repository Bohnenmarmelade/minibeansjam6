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
    public GameObject playerPrefab;
    private GameObject _currentPlayer;
    private GameObject _nextPlayer;

    private Vector2 _nextPlayerMeetPosition = Vector2.zero;
    private Vector2 _currentPlayerMeetPosition = new Vector2(-5, 0);
    
    private BackgroundController _backgroundController;

    private int _level = 1;
    private bool _playerIsAdult = false;

    private bool _transition = false;
    
    private void Awake()
    {
        _backgroundController = GetComponent<BackgroundController>();
        _currentPlayer = Instantiate(playerPrefab, new Vector2(-5, 0), Quaternion.identity);
        
        EventManager eventManager = EventManager.Instance;
        eventManager.OnPlayerDeath.AddListener(HandlePlayerDeath);
        eventManager.OnLevelFinished.AddListener(HandleLevelFinished);
        eventManager.OnPlayerAdult.AddListener(HandleAdult);
    }

    private void HandleLevelFinished()
    {
        if (_playerIsAdult)
        {
            Debug.Log("level finished!");
            _level++;
            _playerIsAdult = false;
            
            //instantiate next whale
            _nextPlayer = Instantiate(playerPrefab, new Vector2(20, 0), quaternion.identity);
            _nextPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;

            _currentPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;

            _transition = true;
        }
        else
        {
            Debug.Log("you lost!");
        }
        
    }

    private void Update()
    {
        if (_transition)
        {
            if (Vector2.Distance(_nextPlayer.transform.position, _nextPlayerMeetPosition) < .1)
            {
                _transition = false;
                
                //give control over next whale && start next level
                _currentPlayer.GetComponent<PlayerMovement>().TransitionToDie = true;
                Destroy(_currentPlayer, .5f);
                _currentPlayer = _nextPlayer;
                _nextPlayer = null;
                _currentPlayer.GetComponent<PlayerMovement>().PlayerHasControl = true;
                _backgroundController.StartNextLevel();
                return;
            }
            
            if (!_currentPlayer.GetComponent<PlayerMovement>().TransitionToDie) {            
                var currentPos = _currentPlayer.transform.position;
                currentPos = Vector2.MoveTowards(currentPos, _currentPlayerMeetPosition, 5 *Time.deltaTime);
                _currentPlayer.transform.position = currentPos;
            }
            
            var nextPos = _nextPlayer.transform.position;
            nextPos = Vector2.MoveTowards(nextPos, _nextPlayerMeetPosition, 5 * Time.deltaTime);
            _nextPlayer.transform.position = nextPos;
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
            
        //instantiate next whale
        _nextPlayer = Instantiate(playerPrefab, new Vector2(20, 0), quaternion.identity);
        _nextPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;

        _currentPlayer.GetComponent<PlayerMovement>().PlayerHasControl = false;
        _currentPlayer.GetComponent<PlayerMovement>().TransitionToDie = true;

        _transition = true;

        _backgroundController.EndThisLevel();

    }

    public int LevelDuration()
    {
        return _backgroundController.LevelDuration;
    }
}
