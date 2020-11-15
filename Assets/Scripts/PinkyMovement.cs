using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private bool _transitionToDie = false;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public bool TransitionToDie
    {
        get => _transitionToDie;
        set => _transitionToDie = value;
    }

    void Update()
    {
        if (_transitionToDie)
        {
            _rigidbody2D.AddForce(Vector2.left * 20f);
        }

    }
    
}
