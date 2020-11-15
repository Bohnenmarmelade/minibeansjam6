using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BackgroundController : MonoBehaviour
{
    
    [SerializeField] [Range(10, 120)] private int levelDuration = 100;

    public int LevelDuration => levelDuration;

    [SerializeField] [Range(5f, 50f)] private float transitionToLevelSpeed = 20f;
    private float _scrollingSpeed = 0f;
    
    public Background backgroundStartPrefab;
    public Background backgroundPrefab;
    private Background _levelBackground;
    private Background _startBackground;
        

    private BackgroundState _state = BackgroundState.START;

    private float _camWidth;
    private float _camLeftEdgePosX;
    private float _camRightEdgePosX;


    public void Awake()
    {
        PixelPerfectCamera cam = Camera.main.GetComponent<PixelPerfectCamera>();
        _camWidth = cam.refResolutionX / cam.assetsPPU; //should always result in a whole number
        _camLeftEdgePosX = cam.transform.position.x - (_camWidth * .5f);
        _camRightEdgePosX = _camLeftEdgePosX + _camWidth;
        
        Init();
    }

    public void InstantiateNextBackground()
    {
        _startBackground = Instantiate(backgroundPrefab);
        float nextPosX = _levelBackground.transform.position.x + _levelBackground.BackgroundImageWidth;

        Vector3 nextPos = _startBackground.transform.position;
        nextPos.x = nextPosX + _startBackground.BackgroundImageWidth;

        _startBackground.transform.position = nextPos;

    }

    private void Init()
    {
        //instantiate start background
        _startBackground = Instantiate(backgroundStartPrefab, new Vector2(_camLeftEdgePosX - 1, 0), Quaternion.identity);

        //instantiate level background
        float nextBackgroundPosX = _startBackground.transform.position.x + _startBackground.BackgroundImageWidth;
        _levelBackground = Instantiate(backgroundPrefab, new Vector3(nextBackgroundPosX, 0), Quaternion.identity);

        StartNextLevel();

    }

    private void Update()
    {
        _scrollingSpeed = (_levelBackground.BackgroundImageWidth - _camWidth) / levelDuration;
        if (_state == BackgroundState.TRANSITION_TO_LEVEL)
        {
            TransitionToLevelUpdate();
        } else if (_state == BackgroundState.IN_LEVEL)
        {
            InLevelUpdate();
        } else if (_state == BackgroundState.TRANSITION_TO_INBETWEEN)
        {
            TransitionToInbetweenUpdate();
        } else if (_state == BackgroundState.INBETWEEN)
        {
            
        }
    }

    private void TransitionToInbetweenUpdate()
    {
        if (_startBackground.transform.position.x + 1 < _camLeftEdgePosX)
        {
            //change position of level background
            Vector2 pos = _levelBackground.transform.position;
            pos.x = _startBackground.transform.position.x + _startBackground.BackgroundImageWidth;
            _levelBackground.transform.position = pos;
            
            Debug.Log("transition to inbetween done");
            _state = BackgroundState.INBETWEEN;
        }
        
        float transitionSpeed = transitionToLevelSpeed * Time.deltaTime;
        Vector3 levelPos = _levelBackground.transform.position;
        Vector3 startPos = _startBackground.transform.position;
        levelPos.x -= transitionSpeed;
        startPos.x -= transitionSpeed;
        _levelBackground.transform.position = levelPos;
        _startBackground.transform.position = startPos;
    }

    private void InLevelUpdate()
    {
        float transitionSpeed = _scrollingSpeed * Time.deltaTime;
        Vector3 levelPos = _levelBackground.transform.position;
        Vector3 startPos = _startBackground.transform.position;
        levelPos.x -= transitionSpeed;
        startPos.x -= transitionSpeed;
        _levelBackground.transform.position = levelPos;
        _startBackground.transform.position = startPos;

        //check if end of level is reached & go to next state
        if (_levelBackground.GetRightEdgePosX() - 1 < _camRightEdgePosX)
        {
            // change start background position
            Vector2 pos = _startBackground.transform.position;
            pos.x = _levelBackground.transform.position.x + _levelBackground.BackgroundImageWidth;
            _startBackground.transform.position = pos;

            Debug.Log("Level done");
            EventManager.Instance.OnLevelFinished.Invoke();
            _state = BackgroundState.TRANSITION_TO_INBETWEEN;
        }
    }

    private void TransitionToLevelUpdate()
    {
        if (_levelBackground.transform.transform.position.x - 1 < _camLeftEdgePosX)
        {
            Debug.Log("Transition to level done");
            _state = BackgroundState.IN_LEVEL;
        }
        float transitionSpeed = transitionToLevelSpeed * Time.deltaTime;
        Vector3 levelPos = _levelBackground.transform.position;
        Vector3 startPos = _startBackground.transform.position;
        levelPos.x -= transitionSpeed;
        startPos.x -= transitionSpeed;
        _levelBackground.transform.position = levelPos;
        _startBackground.transform.position = startPos;
    }

    public void StartNextLevel()
    {
        Debug.Log("start next level");
        EventManager.Instance.OnLevelStarted.Invoke();
        _state = BackgroundState.TRANSITION_TO_LEVEL; 
    }

    public void EndThisLevel()
    {
        Debug.Log("End this level");
        EventManager.Instance.OnLevelFinished.Invoke();
        _state = BackgroundState.TRANSITION_TO_INBETWEEN;
    }
}
