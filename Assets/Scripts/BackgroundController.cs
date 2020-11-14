using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BackgroundController : MonoBehaviour
{
    public Background backgroundPrefab;
    private Background _currentBackground;
    private Background _nextBackground;

    public void Awake()
    {
        _currentBackground = Instantiate(backgroundPrefab);
        _currentBackground.IsScrolling = true;
    }

    private void FixedUpdate()
    {
        if (_currentBackground.isNearEnd() && _nextBackground == null)
        {
            //instantiateNextBackground();
            EventManager.Instance.OnLevelFinished.Invoke();
        } 
        else if (_currentBackground.isOutOfCamFocus())
        {
            Destroy(_currentBackground.gameObject);
            _currentBackground = _nextBackground;
            _nextBackground = null;
        }
    }

    public void InstantiateNextBackground()
    {
        _nextBackground = Instantiate(backgroundPrefab);
        float nextPosX = _currentBackground.transform.position.x + _currentBackground.BackgroundImageWidth;

        Vector3 nextPos = _nextBackground.transform.position;
        nextPos.x = nextPosX;

        _nextBackground.transform.position = nextPos;
        _nextBackground.IsScrolling = true;

    }

    public void StartScrolling()
    {
        if (_currentBackground != null)
        {
            _currentBackground.IsScrolling = true;
        }

        if (_nextBackground)
        {
            _nextBackground.IsScrolling = true;
        } 
    }

    public void StopScrolling()
    {
        if (_currentBackground != null)
        {
            _currentBackground.IsScrolling = false;
        }
        
        if (_nextBackground)
        {
            _nextBackground.IsScrolling = false;
        } 
    }
}
